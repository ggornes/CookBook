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
