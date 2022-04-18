using System;
using System.Collections.Generic;
using System.Linq;
using Input;
using PlayerInventory;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private float itemSlotCellSize = 50.0f;
        [SerializeField] private int maxColNum = 4;
        [SerializeField] private int maxItemsPerPage = 20;

        [SerializeField] private GameObject inventory;
        [SerializeField] private Transform inventoryContainer;
        [SerializeField] private Transform inventoryItemSlot;
        
        private InputManager _input => InputManager.Instance;
        private float spacing = 30.0f;
        private bool isUIActive = false;

        private void Awake()
        {
            _input.reference.actions["OpenInventory"].performed += OpenInventory;
        }

        private void OnDestroy()
        {
            _input.reference.actions["OpenInventory"].performed -= OpenInventory;
        }

        private void Start()
        {
            inventoryItemSlot = inventoryContainer.transform.GetChild(0);
        }

        // TODO: MOVE THIS LATER
        void OpenInventory(InputAction.CallbackContext callbackContext)
        {
            isUIActive = !isUIActive;
            inventory.SetActive(isUIActive);
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

                // populate inventory slot
                itemSlotRectTrans.anchoredPosition = new Vector2(x * itemSlotCellSize + (x+1)*1.5f*spacing, -y * itemSlotCellSize - (y+1)*1.5f*spacing);

                // textual information
                itemSlotRectTrans.GetComponentsInChildren<Text>()[0].text = inventoryList.ElementAt(i).Key.Name; // name
                itemSlotRectTrans.GetComponentsInChildren<Text>()[1].text = inventoryList.ElementAt(i).Value.ToString(); // quantity

                // 3d prefab
                Transform itemPrefabModel =
                    Instantiate(inventoryList.ElementAt(i).Key.ItemModel.UIVariant, itemSlotRectTrans)
                        .GetComponent<Transform>();
                itemPrefabModel.parent = itemSlotRectTrans;
                
                // move to the next item
                x++;
                if (x > maxColNum)
                {
                    x = 0;
                    y++;
                }
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
