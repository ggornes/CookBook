using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookBookData.Model
{
    public class Measure
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
    }
}
