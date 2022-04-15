using System;
using Common.Enums;
using UnityEngine;

namespace UI
{
    public class ToggleUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject toggleGameObject;

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
            toggleGameObject.SetActive(true);
        }

        public void Disable()
        {
            toggleGameObject.SetActive(false);
        }
    }
}
