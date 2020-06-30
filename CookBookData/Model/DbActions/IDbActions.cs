using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookBookData.Model.DbActions
{
    public interface IDbActions
    {
        object[] BrowseIngredients();
        bool DeleteIngredient(Ingredient ingredient);
        bool EditIngredient(Ingredient ingredient);
        bool AddIngredient(Ingredient ingredient);
        object ReadIngredient(Ingredient ingredient);

        object[] BrowseMeasures();
        bool DeleteMeasure(Measure measure);
        bool EditMeasure(Measure measure);
        bool AddMeasure(Measure measure);
        object ReadMeasure(Measure measure);

        bool DeleteRecipeStep(RecipeStep recipeStep);
        object ReadRecipeStep(RecipeStep recipeStep);
        bool AddRecipeStep(RecipeStep recipeStep);
        object[] BrowseRecipeSteps();
        object[] BrowseRecipeSteps(int selectedRecipeId);
        bool EditRecipeStep(RecipeStep recipeStep);

        object[] BrowseRecipes();

        bool EditRecipe(Recipe recipe);
        bool AddRecipe(Recipe recipe);
        object ReadRecipe(Recipe recipe);
        bool DeleteRecipe(Recipe recipe);


        object[] BrowseRecipeIngredients();
        List<object> BrowseRecipeIngredients(int selectedRecipeId);
        object ReadRecipeIngredient(RecipeIngredientItem recipeIngredientItem);
        object ReadRecipeIngredient(Recipe selectedRecipe, Ingredient selectedRecipeIngredient);
        bool AddRecipeIngredient(RecipeIngredient ri);
        bool DeleteRecipeIngredient(RecipeIngredient ri);
        bool EditRecipeIngredient(RecipeIngredient ri);
    }
}
