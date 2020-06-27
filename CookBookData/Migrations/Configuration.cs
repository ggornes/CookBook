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
                new Ingredient { Id = 7, name = "butter" },
                new Ingredient { Id = 8, name = "paprika" },
                new Ingredient { Id = 9, name = "garlic powder" },
                new Ingredient { Id = 10, name = "cayenne pepper" },
                new Ingredient { Id = 11, name = "thyme" },
                new Ingredient { Id = 12, name = "oregano" },
                new Ingredient { Id = 13, name = "pork ribs" },
                new Ingredient { Id = 14, name = "apple cider" },
                new Ingredient { Id = 15, name = "olive oil" }

            });

            context.Measures.AddOrUpdate(new Measure[]
            {
                new Measure { Id = 1, name = "Cup" },
                new Measure { Id = 2, name = "Tea spoon" },
                new Measure { Id = 3, name = "Table spoon" },
                new Measure { Id = 4, name = "Grams" },
                new Measure { Id = 5, name = "Kg" }
            });

            context.Recipes.AddOrUpdate(new Recipe[]
            {
                new Recipe {Id = 1, name = "Chocolate Cake", prepTime=60},
                new Recipe {Id = 2, name = "Chocolate Brownie", prepTime=40},
                new Recipe {Id = 3, name = "BBQ Pork Ribs", prepTime=120}
            });

            context.RecipeSteps.AddOrUpdate(new RecipeStep[]
            {
                new RecipeStep {Id = 1, recipeId = 1, stepNumber = 1, stepInstructions = "Add eggs, flour, chocolate to pan"},
                new RecipeStep {Id = 2, recipeId = 1, stepNumber = 2, stepInstructions = "Bake at 350 for 1 hour"},
                new RecipeStep {Id = 3, recipeId = 2, stepNumber = 1, stepInstructions = "Preheat oven 180C. Grease a square base"},
                new RecipeStep {Id = 4, recipeId = 2, stepNumber = 2, stepInstructions = "Melt butter an chocolate"},
                new RecipeStep {Id = 5, recipeId = 2, stepNumber = 3, stepInstructions = "Pour into prepared pan. Bake for 30 minutes or until a skewer inserted in the centre comes out with moist crumbs clinging"},
                new RecipeStep {Id = 6, recipeId = 3, stepNumber = 1, stepInstructions = "Combine the Rub ingredients and rub onto both sides of the ribs (most on meaty side). Set aside to marinate for 20 minutes (or overnight)."},
                new RecipeStep {Id = 7, recipeId = 3, stepNumber = 2, stepInstructions = "Preheat oven to 160°C/320°F (all oven types)."},
                new RecipeStep {Id = 8, recipeId = 3, stepNumber = 3, stepInstructions = ""},
                new RecipeStep {Id = 9, recipeId = 3, stepNumber = 4, stepInstructions = "Place ribs on a tray in a single layer. Pour apple cider underneath the ribs, cover with foil then bake for 1 hour 30 minutes or until the meat is pretty tender"},
                new RecipeStep {Id = 10, recipeId = 3, stepNumber = 5, stepInstructions = "Remove from oven, turn up to 180°C/350°F. Remove foil, drizzle with olive oil, then return ribs to oven for 15 minutes or until rub becomes nice and crusty."},
                new RecipeStep {Id = 11, recipeId = 3, stepNumber = 6, stepInstructions = "Line a new tray with foil then baking / parchment paper"},
                new RecipeStep {Id = 12, recipeId = 3, stepNumber = 7, stepInstructions = "Remove ribs from oven, transfer to lined tray. Pour any juices from tray over the ribs."},
                new RecipeStep {Id = 13, recipeId = 3, stepNumber = 8, stepInstructions = "Flip ribs so the bonier side is up. Slather with Barbecue Sauce, then bake 10 minutes."},
                new RecipeStep {Id = 14, recipeId = 3, stepNumber = 9, stepInstructions = "Remove from oven, then turn ribs over so the meaty side is up. Slather with Barbecue Sauce, bake 5 minutes. Repeat 2 or 3 more times until you've got a thick glaze on the ribs."},
                new RecipeStep {Id = 15, recipeId = 3, stepNumber = 10, stepInstructions = "Cut ribs into individual or multiple rib portions and serve with remaining Barbcue Sauce!"},

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
                
                new RecipeIngredient {Id = 10, recipeId = 3, ingredientId = 5, measureId = 2, amount = 1},
                new RecipeIngredient {Id = 11, recipeId = 3, ingredientId = 5, measureId = 2, amount = 1},
                new RecipeIngredient {Id = 12, recipeId = 3, ingredientId = 5, measureId = 2, amount = 1},
                new RecipeIngredient {Id = 13, recipeId = 3, ingredientId = 5, measureId = 2, amount = 1},
                new RecipeIngredient {Id = 14, recipeId = 3, ingredientId = 5, measureId = 2, amount = 1},
                new RecipeIngredient {Id = 15, recipeId = 3, ingredientId = 5, measureId = 2, amount = 1},
                new RecipeIngredient {Id = 16, recipeId = 3, ingredientId = 5, measureId = 2, amount = 1},
                new RecipeIngredient {Id = 17, recipeId = 3, ingredientId = 5, measureId = 2, amount = 1},
                new RecipeIngredient {Id = 18, recipeId = 3, ingredientId = 5, measureId = 2, amount = 1},
                new RecipeIngredient {Id = 19, recipeId = 3, ingredientId = 5, measureId = 2, amount = 1},

            });



        }
    }
}
