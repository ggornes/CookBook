using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookBookData.Model
{
    [Table("recipe_steps")]
    public class RecipeStep
    {
        [Key]
        public int Id { get; set; }
        public int recipeId { get; set; }
        public int stepNumber { get; set; }
        [Required]
        [StringLength(512)]
        public string stepInstructions { get; set; }
        public virtual Recipe recipe { get; set; }
    }
}
