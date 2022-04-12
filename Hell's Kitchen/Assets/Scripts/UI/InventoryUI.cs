using System;
using System.Collections.Generic;
using System.Linq;
using Enums;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private float itemSlotCellSize = 50.0f;
        [SerializeField] private int maxColNum = 4;
        [SerializeField] private int maxItemsPerPage = 20;

        [SerializeField] private Transform inventoryContainer;
        [SerializeField] private Transform inventoryItemSlot;
        
        private Dictionary<Item, int> playerInventory;
        private float spacing = 30.0f;

        private void Start()
        {
            inventoryItemSlot = inventoryContainer.transform.GetChild(0);
            playerInventory = PlayerController.Instance.GetPlayerInventory().GetInventoryItems();
        }

        public void UpdateInventory()
        {
            int x = 0, y = 0;

            int smallestLength = Math.Min(maxItemsPerPage, playerInventory.Count);

            for (int i = 0; i < smallestLength; i++)
            {
                RectTransform itemSlotRectTrans = Instantiate(inventoryItemSlot, inventoryContainer).GetComponent<RectTransform>();
                itemSlotRectTrans.gameObject.SetActive(true);

                // populate inventory slot
                itemSlotRectTrans.anchoredPosition = new Vector2(x * itemSlotCellSize + (x+1)*1.5f*spacing, -y * itemSlotCellSize - (y+1)*1.5f*spacing);

                // textual information
                itemSlotRectTrans.GetComponentsInChildren<Text>()[0].text = playerInventory.ElementAt(i).Key.Name; // name
                itemSlotRectTrans.GetComponentsInChildren<Text>()[1].text = playerInventory.ElementAt(i).Value.ToString(); // quantity

                // 3d prefab
                Transform itemPrefabModel =
                    Instantiate(playerInventory.ElementAt(i).Key.ItemModel.getItemPrefab(), itemSlotRectTrans)
                        .GetComponent<Transform>();
                itemPrefabModel.parent = itemSlotRectTrans;
                ChangePrefabLayer(itemPrefabModel, Layer.UI);
                itemPrefabModel.localScale = new Vector3(20, 20, 20);
                
                // move to the next item
                x++;
                if (x > maxColNum)
                {
                    x = 0;
                    y++;
                }
            }
        }

        private void ChangePrefabLayer(Transform modelTransform, Layer UIlayer)
        {
            modelTransform.gameObject.layer = (int)UIlayer;
            foreach (var child in modelTransform.GetComponentsInChildren<Transform>())
            {
                child.gameObject.layer = (int)UIlayer;
            }
        }

        private void ResetInventory()
        {
            Transform[] inventoryItems = inventoryContainer.GetComponentsInChildren<Transform>();

            for (int i = 0; i < inventoryItems.Length; i++)
            {
                Destroy(inventoryItems[i].gameObject);
            }
        }
    }
}
