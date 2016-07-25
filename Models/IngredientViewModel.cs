using System.ComponentModel.DataAnnotations;

namespace NgCookingMVC_BackEND.Models
{
    public class IngredientViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        [DataType(DataType.Text)]
        [Display(Name = "Nom de l'ingrédient ")]
        public string Name { get; set; }
        [Required]
        public bool IsAvailable { get; set; }
        [Required]
        [Display(Name = "Catégory ")]
        public int CategoryId { get; set; }
        [Required]
        [Display(Name = "Calories")]
        public int Calories { get; set; }
    }
}