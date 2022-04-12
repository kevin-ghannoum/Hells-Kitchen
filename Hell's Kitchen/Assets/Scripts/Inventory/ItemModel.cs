using UnityEditor;
using UnityEngine;

public class ItemModel
{
    public readonly GameObject Prefab;

    private ItemModel(string path)
    {
        Prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
    }

    // Ingredients
    public static readonly ItemModel Honey = new ItemModel("Assets/Prefabs/Ingredients/Honey.prefab");
    public static readonly ItemModel PorkChop = new ItemModel("Assets/Prefabs/Ingredients/PorkChop.prefab");
    public static readonly ItemModel Fish = new ItemModel("Assets/Prefabs/Ingredients/Fish.prefab");
    public static readonly ItemModel Mushroom = new ItemModel("Assets/Prefabs/Ingredients/Mushroom.prefab");
    
    // Recipe Results
    public static readonly ItemModel Burger = new ItemModel("Assets/Prefabs/Ingredients/Hamburger.prefab");
    public static readonly ItemModel Salad = new ItemModel("Assets/Prefabs/Ingredients/Salad.prefab");
    public static readonly ItemModel Sushi = new ItemModel("Assets/Prefabs/Ingredients/Sushi.prefab");
}
