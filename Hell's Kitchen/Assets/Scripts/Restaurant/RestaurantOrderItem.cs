using Enums.Items;
using UnityEngine;
using UnityEngine.UI;

namespace Restaurant
{
    public class RestaurantOrderItem : MonoBehaviour
    {
        [Header("Item Properties")]
        [SerializeField]
        public int Quantity;
        
        [SerializeField]
        public ItemInstance Item;
        
        [SerializeField]
        public bool Served;

        [Header("General")]
        private float servedOpacity = 0.5f;
        
        [Header("References")]
        [SerializeField]
        private Image itemImage;

        [SerializeField]
        private Text quantityText;

        [SerializeField]
        private Image checkmarkImage;

        private void Reset()
        {
            itemImage = GetComponentInChildren<Image>();
            quantityText = GetComponentInChildren<Text>();
            RefreshUI();
        }

        private void OnValidate()
        {
            RefreshUI();
        }

        private void Start()
        {
            RefreshUI();
        }

        public void RefreshUI()
        {
            var item = Items.GetItem(Item);
            itemImage.sprite = item.ItemModel.Sprite;
            quantityText.text = $"{Quantity}";
            if (Served)
            {
                EnableServed();
            }
            else
            {
                DisableServed();
            }
        }

        private void EnableServed()
        {
            var imageColor = itemImage.color;
            imageColor.a = servedOpacity;
            itemImage.color = imageColor;
            checkmarkImage.enabled = true;
        }

        private void DisableServed()
        {
            var imageColor = itemImage.color;
            imageColor.a = 1.0f;
            itemImage.color = imageColor;
            checkmarkImage.enabled = false;
        }

    }
}
