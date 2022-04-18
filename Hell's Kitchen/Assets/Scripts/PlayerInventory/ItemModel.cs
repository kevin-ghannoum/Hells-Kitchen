using UnityEditor;
using UnityEngine;

public class ItemModel
{
    public readonly GameObject Prefab;
    public readonly GameObject UIVariant;
    public readonly Sprite Sprite;

    private ItemModel(string prefabPath, string uiVariantPath = null, string spritePath = null)
    {
        Prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        UIVariant = !string.IsNullOrEmpty(uiVariantPath) ? AssetDatabase.LoadAssetAtPath<GameObject>(uiVariantPath) : null;
        Sprite = !string.IsNullOrEmpty(spritePath) ? AssetDatabase.LoadAssetAtPath<Sprite>(spritePath) : null;
    }

    // Ingredients
    public static readonly ItemModel Honey = new ItemModel(
        "Assets/Prefabs/Ingredients/Honey.prefab", 
        "Assets/Prefabs/Ingredients/Honey UI Variant.prefab"
    );
    public static readonly ItemModel Meat = new ItemModel(
        "Assets/Prefabs/Ingredients/Meat.prefab", 
        "Assets/Prefabs/Ingredients/Meat UI Variant.prefab"
    );
    public static readonly ItemModel Fish = new ItemModel(
        "Assets/Prefabs/Ingredients/Fish.prefab", 
        "Assets/Prefabs/Ingredients/Fish UI Variant.prefab"
    );
    public static readonly ItemModel Mushroom = new ItemModel(
        "Assets/Prefabs/Ingredients/Mushroom.prefab", 
        "Assets/Prefabs/Ingredients/Mushroom UI Variant.prefab"
    );
    
    // Recipe Results
    public static readonly ItemModel Burger = new ItemModel(
        "Assets/Prefabs/Ingredients/Hamburger.prefab", 
        "Assets/Prefabs/Ingredients/Hamburger UI Variant.prefab", 
        "Assets/Sprites/Items/Hamburger.png"
    );
    public static readonly ItemModel Salad = new ItemModel(
        "Assets/Prefabs/Ingredients/Salad.prefab", 
        "Assets/Prefabs/Ingredients/Salad UI Variant.prefab",
        "Assets/Sprites/Items/Salad.png"
    );
    public static readonly ItemModel Sushi = new ItemModel(
        "Assets/Prefabs/Ingredients/Sushi.prefab", 
        "Assets/Prefabs/Ingredients/Sushi UI Variant.prefab",
        "Assets/Sprites/Items/Sushi.png"
    );
}
