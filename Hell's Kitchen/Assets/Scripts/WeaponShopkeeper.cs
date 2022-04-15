using System;
using System.Collections;
using System.Collections.Generic;
using Common.Enums;
using Input;
using TMPro;
using UI;
using UnityEngine;

public class WeaponShopkeeper : MonoBehaviour
{
    private InputManager _input => InputManager.Instance;
    [SerializeField] private GameObject interactText;
    [SerializeField] private WeaponShopUI shopUI;

    private void Awake()
    {
        interactText.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player) && _input.interact)
        {
            shopUI.Initialize();
            interactText.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            interactText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            interactText.SetActive(false);
        }
    }
}
