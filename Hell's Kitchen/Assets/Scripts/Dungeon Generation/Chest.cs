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
        [SerializeField] private GameObject canvas;

        private bool _isLooted = false;
        
        private InputManager _input => InputManager.Instance;
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            canvas.SetActive(false);
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
                    canvas.SetActive(false);
                    GameStateManager.Instance.cashMoney += GetRandomAmountInRange();
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(Tags.Player) && !_isLooted)
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
}
