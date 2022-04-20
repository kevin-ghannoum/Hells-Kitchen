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
    InputManager _input =>InputManager.Instance;
    [SerializeField] private SousChefType type;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.Player) && other.GetComponent<PhotonView>().IsMine)
        {
            _input.reference.actions["Interact"].performed +=SelectSousChef;
        }
    }
        
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Tags.Player) && other.GetComponent<PhotonView>().IsMine)
        {
            _input.reference.actions["Interact"].performed -= SelectSousChef;
        }
    }

    private void SelectSousChef(InputAction.CallbackContext context)
    {
        GameStateManager.SetSousChef(type);
        AdrenalinePointsUI.SpawnIngredientString(gameObject.transform.position, "Recruited Sous Chef");
    }
}
