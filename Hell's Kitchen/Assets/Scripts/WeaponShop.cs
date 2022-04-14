using System;
using System.Collections;
using System.Collections.Generic;
using Common.Enums;
using Input;
using TMPro;
using UnityEngine;

public class WeaponShop : MonoBehaviour
{
    private InputManager _input => InputManager.Instance;
    [SerializeField] private GameObject interactText;
    [SerializeField] private GameObject shopUI;

    private void Awake()
    {
        interactText.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player) && _input.interact)
        {
            shopUI.SetActive(true);
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
