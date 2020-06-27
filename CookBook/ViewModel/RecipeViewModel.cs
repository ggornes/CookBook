using CookBook.View;
using CookBook.ViewModel.Interfaces;
using CookBookData;
using CookBookData.Model;
using CookBookData.Model.DbActions;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CookBook.ViewModel
{
    public class RecipeViewModel: IViewModel, INotifyPropertyChanged
    {
        private DbActions dbActions;

        // Recipe - Ingredient List View
        // The Source is binded to the selectedRecipe
        private ObservableCollection<RecipeIngredientItem> _ingredientItems { get; set; }
        public ObservableCollection<RecipeIngredientItem> ingredientItems
        { 
            get { return _ingredientItems; }
            set { if(_ingredientItems != value)
                    {
                    _ingredientItems = value;
                        OnPropertyChanged();
                    
                    // Make Edit Recipe-Ingredients controls visible if an element of the ListView is selected
                    if(ingredientItems.Count() == 0)
                    {
                        ShowControlsRecipeIngredientSelected = false;
                    }
                }
                }
        }

        // Ingredients Combo Box
        private ObservableCollection<Ingredient> _allIngredientItems { get; set; }
        public ObservableCollection<Ingredient> allIngredientItems
        {
            get { return _allIngredientItems; }
            set
            {
                if (_allIngredientItems != value)
                {
                    _allIngredientItems = value;
                    OnPropertyChanged();
                }
            }
        }

        // Measures combo box
        private ObservableCollection<Measure> _allMeasureItems { get; set; }
        public ObservableCollection<Measure> allMeasureItems
        {
            get { return _allMeasureItems; }
            set
            {
                if (_allMeasureItems != value)
                {
                    _allMeasureItems = value;
                    OnPropertyChanged();
                }
            }
        }

        // Recipe - Step Listview
        private ObservableCollection<RecipeStep> _steps;
        public ObservableCollection<RecipeStep> steps
        {
            get { return _steps; }
            set { if(_steps != value)
                    {
                        _steps = value;
                        OnPropertyChanged();
                    Console.WriteLine("steps {0}", steps.Count());
                    if (steps.Count() == 0)
                    {
                        ShowControlsRecipeStepSelected = false;
                    }
                    }
                }
        }

        // show controls

        private bool _showControls;

        public bool ShowControls
        {
            get { return _showControls; }
            set
            {
                if(_showControls != value)
                {
                    _showControls = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _showControlsRecipeStepSelected;
        public bool ShowControlsRecipeStepSelected
        {
            get { return _showControlsRecipeStepSelected; }
            set
            {
                if ( _showControlsRecipeStepSelected != value)
                {
                    _showControlsRecipeStepSelected = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _showControlsRecipeIngredientSelected;
        public bool ShowControlsRecipeIngredientSelected
        {
            get { return _showControlsRecipeIngredientSelected; }
            set
            {
                if (_showControlsRecipeIngredientSelected != value)
                {
                    _showControlsRecipeIngredientSelected = value;
                    OnPropertyChanged();
                }
            }
        }

        // Recipe List View
        private ObservableCollection<Recipe> _searchedRecipeItems { get; set; }
        public ObservableCollection<Recipe> searchedRecipeItems
        {
            get
            {
                if (_searchedRecipeItems == null)
                {
                    _searchedRecipeItems = new ObservableCollection<Recipe>();
                }

                return _searchedRecipeItems;
            }
            set
            {
                _searchedRecipeItems = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<RecipeIngredientItem> recipeIngredientItems { get; set; }

        private Recipe _selectedRecipe;

        public Recipe selectedRecipe
        {
            get { return _selectedRecipe; }
            set
            {
                _selectedRecipe = value;
                OnPropertyChanged();

                
                if (selectedRecipe != null)
                {
                    ShowControls = true;

                    // read steps and Ingredients when recipe is selected
                    var allRecipeSteps = this.dbActions.BrowseRecipeSteps(selectedRecipe.Id);
                    steps = new ObservableCollection<RecipeStep>(
                        allRecipeSteps.Select(obj2 =>
                        {
                            var recipeStep = (CookBookData.Model.RecipeStep)obj2;
                            return new RecipeStep { Id = recipeStep.Id, stepNumber = recipeStep.stepNumber, stepInstructions = recipeStep.stepInstructions };
                        })
                        );

                    // read recipeIngredients table when recipe is selected
                    var allRecipeIngredients = dbActions.BrowseRecipeIngredients(selectedRecipe.Id);

                    ingredientItems = new ObservableCollection<RecipeIngredientItem>(
                        allRecipeIngredients.Select(obj =>
                        {
                            var ing = (CookBookData.RecipeIngredientItem)obj;
                            return new RecipeIngredientItem { Id = ing.Id, ingredientName = ing.ingredientName, amount = ing.amount, measure = ing.measure };
                        })
                        );

                    // Read all Ingredients
                    var allIngredients = dbActions.BrowseIngredients();

                    allIngredientItems = new ObservableCollection<Ingredient>(
                        allIngredients.Select(obj3 =>
                        {
                            var ingredient = (CookBookData.Model.Ingredient)obj3;
                            return new Ingredient { Id = ingredient.Id, name = ingredient.name };
                        })
                        );

                    // Read All Measures
                    var allMeasures = dbActions.BrowseMeasures();
                    allMeasureItems = new ObservableCollection<Measure>(
                        allMeasures.Select(obj4 =>
                        {
                            var measure = (CookBookData.Model.Measure)obj4;
                            return new Measure { Id = measure.Id, name = measure.name };
                        })
                        );
                }
            }
        }

        // Selected Recipe Step
        private RecipeStep _selectedRecipeStep;
        public RecipeStep selectedRecipeStep
        {
            get { return _selectedRecipeStep; }
            set
            {
                _selectedRecipeStep = value;
                OnPropertyChanged();

                // To make controls visible, one must select first a recipe step from
                // the ListView
                if (selectedRecipe != null)
                {
                    ShowControlsRecipeStepSelected = true;
                }
            }
        }

        // Selected Recipe Ingredient from Recipe-Ingredient ListView (ObservableCollection<RecipeIngredient>)
        private RecipeIngredientItem _selectedRecipeIngredient;
        public RecipeIngredientItem selectedRecipeIngredient
        {
            get { return _selectedRecipeIngredient; }
            set
            {
                _selectedRecipeIngredient = value;
                OnPropertyChanged();
                if (selectedRecipe != null)
                {
                    if (selectedRecipeIngredient != null)
                    {
                        Console.WriteLine(selectedRecipeIngredient.ingredientName);
                        ShowControlsRecipeIngredientSelected = true;

                        var readRecipeIngredient = this.dbActions.ReadIngredient(new CookBookData.Model.Ingredient { name = selectedRecipeIngredient.ingredientName });

                        //var readRecipeIngredient2 = this.dbActions.ReadIngredient(new CookBookData.Model.Ingredient { name = selectedRecipeIngredient.ingredientName }, new CookBookData.Model.Recipe{});

                        trueSelectedRecipeIngredient = (Ingredient)readRecipeIngredient;

                        Console.WriteLine(selectedRecipeIngredient.measure);

                        var readRecipeMeasure = this.dbActions.ReadMeasure(new CookBookData.Model.Measure { name = selectedRecipeIngredient.measure });

                        selectedRecipeMeasure = (Measure)readRecipeMeasure;
                    }
                    

                }
                
            }
        }

        // Selected Ingredient
        private Ingredient _trueSelectedRecipeIngredient;
        public Ingredient trueSelectedRecipeIngredient
        {
            get { return _trueSelectedRecipeIngredient; }
            set
            {
                _trueSelectedRecipeIngredient = value;
                OnPropertyChanged();
            }
        }


        // Selected Measure
        private Measure _selectedRecipeMeasure;
        public Measure selectedRecipeMeasure
        {

            get { return _selectedRecipeMeasure; }
            set
            {
                _selectedRecipeMeasure = value;
                OnPropertyChanged();
            }
        }

        // Search Results
        private ObservableCollection<Recipe> _recipeItems;
        public ObservableCollection<Recipe> recipeItems
        {
            get
            {
                if (_recipeItems == null)
                {
                    _recipeItems = new ObservableCollection<Recipe>();
                }

                searchedRecipeItems = new ObservableCollection<Recipe>(_recipeItems);

                return _recipeItems;
            }

            set
            {
                _recipeItems = value;
                OnPropertyChanged();
            }
        }

        // Search String
        private string _searchString;
        public string searchString
        {
            get
            {
                if (!string.IsNullOrEmpty(_searchString) || !string.IsNullOrWhiteSpace(_searchString))
                {
                    searchedRecipeItems = new ObservableCollection<Recipe>(recipeItems.Where(r => r.name.ToLower().Contains(_searchString.ToLower())));
                }
                else
                {
                    searchedRecipeItems = new ObservableCollection<Recipe>(recipeItems);
                }
                return _searchString;
            }
            set
            {
                _searchString = value;
                OnPropertyChanged();
            }
        }

        // Sort Recipes
        private ObservableCollection<string> _sortValues;
        public ObservableCollection<string> sortValues
        {
            get { return _sortValues; }
            set
            {
                _sortValues = value;
                OnPropertyChanged();
            }
        }

        private string _selectedSortValue;
        public string selectedSortValue
        {
            get
            {
                if (_selectedSortValue == "prepTime")
                {
                    searchedRecipeItems = new ObservableCollection<Recipe>(searchedRecipeItems.OrderBy(r => r.prepTime));
                }
                else if (_selectedSortValue == "name")
                {
                    searchedRecipeItems = new ObservableCollection<Recipe>(searchedRecipeItems.OrderBy(r => r.name));
                }
                else
                {
                    searchedRecipeItems = new ObservableCollection<Recipe>(searchedRecipeItems.OrderBy(r => r.Id));
                }

                return _selectedSortValue;
            }

            set
            {
                _selectedSortValue = value;
                OnPropertyChanged();
            }
        }






        public RecipeViewModel()
        {
            // Load Data Base methods for doing BREAD/CRUD operations
            dbActions = new DbActions();

            #region Button Action Binding
            EditRecipeCommand = new RelayCommand(EditRecipe);
            EditRecipeStepCommand = new RelayCommand(EditRecipeStep);
            EditRecipeIngredientCommand = new RelayCommand(EditIRecipeIngredient);

            AddRecipeCommand = new RelayCommand(OpenAddRecipeWindow);
            AddRecipeStepCommand = new RelayCommand(OpenAddRecipeStepWindow);
            AddRecipeIngredientCommand = new RelayCommand(OpenAddRecipeIngredientWindow);


            DeleteRecipeStepCommand = new RelayCommand(DeleteRecipeStep);
            DeleteRecipeIngredientCommand = new RelayCommand(DeleteRecipeIngredient);
            DeleteRecipeCommand = new RelayCommand(DeleteRecipe);
            #endregion


            // Hides AddIngredient and AddStep buttons until user clicks on a recipe from the list
            _showControls = false;
            // Make Edit Controls only visible when selecting a recipeStep item from the ListView
            _showControlsRecipeStepSelected = false;


            // read form database and store all recipes in a variable
            var allRecipes = this.dbActions.BrowseRecipes();

            // update Recipe ListView
            recipeItems = new ObservableCollection<Recipe>(
                allRecipes.Select(obj =>
                {
                    var recipe = (CookBookData.Model.Recipe)obj;
                    return new Recipe { Id = recipe.Id, name = recipe.name, prepTime = recipe.prepTime, favorite = recipe.favorite };
                })
                );

            // sort
            sortValues = new ObservableCollection<string> { "Id", "name", "prepTime"};
            //selectedSortValue = sortValues[0];
        }




        #region Add 
        // Open a new Window view to Add Recipes, Recipe-Ingredients or Recipe-Steps

        public ICommand AddRecipeCommand { get; set; }
        private void OpenAddRecipeWindow(object obj)
        {
            var AddRecipeVM = new AddRecipeViewModel(dbActions, searchedRecipeItems);
            var addRecipeView = new AddRecipeView(AddRecipeVM);

            addRecipeView.Show();
        }

        public ICommand AddRecipeIngredientCommand { get; set; }

        private void OpenAddRecipeIngredientWindow(object obj)
        {
            if (selectedRecipe != null)
            {
                var AddRecipeIngredientVM = new AddRecipeIngredientViewModel(dbActions, allIngredientItems, allMeasureItems, ingredientItems, selectedRecipe.Id);
                var AddRecipeIngredientV = new AddRecipeIngredientView(AddRecipeIngredientVM);

                AddRecipeIngredientV.Show();
            }
            else
            {
                MessageBox.Show("Please select a Recipe from the list first", "Invalid operation", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }


        public ICommand AddRecipeStepCommand { get; set; }

        private void OpenAddRecipeStepWindow(object obj)
        {
            if (selectedRecipe != null)
            {
                var AddRecipeStepVM = new AddRecipeStepViewModel(dbActions, steps, selectedRecipe.Id);
                var AddRecipeStepV = new AddRecipeStepView(AddRecipeStepVM);

                AddRecipeStepV.Show();
            }
            else
            {
                MessageBox.Show("Please select a Recipe from the list first", "Invalid operation", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }


        #endregion

        #region Delete

        public ICommand DeleteRecipeIngredientCommand { get; set; }

        private void DeleteRecipeIngredient(object obj)
        {
            if (selectedRecipeIngredient != null)
            {
                if (MessageBox.Show("Do you want to delete the selected recipe ingredient?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
                {
                    if (dbActions.DeleteRecipeIngredient(new CookBookData.Model.RecipeIngredient { Id = selectedRecipeIngredient.Id }))
                    {
                        ingredientItems.Remove(selectedRecipeIngredient);
                    }
                }
            }
        }

        public ICommand DeleteRecipeCommand { get; set; }
        private void DeleteRecipe(object obj)
        {
            if (selectedRecipe != null)
            {
                if (MessageBox.Show("Do you want to delete the selected recipe ?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
                {
                    if (dbActions.DeleteRecipe(new CookBookData.Model.Recipe { Id = selectedRecipe.Id }))
                    {
                        recipeItems.Remove(selectedRecipe);
                        searchedRecipeItems.Remove(selectedRecipe);
                        // ToDo: clear Recipe-Ingredient and Recipe-Steps listviews
                        ingredientItems.Clear();
                        steps.Clear();
                    }
                }
            }
        }

        public ICommand DeleteRecipeStepCommand { get; set; }

        private void DeleteRecipeStep(object obj)
        {
            if (selectedRecipeStep != null)
            {
                if (MessageBox.Show("Do you want to delete the selected recipe Step?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
                {
                    if (dbActions.DeleteRecipeStep(new CookBookData.Model.RecipeStep { Id = selectedRecipeStep.Id }))
                    {
                        steps.Remove(selectedRecipeStep);
                    }
                }
            }
        }

        #endregion

        #region Edit

        public ICommand EditRecipeCommand { get; set; }

        void EditRecipe(object obj)
        {
            if (selectedRecipe != null && !string.IsNullOrEmpty(selectedRecipe.name) && !string.IsNullOrWhiteSpace(selectedRecipe.name))
            {
                if (dbActions.EditRecipe(new CookBookData.Model.Recipe { Id = selectedRecipe.Id, name = selectedRecipe.name, prepTime = selectedRecipe.prepTime, favorite = selectedRecipe.favorite }))
                {
                    MessageBox.Show("Recipe updated", "Recipe updated", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                }
            }

        }

        public ICommand EditRecipeIngredientCommand { get; set; }

        void EditIRecipeIngredient(object obj)
        {
            // quick validations on the Edit Recipe-Ingredient controls
            if (
                selectedRecipeIngredient != null &&
                !string.IsNullOrEmpty(selectedRecipeIngredient.ingredientName) && !string.IsNullOrWhiteSpace(selectedRecipeIngredient.ingredientName) &&
                !string.IsNullOrEmpty(selectedRecipeIngredient.measure) && !string.IsNullOrWhiteSpace(selectedRecipeIngredient.measure)
                )
            {
                var test = new CookBookData.Model.RecipeIngredient {Id = selectedRecipeIngredient.Id, recipeId=selectedRecipe.Id, ingredientId = trueSelectedRecipeIngredient.Id, measureId = selectedRecipeMeasure.Id, amount = selectedRecipeIngredient.amount };

                if (dbActions.EditRecipeIngredient(test))
                {
                    MessageBox.Show("Recipe Ingredient updated", "Recipe Ingredient updated", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                    // ToDo: reload the recipeIngredients listview
                    // read recipeIngredients table when recipe is selected
                    var allRecipeIngredients = dbActions.BrowseRecipeIngredients(selectedRecipe.Id);

                    ingredientItems = new ObservableCollection<RecipeIngredientItem>(
                        allRecipeIngredients.Select(obj4 =>
                        {
                            var ing = (CookBookData.RecipeIngredientItem)obj4;
                            return new RecipeIngredientItem { Id = ing.Id, ingredientName = ing.ingredientName, amount = ing.amount, measure = ing.measure };
                        })
                        );
                }
            }
        }

        public ICommand EditRecipeStepCommand { get; set; }

        void EditRecipeStep(object obj)
        {
            if (selectedRecipeStep != null && !string.IsNullOrEmpty(selectedRecipeStep.stepInstructions) && !string.IsNullOrWhiteSpace(selectedRecipeStep.stepInstructions))
            {
                //var test = new CookBookData.Model.RecipeStep { Id = selectedRecipeStep.Id, recipeId = selectedRecipe.Id, stepNumber = selectedRecipeStep.stepNumber, stepInstructions = selectedRecipeStep.stepInstructions };

                if (dbActions.EditRecipeStep(new CookBookData.Model.RecipeStep { Id = selectedRecipeStep.Id, recipeId = selectedRecipe.Id, stepNumber = selectedRecipeStep.stepNumber, stepInstructions = selectedRecipeStep.stepInstructions }))
                {
                    MessageBox.Show("Recipe Step updated", "Recipe Step updated", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                }
            }
        }
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        #endregion
    }
}
