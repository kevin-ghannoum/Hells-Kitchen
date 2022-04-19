using UnityEngine;

public class ItemModel
{
    public readonly GameObject Prefab;
    public readonly Sprite Sprite;

    private ItemModel(string prefabPath, string spritePath = null)
    {
        Prefab = Resources.Load<GameObject>(prefabPath);
        Sprite = !string.IsNullOrEmpty(spritePath) ? Resources.Load<Sprite>(spritePath) : null;
    }

    // Ingredients
    public static readonly ItemModel Honey = new ItemModel(
        "Honey",
        "Honey"
    );
    public static readonly ItemModel Meat = new ItemModel(
        "Meat",
        "Meat"
    );
    public static readonly ItemModel Fish = new ItemModel(
        "Fish",
        "Fish"
    );
    public static readonly ItemModel Mushroom = new ItemModel(
        "Mushroom",
        "Mushroom"
    );
    
    // Recipe Results
    public static readonly ItemModel Burger = new ItemModel(
        "Hamburger",
        "Hamburger"
    );
    public static readonly ItemModel Salad = new ItemModel(
        "Salad",
        "Salad"
    );
    public static readonly ItemModel Sushi = new ItemModel(
        "Sushi",
        "Sushi"
    );
}
