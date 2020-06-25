using CookBookData.Model.DbContext;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookBookData.Model.DbActions
{
    public class DbActions
    {
        private CookBookContext _cookBookContext;

        public static MySqlConnection GetMySqlConnection()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CookBookContext"].ConnectionString;
            MySqlConnection connection = new MySqlConnection(connectionString);
            return connection;
        }

        #region Recipe
        public bool EditRecipe(Recipe recipe)
        {
            if (recipe == null) return false;

            using (var context = new CookBookContext())
            {
                try
                {
                    context.Entry(recipe).State = EntityState.Modified;
                    context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Could not edit recipe");
                    Console.WriteLine(ex.Message);

                    return false;
                }
            }
        }

        public bool AddRecipe(Recipe recipe)
        {
            if (recipe == null) { return false; }

            using (var context = new CookBookContext())
            {
                try
                {
                    context.Entry(recipe).State = EntityState.Added;
                    context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Could not add new Recipe");
                    Console.WriteLine(ex.Message);

                    return false;
                }
            }
        }

        public object ReadRecipe(Recipe recipe)
        {
            if (recipe == null) return false;

            using (var context = new CookBookContext())
            {
                try
                {
                    return context.Recipes.FirstOrDefault(e => e.Id == recipe.Id || e.name == recipe.name);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Could not read recipe");
                    Console.WriteLine(ex.Message);

                    return null;
                }
            }
        }

        #endregion

        #region RecipeIngredients
        public List<object> BrowseRecipeIngredients(int selectedRecipeId)
        {
            List<object> allRecipeIngredients = new List<object>();
            using (MySqlConnection connection = GetMySqlConnection())
            {

                try
                {
                    connection.Open();
                    string queryRecipeIngredients = "SELECT i.name AS 'Ingredient', ri.Id AS 'RI Id', ri.amount AS 'Amount', mu.name AS 'Unit of Measure' FROM recipes r JOIN recipe_ingredients ri on r.id = ri.recipeId JOIN ingredients i on i.id = ri.ingredientId LEFT OUTER JOIN measures mu on mu.id = measureId WHERE r.id = @Id;";
                    MySqlCommand getIngredients = new MySqlCommand(queryRecipeIngredients, connection);
                    getIngredients.Parameters.Add(new MySqlParameter("Id", selectedRecipeId));
                    MySqlDataReader ingredientsReader;
                    ingredientsReader = getIngredients.ExecuteReader();
                    while (ingredientsReader.Read())
                    {
                        
                        String ingredientName = ingredientsReader.GetString(0);
                        int Id = ingredientsReader.GetInt32(1);
                        int amount = ingredientsReader.GetInt32(2);
                        String measure = (ingredientsReader.IsDBNull(3)) ? "" : ingredientsReader.GetString(3);
                        //if(!ingredientsReader.IsDBNull(2))





                        // create instance of RecipeStepsUIModel and add it to a list or array
                        var recipeIngredient = new RecipeIngredientItem { Id = Id, ingredientName = ingredientName, amount = amount, measure = measure };
                        allRecipeIngredients.Add(recipeIngredient);

                        // recipeStepList.push(recipeStep)

                        //Console.WriteLine("Step #: {0} | Step Instruction: {1}", stepNumber, stepInstruction);




                    }
                    ingredientsReader.Close();
                    connection.Close();

                    //return new object[] { };
                    return allRecipeIngredients;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error connecting to database: " + e.Message);
                    //return new object[] { };
                    return new List<object> { };
                }
            }
        }

        public object ReadRecipeIngredient(RecipeIngredientItem recipeIngredientItem)
        {
            if (recipeIngredientItem == null) return false;

            using (var context = new CookBookContext())
            {
                try
                {
                    return context.Ingredients.FirstOrDefault(e => e.name == recipeIngredientItem.ingredientName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Could not read Recipe Ingredient");
                    Console.WriteLine(ex.Message);

                    return null;
                }
            }
        }

        // get the RecipeIngredient from both, recipeId and ingredientId
        public object ReadRecipeIngredient(Recipe selectedRecipe, Ingredient selectedRecipeIngredient)
        {
            if (selectedRecipe == null || selectedRecipeIngredient == null) return false;
            using (var context = new CookBookContext())
            {
                try
                {
                    return context.RecipeIngredients.FirstOrDefault(e => (e.Id == selectedRecipe.Id && e.ingredientId == selectedRecipeIngredient.Id));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Could not read Recipe Ingredient");
                    Console.WriteLine(ex.Message);

                    return null;
                }
            }

        }
        #endregion

        #region RecipeSteps

        public bool DeleteRecipeStep(RecipeStep recipeStep)
        {
            if (recipeStep == null) return false;

            using (var context = new CookBookContext())
            {
                try
                {
                    context.Entry(recipeStep).State = EntityState.Deleted;
                    context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Could not delete recipe Step");
                    Console.WriteLine(ex.Message);

                    return false;
                }
            }
        }

        public object ReadRecipeStep(RecipeStep recipeStep)
        {
            if (recipeStep == null) return false;

            using (var context = new CookBookContext())
            {
                try
                {
                    return context.RecipeSteps.FirstOrDefault(e => e.Id == recipeStep.Id || e.stepInstructions == recipeStep.stepInstructions);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Could not read Recipe step");
                    Console.WriteLine(ex.Message);

                    return null;
                }
            }
        }

        public bool AddRecipeStep(RecipeStep recipeStep)
        {
            if(recipeStep == null) { return false; }

            using (var context = new CookBookContext())
            {
                try
                {
                    context.Entry(recipeStep).State = EntityState.Added;
                    context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Could not add new Recipe Step");
                    Console.WriteLine(ex.Message);

                    return false;
                }
            }
        }

        
        public object[] BrowseRecipeSteps()
        {
            using (var context = new CookBookContext())
            {
                try
                {
                    object result = context.RecipeSteps.ToArray();

                    return context.RecipeSteps.ToArray();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error connecting to database: " + e.Message);
                    return new object[] { };
                }
            }
        }
        public object[] BrowseRecipeSteps(int selectedRecipeId)
        {
            Console.WriteLine("Cliecked");

            using (var context = new CookBookContext())
            {
                try
                {
                    object result = context.RecipeSteps.Where(rs => rs.recipeId == selectedRecipeId).ToArray();

                    return context.RecipeSteps.Where(rs => rs.recipeId == selectedRecipeId).ToArray();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error connecting to database: " + e.Message);
                    return new object[] { };
                }
            }

            //using (MySqlConnection connection = GetMySqlConnection())
            //{
                
            //    try
            //    {
            //        connection.Open();
            //        string querySteps = "SELECT rs.stepNumber, rs.stepInstructions FROM recipes r JOIN recipe_steps rs on r.id = rs.recipeId WHERE r.id = @Id";
            //        MySqlCommand getSteps = new MySqlCommand(querySteps, connection);
            //        getSteps.Parameters.Add(new MySqlParameter("Id", selectedRecipeId));
            //        MySqlDataReader stepsReader;
            //        stepsReader = getSteps.ExecuteReader();
            //        while (stepsReader.Read())
            //        {
            //            int stepNumber = stepsReader.GetInt32(0);
            //            String stepInstruction = stepsReader.GetString(1);


            //            // create instance of RecipeStepsUIModel and add it to a list or array
            //            var recipeStep = new RecipeStepItem { stepNumber=stepNumber, stepInstruction=stepInstruction};

            //            // recipeStepList.push(recipeStep)

            //            //Console.WriteLine("Step #: {0} | Step Instruction: {1}", stepNumber, stepInstruction);



                        
            //        }
            //        stepsReader.Close();
            //        connection.Close();

            //        return new object[] { };
            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine("Error connecting to database: " + e.Message);
            //        return new object[] { };
            //    }
            //}

        }


        public bool EditRecipeIngredient(RecipeIngredient ri)
        {
            if (ri == null) return false;

            using (var context = new CookBookContext())
            {
                try
                {
                    context.Entry(ri).State = EntityState.Modified;
                    context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Could not edit Recipe Ingredient");
                    Console.WriteLine(ex.Message);

                    return false;
                }
            }
        }


        public bool EditRecipeStep(RecipeStep step)
        {
            if (step == null) return false;

            using (var context = new CookBookContext())
            {
                try
                {
                    context.Entry(step).State = EntityState.Modified;
                    context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Could not edit Recipe Step");
                    Console.WriteLine(ex.Message);

                    return false;
                }
            }
        }


            public object[] BrowseRecipes()
        {
            using (var context = new CookBookContext())
            {
                try
                {
                    return context.Recipes.ToArray();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Could not retrieve Recipes");
                    Console.WriteLine(ex.Message);

                    return new object[] { };
                }
            }
        }

        #endregion

        #region Measures
        public object[] BrowseMeasures()
        {

            using (var context = new CookBookContext())
            {
                try
                {
                    return context.Measures.ToArray();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Could not retrieve measure");
                    Console.WriteLine(ex.Message);

                    return new object[] { };
                }
            }

        }

        public bool DeleteMeasure(Measure measure)
        {
            if (measure == null) return false;

            using (var context = new CookBookContext())
            {
                try
                {
                    context.Entry(measure).State = EntityState.Deleted;
                    context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Could not delete measure");
                    Console.WriteLine(ex.Message);

                    return false;
                }
            }
        }

        public bool EditMeasure(Measure measure)
        {
            if (measure == null) return false;

            using (var context = new CookBookContext())
            {
                try
                {
                    context.Entry(measure).State = EntityState.Modified;
                    context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Could not edit measure");
                    Console.WriteLine(ex.Message);

                    return false;
                }
            }
        }

        public bool AddMeasure(Measure measure)
        {
            if (measure == null) { return false; }

            using (var context = new CookBookContext())
            {
                try
                {
                    context.Entry(measure).State = EntityState.Added;
                    context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Could not add new measure");
                    Console.WriteLine(ex.Message);

                    return false;
                }
            }
        }

        public object ReadMeasure(Measure measure)
        {
            if (measure == null) return false;

            using (var context = new CookBookContext())
            {
                try
                {
                    var test = context.Measures.FirstOrDefault(e => e.name == measure.name);
                    return context.Measures.FirstOrDefault(e => e.Id == measure.Id || e.name == measure.name);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Could not read measure");
                    Console.WriteLine(ex.Message);

                    return null;
                }
            }
        }

        #endregion

        #region Ingredient
        public object[] BrowseIngredients()
        {

            using (var context = new CookBookContext())
            {
                try
                {
                    return context.Ingredients.ToArray();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Could not retrieve ingredients");
                    Console.WriteLine(ex.Message);

                    return new object[] { };
                }
            }

        }

        public bool DeleteIngredient(Ingredient ingredient)
        {
            if (ingredient == null) return false;

            using (var context = new CookBookContext())
            {
                try
                {
                    context.Entry(ingredient).State = EntityState.Deleted;
                    context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Could not delete ingredient");
                    Console.WriteLine(ex.Message);

                    return false;
                }
            }
        }

        public bool EditIngredient(Ingredient ingredient)
        {
            if (ingredient == null) return false;

            using (var context = new CookBookContext())
            {
                try
                {
                    context.Entry(ingredient).State = EntityState.Modified;
                    context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Could not edit ingredient");
                    Console.WriteLine(ex.Message);

                    return false;
                }
            }
        }


        public bool AddIngredient(Ingredient ingredient)
        {
            if (ingredient == null) { return false; }

            using (var context = new CookBookContext())
            {
                try
                {
                    context.Entry(ingredient).State = EntityState.Added;
                    context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Could not add new item");
                    Console.WriteLine(ex.Message);

                    return false;
                }
            }
        }

        public object ReadIngredient(Ingredient ingredient)
        {
            if (ingredient == null) return false;

            using (var context = new CookBookContext())
            {
                try
                {
                    return context.Ingredients.FirstOrDefault(e => e.Id == ingredient.Id || e.name == ingredient.name);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Could not read ingredient");
                    Console.WriteLine(ex.Message);

                    return null;
                }
            }
        }

        #endregion
    }
}
