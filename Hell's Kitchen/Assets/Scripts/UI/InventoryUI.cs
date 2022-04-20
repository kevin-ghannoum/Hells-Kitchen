using System;
using System.Collections.Generic;
using System.Linq;
using PlayerInventory;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private int maxItemsPerPage = 20;
        [SerializeField] private Transform inventoryContainer;
        [SerializeField] private Transform inventoryItemSlot;
        
        private float spacing = 30.0f;
        
        private void Start()
        {
            inventoryItemSlot = inventoryContainer.transform.GetChild(0);
        }

        public void UpdateInventory(Dictionary<Item, int> inventoryList)
        {
            int x = 0, y = 0;

            int smallestLength = Math.Min(maxItemsPerPage, inventoryList.Count);
            
            // clear previous data
            ResetInventory();

            for (int i = 0; i < smallestLength; i++)
            {
                RectTransform itemSlotRectTrans = Instantiate(inventoryItemSlot, inventoryContainer).GetComponent<RectTransform>();
                itemSlotRectTrans.gameObject.SetActive(true);

                // textual information
                itemSlotRectTrans.GetComponentsInChildren<TextMeshProUGUI>()[0].text = inventoryList.ElementAt(i).Key.Name; // name
                itemSlotRectTrans.GetComponentsInChildren<TextMeshProUGUI>()[1].text = inventoryList.ElementAt(i).Value.ToString(); // quantity

                // sprites
                itemSlotRectTrans.gameObject.GetComponentInChildren<Image>().sprite = inventoryList.ElementAt(i).Key.ItemModel.Sprite;
            }
        }

        #region InventoryTabs
        // public void DisplayFullInventory()
        // {
        //     UpdateInventory(_playerInventory);
        // }
        //
        // public void DisplayInventoryIngredients()
        // {
        //     DisplayInventoryByType(ItemType.Ingredient);
        // }
        //
        // public void DisplayInventoryRecipeResults()
        // {
        //     DisplayInventoryByType(ItemType.RecipeResult);
        // }

        // private void DisplayInventoryByType(ItemType itemType)
        // {
        //     Dictionary<Item, int> inventoryItems = new Dictionary<Item, int>();
        //
        //     foreach (var matchingItem in _playerInventory.Where(pair => pair.Key.ItemType.Equals(itemType)))
        //     {
        //         inventoryItems.Add(matchingItem.Key, matchingItem.Value);
        //     }
        //     
        //     print(inventoryItems.Count);
        //     
        //     UpdateInventory(inventoryItems);
        // }
        #endregion InventoryTabs
        
        private void ResetInventory()
        {
            Transform[] inventoryItems = inventoryContainer.GetComponentsInChildren<Transform>();

            for (int i = 1; i < inventoryItems.Length-1; i++)
            {
                Destroy(inventoryItems[i].gameObject);
            }
        }
    }
}
