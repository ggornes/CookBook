using CookBook.ViewModel.Interfaces;
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

        private Ingredient _selectedIngredient;


        public Ingredient selectedIngredient
        {
            get { return selectedIngredient; }
            set
            {
                _selectedIngredient = value;
                OnPropertyChanged();
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
            
            void ReadRecipe(object obj)
            {
                // Read from database and store collections in variables

                //var recipeIngredientItems = this.dbActions.BrowseRecipeIngredients();
                //var recipeStepItems = this.dbActions.BrowseRecipeSteps();

                // The List views on the Recipe View have different properties than the
                // RecipeIngredient and RecipeStep Classes in the Models
                // We are going to use a new UI Model to represent this objects
                //
                // RecipeIngredientsItem and RecipeStepsItem
            }
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
