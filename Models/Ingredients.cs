using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NgCookingMVC_BackEND.Models
{
    public class Ingredients
    {
      
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public bool IsAvailable { get; set; }
        [Required(ErrorMessage = "Picture URL is Required")]
        [DataType(DataType.Upload)]
        public byte[] Picture { get; set; }    
        public int Calories { get; set; }
        [ForeignKey("CategoryId")]
        public Categories Category { get; set; }
        public int CategoryId { get; set; }
        public virtual ICollection<RecettesIngredients> RecettesIngredients { get; set; }
        public Ingredients()
        {
            RecettesIngredients = new HashSet<RecettesIngredients>();
        }
    }
}