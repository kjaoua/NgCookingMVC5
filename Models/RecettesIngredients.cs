namespace NgCookingMVC_BackEND.Models
{
    public class RecettesIngredients
    {
        public int IngredientId { get; set; }
        public int RecetteId { get; set; }
        public virtual Ingredients Ingredient { get; set; }
        public virtual Recettes Recette { get; set; }
    }
}