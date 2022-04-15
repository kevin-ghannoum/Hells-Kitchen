using UnityEditor;
using UnityEngine;

namespace PlayerInventory
{
    public class ItemModel
    {
        public GameObject itemPrefab;

        public ItemModel(string path)
        {
            itemPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        }
    }
}