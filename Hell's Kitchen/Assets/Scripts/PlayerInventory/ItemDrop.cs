using Common.Enums;
using Enums.Items;
using Player;
using UI;
using UnityEngine;

namespace PlayerInventory
{
    public class ItemDrop : MonoBehaviour
    {
        [SerializeField]
        private int quantity = 1;

        [SerializeField]
        private ItemInstance item;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Tags.Player))
            {
                PlayerController player = other.gameObject.GetComponent<PlayerController>();
                player.AddItemToInventory(Items.GetItem(item), quantity);
                AdrenalinePointsUI.SpawnIngredientString(transform.position + 2.0f * Vector3.up, "+ " + quantity + " " + item);
                Destroy(gameObject);
            }
        }
    }
}
