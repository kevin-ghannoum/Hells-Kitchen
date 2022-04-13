using System;
using Common;
using Common.Enums;
using Input;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Dungeon_Generation
{
    public class Chest: MonoBehaviour
    {
        [SerializeField] private int amountMinInclusive = 20;
        [SerializeField] private int amountMaxExclusive = 40;

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
            var obj = other.gameObject;
            if (obj.CompareTag(Tags.Player))
            {
                // TODO ADD UI
                if (_input.dropItem && !_isLooted)
                {
                    _isLooted = true;
                    _animator.SetTrigger(ObjectAnimator.OpenChest);
                    GameStateManager.Instance.cashMoney += GetRandomAmountInRange();
                }
            }
        }
    }
}