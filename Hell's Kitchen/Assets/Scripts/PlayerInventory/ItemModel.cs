using System;
using UnityEditor;
using UnityEngine;

public class ItemModel
{
    public readonly GameObject Prefab;
    public readonly Sprite Sprite;

    private ItemModel(string path1, string path2 = null)
    {
        Prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path1);

        if (!String.IsNullOrEmpty(path2))
        {
            Sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path2);
        }
        else
        {
            Sprite = null;
        }
    }

    // Ingredients
    public static readonly ItemModel Honey = new ItemModel("Assets/Prefabs/Ingredients/Honey.prefab", "Assets/Sprites/ingredients/freepik_honey_1378430.png");
    public static readonly ItemModel Meat = new ItemModel("Assets/Prefabs/Ingredients/Meat.prefab", "Assets/Sprites/ingredients/freepik_meat_2058020.png");
    public static readonly ItemModel Fish = new ItemModel("Assets/Prefabs/Ingredients/Fish.prefab", "Assets/Sprites/ingredients/freepik_fish_394730.png");
    public static readonly ItemModel Mushroom = new ItemModel("Assets/Prefabs/Ingredients/Mushroom.prefab", "Assets/Sprites/ingredients/freepik_mushroom_1412518.png");
    
    // Recipe Results
    public static readonly ItemModel Burger = new ItemModel("Assets/Prefabs/Ingredients/Hamburger.prefab", "Assets/Sprites/ingredients/freepik_hamurger_3075977.png");
    public static readonly ItemModel Salad = new ItemModel("Assets/Prefabs/Ingredients/Salad.prefab", "Assets/Sprites/ingredients/freepik_salad_1057510.png");
    public static readonly ItemModel Sushi = new ItemModel("Assets/Prefabs/Ingredients/Sushi.prefab", "Assets/Sprites/ingredients/freepik_sushi_3978725.png");
}