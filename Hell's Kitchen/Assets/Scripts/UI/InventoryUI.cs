using System;
using System.Collections.Generic;
using System.Linq;
using Common.Enums.Items;
using Enums.Items;
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

        private void Start()
        {
            inventoryItemSlot = inventoryContainer.transform.GetChild(0);
        }

        public void UpdateInventory(Dictionary<ItemInstance, int> inventoryList)
        {
            int smallestLength = Math.Min(maxItemsPerPage, inventoryList.Count);
            
            // clear previous data
            ResetInventory();

            for (int i = 0; i < smallestLength; i++)
            {
                RectTransform itemSlotRectTrans = Instantiate(inventoryItemSlot, inventoryContainer).GetComponent<RectTransform>();
                itemSlotRectTrans.gameObject.SetActive(true);

                var item = Items.GetItem(inventoryList.ElementAt(i).Key);
                
                // textual information
                itemSlotRectTrans.GetComponentsInChildren<TextMeshProUGUI>()[0].text = item.Name; // name
                itemSlotRectTrans.GetComponentsInChildren<TextMeshProUGUI>()[1].text = inventoryList.ElementAt(i).Value.ToString(); // quantity

                // sprites
                itemSlotRectTrans.gameObject.GetComponentInChildren<Image>().sprite = item.ItemModel.Sprite;
            }
        }

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
