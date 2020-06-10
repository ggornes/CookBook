namespace CookBookData.Migrations
{
    using CookBookData.Model;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CookBookData.Model.DbContext.CookBookContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(CookBookData.Model.DbContext.CookBookContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.

            context.Ingredients.AddOrUpdate(new Ingredient[]
            {
                new Ingredient { Id = 1, name = "egg" },
                new Ingredient { Id = 2, name = "salt" },
                new Ingredient { Id = 3, name = "sugar" },
                new Ingredient { Id = 4, name = "chocolate" },
                new Ingredient { Id = 5, name = "vanilla extract" },
                new Ingredient { Id = 6, name = "flour" },
                new Ingredient { Id = 7, name = "butter" }
            });

            context.Measures.AddOrUpdate(new Measure[]
            {
                new Measure { Id = 1, name = "Cup" },
                new Measure { Id = 2, name = "Tea spoon" },
                new Measure { Id = 3, name = "Table spoon" },
                new Measure { Id = 4, name = "Grams" }
            });

            context.Recipes.AddOrUpdate(new Recipe[]
            {
                new Recipe {Id = 1, name = "Chocolate Cake", prepTime=60},
                new Recipe {Id = 2, name = "Chocolate Brownie", prepTime=40}
            });

            context.RecipeSteps.AddOrUpdate(new RecipeStep[]
            {
                new RecipeStep {Id = 1, recipeId = 1, stepNumber = 1, stepInstructions = "Add eggs, flour, chocolate to pan"},
                new RecipeStep {Id = 2, recipeId = 1, stepNumber = 2, stepInstructions = "Bake at 350 for 1 hour"},
                new RecipeStep {Id = 3, recipeId = 2, stepNumber = 1, stepInstructions = "Preheat oven 180C. Grease a square base"},
                new RecipeStep {Id = 4, recipeId = 2, stepNumber = 2, stepInstructions = "Melt butter an chocolate"},
                new RecipeStep {Id = 5, recipeId = 2, stepNumber = 3, stepInstructions = "Pour into prepared pan. Bake for 30 minutes or until a skewer inserted in the centre comes out with moist crumbs clinging"},

            });

            context.RecipeIngredients.AddOrUpdate(new RecipeIngredient[]
            {
                new RecipeIngredient {Id = 1, recipeId = 1, ingredientId = 1, measureId = null, amount = 3},
                new RecipeIngredient {Id = 2, recipeId = 1, ingredientId = 2, measureId = 2, amount = 3},
                new RecipeIngredient {Id = 3, recipeId = 1, ingredientId = 3, measureId = 1, amount = 2},
                new RecipeIngredient {Id = 4, recipeId = 1, ingredientId = 4, measureId = 1, amount = 1},
                new RecipeIngredient {Id = 5, recipeId = 2, ingredientId = 7, measureId = 4, amount = 125},
                new RecipeIngredient {Id = 6, recipeId = 2, ingredientId = 4, measureId = 4, amount = 125 },
                new RecipeIngredient {Id = 7, recipeId = 2, ingredientId = 1, measureId = null, amount = 3},
                new RecipeIngredient {Id = 8, recipeId = 2, ingredientId = 3, measureId = 4, amount = 335},
                new RecipeIngredient {Id = 9, recipeId = 2, ingredientId = 5, measureId = 2, amount = 1},

            });



        }
    }
}
