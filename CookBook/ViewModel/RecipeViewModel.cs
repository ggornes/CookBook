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


        public ObservableCollection<Recipe> recipeItems { get; set; }

        public ObservableCollection<RecipeIngredient> recipeIngredientItems { get; set; }

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
                            return new RecipeStep { stepNumber = recipeStep.stepNumber, stepInstructions = recipeStep.stepInstructions };
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
            }
        }



        public RecipeViewModel()
        {
            dbActions = new DbActions();

            EditRecipeCommand = new RelayCommand(EditRecipe);

            EditRecipeStepCommand = new RelayCommand(EditRecipeStep);

            _showControls = false;

            


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
                if (dbActions.EditRecipeStep(new CookBookData.Model.RecipeStep { Id = selectedRecipe.Id, recipeId = selectedRecipe.Id, stepNumber = selectedRecipeStep.stepNumber, stepInstructions = selectedRecipeStep.stepInstructions }))
                {
                    MessageBox.Show("Recipe Step updated", "Recipe Step updated", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                }
            }
        }












        public ICommand EditRecipeCommand { get; set; }

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
