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
    [Table("recipe_steps")]
    [Serializable()]
    public class RecipeStep: ISerializable
    {
        [Key]
        public int Id { get; set; }
        public int recipeId { get; set; }
        public int stepNumber { get; set; }
        [Required]
        [StringLength(512)]
        public string stepInstructions { get; set; }
        public virtual Recipe recipe { get; set; }



        #region ISerializable
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Id", Id);
            info.AddValue("recipeId", recipeId);
            info.AddValue("stepNumber", stepNumber);
            info.AddValue("stepInstructions", stepInstructions);
        }
        #endregion
    }
}
