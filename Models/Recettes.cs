using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NgCookingMVC_BackEND.Models
{
    public class Recettes
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        
        public string NameToDisplay { get; set; }
        
        public string CreatorId { get; set; }

        [ForeignKey("CreatorId")]
        public NgCookingUser Creator { get; set; }

        public bool IsAvailable { get; set; }

        [Required(ErrorMessage = "Picture URL is Required")]
        [DataType(DataType.Upload)]
        public byte[] Picture { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Preparation { get; set; }

        public ICollection<Comments> Comments { get; set; }

        public virtual ICollection<RecettesIngredients> RecettesIngredients { get; set; }
        public Recettes()
        {
            RecettesIngredients = new HashSet<RecettesIngredients>();
            Comments = new HashSet<Comments>();
            Picture = new byte[] { };
        }
    }
}