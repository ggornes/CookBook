using CookBook.View;
using CookBook.ViewModel.Interfaces;
using CookBookData;
using CookBookData.Model;
using CookBookData.Model.DbActions;
using System;
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

        // ingredientItems is the source of the Ingredients ListView.
        // The itemSource is binded to the selectedRecipe
        private ObservableCollection<RecipeIngredientItem> _ingredientItems { get; set; }
        public ObservableCollection<RecipeIngredientItem> ingredientItems
        { 
            get { return _ingredientItems; }
            set { if(_ingredientItems != value)
                    {
                    _ingredientItems = value;
                        OnPropertyChanged();
                    if(ingredientItems.Count() == 0)
                    {
                        ShowControlsRecipeIngredientSelected = false;
                    }
                }
                }
        }

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


        public ObservableCollection<Recipe> recipeItems { get; set; }

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
                            return new RecipeIngredientItem { ingredientName = ing.ingredientName, amount = ing.amount, measure = ing.measure };
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
                }
            }
        }

        private RecipeStep _selectedRecipeStep;
        public RecipeStep selectedRecipeStep
        {
            get { return _selectedRecipeStep; }
            set
            {
                _selectedRecipeStep = value;
                OnPropertyChanged();
                if (selectedRecipe != null )
                {
                    ShowControlsRecipeStepSelected = true;
                }                    
            }
        }

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
                    ShowControlsRecipeIngredientSelected = true;
                }
            }
        }



        public RecipeViewModel()
        {
            dbActions = new DbActions();

            EditRecipeCommand = new RelayCommand(EditRecipe);
            EditRecipeStepCommand = new RelayCommand(EditRecipeStep);
            AddRecipeCommand = new RelayCommand(OpenAddRecipeWindow);

            _showControls = false;
            _showControlsRecipeStepSelected = false;




            // read form database and store all recipes in a variable
            var allRecipes = this.dbActions.BrowseRecipes();

            // update ListView
            recipeItems = new ObservableCollection<Recipe>(
                allRecipes.Select(obj =>
                {
                    var recipe = (CookBookData.Model.Recipe)obj;
                    return new Recipe { Id = recipe.Id, name = recipe.name, prepTime = recipe.prepTime };
                })
                );

            

        }

        private void OpenAddRecipeWindow(object obj)
        {
            var AddRecipeVM = new AddRecipeViewModel(dbActions, recipeItems);
            var addRecipeView = new AddRecipeView(AddRecipeVM);

            addRecipeView.Show();
        }

        void EditRecipe(object obj)
        {
            if (selectedRecipe != null)
            {
                ShowControls = true;
            }

        }

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












        public ICommand EditRecipeCommand { get; set; }
        public ICommand AddRecipeCommand { get; set; }
        public ICommand EditRecipeStepCommand { get; set; }


        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        #endregion
    }
}
