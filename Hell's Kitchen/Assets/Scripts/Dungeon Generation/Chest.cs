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
        [SerializeField] private  GameObject canvas;
        [SerializeField] private  RectTransform textTransform;

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
                    GameStateManager.Instance.cashMoney += GetRandomAmountInRange();
                }
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
        
        // private void LateUpdate()
        // {
        //     if (!Camera.main)
        //         return;
        //
        //     var canvasRect = canvas.GetComponent<RectTransform>();
        //     
        //     // Offset position above object bbox (in world space)
        //     float offsetPosY = transform.position.y + 2.5f;
        //     
        //     // Final position of marker above GO in world space
        //     Vector3 offsetPos = new Vector3(transform.position.x, offsetPosY, transform.position.z);
        //
        //     
        //     // Calculate *screen* position (note, not a canvas/recttransform position)
        //     Vector2 canvasPos;
        //     Vector2 screenPoint = Camera.main.WorldToScreenPoint(offsetPos);
        //
        //     // Convert screen position to Canvas / RectTransform space <- leave camera null if Screen Space Overlay
        //     RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, null, out canvasPos);
        //
        //     // Set
        //     textTransform.localPosition = canvasPos;
        //     textTransform.forward = canvas.transform.forward;
        //
        // }
    }
}