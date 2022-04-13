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
    [SerializeField] private GameObject canvas;

    private void Awake()
    {
        canvas.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player) && _input.interact)
        {
            // Open Weapon Shop UI
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            canvas.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            canvas.SetActive(false);
        }
    }
}
