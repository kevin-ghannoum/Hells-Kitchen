using System.Collections.Generic;
using PlayerInventory;

public interface IRecipe
{
    public List<(Item item, int quantity)> GetIngredientList();
    public Item GetRecipeResult();

    public float GetCost();
}
