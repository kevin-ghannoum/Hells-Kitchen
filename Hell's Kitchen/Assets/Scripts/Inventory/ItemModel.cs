using System;
using UnityEditor;
using UnityEngine;

public class ItemModel
{
    public readonly GameObject Prefab;
    public readonly GameObject UIVariant;

    private ItemModel(string path1, string path2 = null)
    {
        Prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path1);
        // UI variants
        if (!String.IsNullOrEmpty(path2))
        {
            UIVariant = AssetDatabase.LoadAssetAtPath<GameObject>(path2);
        }
        else
        {
            UIVariant = null;
        }
    }

    // Ingredients
    public static readonly ItemModel Honey = new ItemModel("Assets/Prefabs/Ingredients/Honey.prefab", "Assets/Prefabs/Ingredients/Honey UI Variant.prefab");
    public static readonly ItemModel Meat = new ItemModel("Assets/Prefabs/Ingredients/Meat.prefab", "Assets/Prefabs/Ingredients/Meat UI Variant.prefab");
    public static readonly ItemModel Fish = new ItemModel("Assets/Prefabs/Ingredients/Fish.prefab", "Assets/Prefabs/Ingredients/Fish UI Variant.prefab");
    public static readonly ItemModel Mushroom = new ItemModel("Assets/Prefabs/Ingredients/Mushroom.prefab", "Assets/Prefabs/Ingredients/Mushroom UI Variant.prefab");
    
    // Recipe Results
    public static readonly ItemModel Burger = new ItemModel("Assets/Prefabs/Ingredients/Hamburger.prefab", "Assets/Prefabs/Ingredients/Hamburger UI Variant.prefab");
    public static readonly ItemModel Salad = new ItemModel("Assets/Prefabs/Ingredients/Salad.prefab", "Assets/Prefabs/Ingredients/Salad UI Variant.prefab");
    public static readonly ItemModel Sushi = new ItemModel("Assets/Prefabs/Ingredients/Sushi.prefab", "Assets/Prefabs/Ingredients/Sushi UI Variant.prefab");
}
