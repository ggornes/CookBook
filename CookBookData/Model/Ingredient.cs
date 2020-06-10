using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookBookData.Model
{
    public class Ingredient
    {
        public Ingredient()
        {
            this.recipeIngredients = new HashSet<RecipeIngredient>();
        }


        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "VARCHAR")]
        [Required]
        [StringLength(32)]
        public string name { get; set; }
        public virtual ICollection<RecipeIngredient> recipeIngredients { get; set; }
    }
}
