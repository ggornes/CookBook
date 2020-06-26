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
    [Serializable()]
    public class Measure: ISerializable
    {
        public Measure()
        {
            recipeIngredients = new HashSet<RecipeIngredient>();
        }

        [Key]
        public int Id { get; set; }
        [Column(TypeName = "VARCHAR")]
        [Required]
        [StringLength(32)]
        public string name { get; set; }
        public virtual ICollection<RecipeIngredient> recipeIngredients { get; set; }


        protected Measure(SerializationInfo info, StreamingContext context)
        {
            Id = (int)info.GetValue("Id", typeof(int));
            name = (string)info.GetValue("name", typeof(string));
        }


        #region ISerializable
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Id", Id);
            info.AddValue("name", name);
        }
        #endregion
    }
}
