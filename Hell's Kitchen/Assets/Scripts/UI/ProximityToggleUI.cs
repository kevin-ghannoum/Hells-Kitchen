using System;
using Common.Enums;
using UnityEngine;

namespace UI
{
    public class ProximityToggleUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject toggleGameObject;
        
        private Canvas _canvas;
        private bool _isDisabled = false;
        
        public bool IsDisabled {
            get => _isDisabled;
            set {
                _isDisabled = value;
                if (_isDisabled)
                    Disable();
            }
        }

        private void Start()
        {
            _canvas = toggleGameObject.GetComponentInChildren<Canvas>();
            Disable();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_isDisabled && other.gameObject.CompareTag(Tags.Player))
            {
                Enable();
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (!_isDisabled && other.gameObject.CompareTag(Tags.Player))
            {
                Disable();
            }
        }

        public void Enable()
        {
            _canvas.gameObject.SetActive(true);
        }

        public void Disable()
        {
            _canvas.gameObject.SetActive(false);
        }
    }
}
