using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookBookData.Model
{
    public class Recipe
    {
        public Recipe()
        {
            RecipeIngredients = new HashSet<RecipeIngredient>();
            recipeSteps = new HashSet<RecipeStep>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(32)]
        public string name { get; set; }
        public int prepTime { get; set; }
        public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; }
        public virtual ICollection<RecipeStep> recipeSteps { get; set; }
    }

    
}
