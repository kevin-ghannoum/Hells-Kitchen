using System;
using System.Collections.Generic;
using System.Linq;
using Enums;
using Input;
using Player;
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
        
        private Dictionary<Item, int> _playerInventory;
        private InputManager _input => InputManager.Instance;
        private float spacing = 30.0f;
        private bool isUIActive = false;

        private void Awake()
        {
            _input.reference.actions["OpenInventory"].performed += OpenInventory;
        }

        private void Start()
        {
            inventoryItemSlot = inventoryContainer.transform.GetChild(0);
            _playerInventory = PlayerController.Instance.GetPlayerInventory().GetInventoryItems();
        }

        public void UpdateInventory()
        // TODO: MOVE THIS LATER
        void OpenInventory(InputAction.CallbackContext callbackContext)
        {
            isUIActive = !isUIActive;
            inventory.SetActive(isUIActive);
        }

        {
            int x = 0, y = 0;

            int smallestLength = Math.Min(maxItemsPerPage, _playerInventory.Count);
            
            // avoid duplicate gameObjects
            ResetInventory();

            for (int i = 0; i < smallestLength; i++)
            {
                RectTransform itemSlotRectTrans = Instantiate(inventoryItemSlot, inventoryContainer).GetComponent<RectTransform>();
                itemSlotRectTrans.gameObject.SetActive(true);

                // populate inventory slot
                itemSlotRectTrans.anchoredPosition = new Vector2(x * itemSlotCellSize + (x+1)*1.5f*spacing, -y * itemSlotCellSize - (y+1)*1.5f*spacing);

                // textual information
                itemSlotRectTrans.GetComponentsInChildren<Text>()[0].text = _playerInventory.ElementAt(i).Key.Name; // name
                itemSlotRectTrans.GetComponentsInChildren<Text>()[1].text = _playerInventory.ElementAt(i).Value.ToString(); // quantity

                // 3d prefab
                Transform itemPrefabModel =
                    Instantiate(_playerInventory.ElementAt(i).Key.ItemModel.UIVariant, itemSlotRectTrans)
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

        private void ChangePrefabLayer(Transform modelTransform, Layer uiLayer)
        {   
            modelTransform.gameObject.layer = (int)uiLayer;
            foreach (var child in modelTransform.GetComponentsInChildren<Transform>())
            {
                child.gameObject.layer = (int)uiLayer;
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
