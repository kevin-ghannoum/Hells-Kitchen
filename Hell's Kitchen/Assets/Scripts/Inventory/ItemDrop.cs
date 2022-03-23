using System;
using Enums;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private Item item;
    [SerializeField] private int quantity;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Character character = other.gameObject.GetComponent<Character>();
            character.AddItemToInventory(item, quantity);
        }
    }
}
