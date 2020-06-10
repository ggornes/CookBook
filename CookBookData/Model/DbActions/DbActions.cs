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
        #endregion
    }
}
