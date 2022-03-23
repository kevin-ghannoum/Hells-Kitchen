using System.Collections.Generic;

public interface IRecipe
{
    public List<(Item item, int quantity)> GetIngredientList();
    public Item GetRecipeResult();
}
