using UnityEditor;
using UnityEngine;

public class ItemModel
{
    public GameObject itemPrefab;

    public ItemModel(string path)
    {
        itemPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
    }
}