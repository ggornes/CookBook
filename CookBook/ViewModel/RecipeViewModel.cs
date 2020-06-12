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
using System.Windows.Input;

namespace CookBook.ViewModel
{
    public class RecipeViewModel: INotifyPropertyChanged
    {
        private DbActions dbActions;
        public ObservableCollection<Recipe> recipeItems { get; set; }

        private ObservableCollection<RecipeStep> _steps;
        public ObservableCollection<RecipeStep> steps
        {
            get { return _steps; }
            set { if(_steps != value)
                {
                    _steps = value;
                    OnPropertyChanged();
                } }
        }
        public ObservableCollection<RecipeIngredient> ingredientItems { get; set; }

        public ObservableCollection<RecipeIngredient> recipeIngredientItems { get; set; }

        private Recipe _selectedRecipe;


        public Recipe selectedRecipe
        {
            get { return _selectedRecipe; }
            set
            {
                _selectedRecipe = value;
                OnPropertyChanged();

                // read steps and Ingredients when recipe is selected
                if (selectedRecipe != null)
                {
                    var allRecipeSteps = this.dbActions.BrowseRecipeSteps(selectedRecipe.Id);
                    steps = new ObservableCollection<RecipeStep>(
                        allRecipeSteps.Select(obj2 =>
                        {
                            var recipeStep = (CookBookData.Model.RecipeStep)obj2;
                            return new RecipeStep { stepNumber = recipeStep.stepNumber, stepInstructions = recipeStep.stepInstructions };
                        })
                        );
                }

            }
        }

        public RecipeViewModel()
        {
            dbActions = new DbActions();

            ReadRecipeCommand = new RelayCommand(ReadRecipe);

            
            // read form database and store all recipes in a variable
            var allRecipes = this.dbActions.BrowseRecipes();

            // update ingredient ListView
            recipeItems = new ObservableCollection<Recipe>(
                allRecipes.Select(obj =>
                {
                    var recipe = (CookBookData.Model.Recipe)obj;
                    return new Recipe { Id = recipe.Id, name = recipe.name, prepTime = recipe.prepTime };
                })
                );

            // read recipeIngredients table
            var allRecipeIngredients = dbActions.BrowseRecipeIngredients();

            ingredientItems = new ObservableCollection<RecipeIngredient>(
                allRecipeIngredients.Select(obj =>
                {
                    var ing = (CookBookData.Model.RecipeIngredient)obj;
                    return new RecipeIngredient { Id = ing.Id, recipeId = ing.recipeId, ingredientId = ing.ingredientId, measureId = ing.measureId, amount = ing.amount };
                })
                );

        }

        void ReadRecipe(object obj)
        {
            

        }












        public ICommand ReadRecipeCommand { get; set; }


        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        #endregion
    }
}
