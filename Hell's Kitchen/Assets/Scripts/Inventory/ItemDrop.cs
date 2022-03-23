using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private Item item;
    [SerializeField] private int quantity;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player character = other.gameObject.GetComponent<Player>();
            character.AddItemToInventory(item, quantity);
        }
    }
}
