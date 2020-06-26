using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CookBookData.Model
{
    [Table("recipe_ingredients")]
    [Serializable()]
    public class RecipeIngredient: ISerializable
    {
        [Key]
        public int Id { get; set; }
        public int recipeId { get; set; }
        public int ingredientId { get; set; }
        public int? measureId { get; set; }
        public int amount { get; set; }
        public virtual Ingredient ingredient { get; set; }
        public virtual Measure measure { get; set; }
        public virtual Recipe recipe { get; set; }


        #region ISerializable
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Id", Id);
            info.AddValue("recipeId", recipe);
            info.AddValue("ingredientId", ingredientId);
            info.AddValue("measureId", measureId);
            info.AddValue("amount", amount);
        }
        #endregion
    }
}
