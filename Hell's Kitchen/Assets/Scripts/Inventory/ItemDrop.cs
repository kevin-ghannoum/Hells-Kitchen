using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Common.Enums;
using Enums.Items;
using Player;
using UnityEngine;

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
            Destroy(gameObject);
        }
    }
}
