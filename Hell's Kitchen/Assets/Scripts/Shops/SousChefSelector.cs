using Common;
using Common.Enums;
using Dungeon_Generation;
using UI;
using UnityEngine;

namespace Shops
{
    public class SousChefSelector : Interactable
    {
        [SerializeField] private SousChefType type;
    
        protected override void Interact()
        {
            GameStateManager.SetSousChef(type);
            AdrenalinePointsUI.SpawnIngredientString(gameObject.transform.position, "Recruited Sous Chef");
        }
    }
}
