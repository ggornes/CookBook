using CookBookData.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace CookBook.Serialiser
{
    public class Serialiser
    {
        public string FileName { get; set; }
        public ISerializable ObjectToSerialise { get; set; }

        private List<Recipe> recipes;
        private List<Ingredient> ingredients;
        private List<Measure> measures;
        private List<RecipeIngredient> recipeIngredients;
        private List<RecipeStep> recipeSteps;

        public Serialiser() { }

        public Serialiser(string fileName, ISerializable objectToSerialise)
        {
            ObjectToSerialise = objectToSerialise;
            FileName = fileName;
        }

        public Serialiser(ISerializable objectToSerialise) : this("", objectToSerialise) { }

        internal Serialiser(string fileName, List<Recipe> recipes)
        {
            this.recipes = recipes;
            this.FileName = fileName;
        }

        internal Serialiser(string fileName, List<Ingredient> ingredients)
        {
            this.ingredients = ingredients;
            this.FileName = fileName;
        }

        //internal Serialiser(string fileName, List<Recipe> recipes, List<Ingredient> ingredients, List<RecipeIngredient> recipeIngredients, List<RecipeStep> recipeSteps)
        internal Serialiser(string fileName, List<Recipe> recipes, List<Ingredient> ingredients, List<Measure> measures, List<RecipeIngredient> recipeIngredients, List<RecipeStep> recipeSteps)
        {
            this.FileName = fileName;
            this.recipes = recipes;
            this.ingredients = ingredients;
            this.measures = measures;
            this.recipeIngredients = recipeIngredients;
            this.recipeSteps = recipeSteps;
        }

        /// <summary>
        /// Starts the serialisation process
        /// <para>If no filename is provided, the data will be saved as "serialised.bin" in the bin/Debug directory</para>
        /// </summary>
        /// <returns>Result of the operation</returns>
        public bool Serialise()
        {
            Stream stream;

            try
            {
                if (string.IsNullOrWhiteSpace(FileName))
                {
                    FileName = "serialised.bin";
                }

                stream = File.Open(FileName, FileMode.Create);
                BinaryFormatter b = new BinaryFormatter();

                // Merge all models into a single list of objects
                List<object> merged = recipes.Cast<object>().Concat(ingredients.Cast<object>().ToList()).Concat(measures.Cast<object>().ToList()).Concat(recipeIngredients.Cast<object>().ToList()).Concat(recipeSteps.Cast<object>().ToList()).ToList();

                b.Serialize(stream, merged);

                stream.Close();
                return true;
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Serialisation exception: could not serialise object");
                Console.WriteLine(e.Message);

                return false;
            }
            catch (IOException e)
            {
                Console.WriteLine("I/O exception: could not create file to serialise object");
                Console.WriteLine(e.Message);

                return false;
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine("Unauthorised access exception: insufficient file system permissions");
                Console.WriteLine(e.Message);

                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not serialise object");
                Console.WriteLine(e.Message);

                return false;
            }
        }
    }
}
