using System.Collections;
using System.Collections.Generic;
using Common;
using Common.Enums;
using Input;
using Photon.Pun;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class SousChefSelector : MonoBehaviour
{
    [SerializeField] private SousChefType type;
    
    private void OnTriggerStay(Collider other)
    {
        if (InputManager.Actions.Interact.triggered && other.CompareTag(Tags.Player))
        {
            var pv = other.GetComponent<PhotonView>();
            if (pv != null && pv.IsMine)
            {
                SelectSousChef();
            }
        }
    }

    private void SelectSousChef()
    {
        GameStateManager.SetSousChef(type);
        AdrenalinePointsUI.SpawnIngredientString(gameObject.transform.position, "Recruited Sous Chef");
    }
}
