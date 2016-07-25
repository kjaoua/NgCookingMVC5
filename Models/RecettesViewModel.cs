using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NgCookingMVC_BackEND.Models
{
    public class RecettesViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "recette name is Required")]
        [StringLength(255)]
        [DataType(DataType.Text)]
        [Display(Name = "Nom de la recette ")]
        public string Name { get; set; }

        [Required]
        [StringLength(255)]
        [DataType(DataType.Text)]
        [Display(Name = "Nom de la recette ")]
        public string NameToDisplay { get; set; }

        [Required]
        public bool IsAvailable { get; set; }

        [Required(ErrorMessage = "Preparation is Required")]
        [Display(Name = "Préparation de la recette ")]
        [DataType(DataType.MultilineText)]
        public string Preparation { get; set; }

        public List<Ingredients> Ingredientts { get; set; }
    }
}