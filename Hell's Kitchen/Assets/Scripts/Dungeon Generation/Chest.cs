using System;
using Common;
using Common.Enums;
using Input;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Dungeon_Generation
{
    public class Chest: MonoBehaviour
    {
        [SerializeField] private int amountMinInclusive = 20;
        [SerializeField] private int amountMaxExclusive = 40;
        [SerializeField] private  GameObject interactText;

        private bool _isLooted = false;
        
        private InputManager _input => InputManager.Instance;
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        private int GetRandomAmountInRange()
        {
            return Random.Range(amountMinInclusive, amountMaxExclusive);
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag(Tags.Player))
            {
                if (_input.interact && !_isLooted)
                {
                    _isLooted = true;
                    _animator.SetTrigger(ObjectAnimator.OpenChest);
                    GameStateManager.Instance.cashMoney += GetRandomAmountInRange();
                }
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
        
        private void LateUpdate()
        {
            if (!Camera.main)
                return;
            
            interactText.transform.rotation = Camera.main.transform.rotation;
            interactText.transform.forward = -interactText.transform.forward;
            
        }
    }
}