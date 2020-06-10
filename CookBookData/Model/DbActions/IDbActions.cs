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
    }
}
