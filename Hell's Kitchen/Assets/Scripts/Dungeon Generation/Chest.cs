using Common;
using Common.Enums;
using Input;
using UI;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

namespace Dungeon_Generation
{
    public class Chest: MonoBehaviour
    {
        [SerializeField] private int amountMinInclusive = 20;
        [SerializeField] private int amountMaxExclusive = 40;
        [SerializeField] private ToggleUI toggleUI;

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
                    toggleUI.IsDisabled = true;
                    var amount = GetRandomAmountInRange();
                    GameStateManager.SetCashMoney(GameStateData.cashMoney + amount);
                    AdrenalinePointsUI.SpawnGoldNumbers(transform.position + 2.0f * Vector3.up, amount);
                }
            }
        }
    }
}
