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
    class Deserialiser
    {
        public string FileName { get; set; }
        public ISerializable DeserialisedObject { get; set; }

        public List<Recipe> DeserialisedRecipes { get; set; } = new List<Recipe>();
        public List<Ingredient> DeserialisedIngredients { get; set; } = new List<Ingredient>();
        public List<Measure> DeserialisedMeasures { get; set; } = new List<Measure>();
        public List<RecipeIngredient> DeserialisedRecipeIngredients { get; set; } = new List<RecipeIngredient>();
        public List<RecipeStep> DeserialisedRecipeSteps { get; set; } = new List<RecipeStep>();

        public Deserialiser() { }

        public Deserialiser(string fileName)
        {
            FileName = fileName;
        }


        public bool Deserialise()
        {
            Stream stream;

            try
            {
                stream = File.Open(FileName, FileMode.Open);
                BinaryFormatter binaryFormatter = new BinaryFormatter();

                object allData = binaryFormatter.Deserialize(stream);
                List<object> data = allData as List<object>;

                foreach (var obj in data)
                {
                    if (obj != null)
                    {
                        if (obj is Recipe)
                        {
                            DeserialisedRecipes.Add(obj as Recipe);
                        }

                        if (obj is Ingredient)
                        {
                            DeserialisedIngredients.Add(obj as Ingredient);
                        }

                        if (obj is Measure)
                        {
                            DeserialisedMeasures.Add(obj as Measure);
                        }

                        if (obj is RecipeIngredient)
                        {
                            DeserialisedRecipeIngredients.Add(obj as RecipeIngredient);
                        }

                        if (obj is RecipeStep)
                        {
                            DeserialisedRecipeSteps.Add(obj as RecipeStep);
                        }
                    }
                }

                stream.Close();

                return true;
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Serialisation exception: could not deserialise object");
                Console.WriteLine(e.Message);

                return false;
            }
            catch (IOException e)
            {
                Console.WriteLine("I/O exception: could not open file to deserialise object");
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
                Console.WriteLine("Could not deserialise object");
                Console.WriteLine(e.Message);

                return false;
            }
        }
    }
}
