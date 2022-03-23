using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    /**
     * Singleton instance of player script.
     */
    public static Character Instance;
    
    private Inventory inventory;

    public Dictionary<Item, int> GetPlayerInventory()
    {
        return inventory.GetInventory();
    }

    public void AddItemToInventory(Item item, int quantity)
    {
        inventory.AddItemToInventory(item, quantity);
    }

    public void RemoveItemFromInventory(Item item, int quantity)
    {
        inventory.RemoveItemFromInventory(item,quantity);
    }

    #region Unity Events
    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);
        } else {
            Instance = this;
        }
    }
    #endregion
}
