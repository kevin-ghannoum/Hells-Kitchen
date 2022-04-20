using Common.Enums;
using Input;
using Photon.Pun;
using UnityEngine;

namespace Dungeon_Generation
{
    public abstract class Interactable : MonoBehaviour
    {
        private bool _interactionEnabled;

        public virtual void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(Tags.Player))
            {
                PhotonView pv = other.GetComponent<PhotonView>();
                if (pv != null && pv.IsMine)
                {
                    _interactionEnabled = true;
                }
            }
        }

        public virtual void OnTriggerExit(Collider other) 
        {
            if (other.CompareTag(Tags.Player))
            {
                PhotonView pv = other.GetComponent<PhotonView>();
                if (pv != null && pv.IsMine)
                {
                    _interactionEnabled = false;
                }
            }
        }

        public virtual void Update()
        {
            if (_interactionEnabled && InputManager.Actions.Interact.triggered)
            {
                Interact();
            }
        }

        protected abstract void Interact();
    }
}
