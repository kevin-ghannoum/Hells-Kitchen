using UnityEngine;
using UnityEngine.Events;

namespace Common
{
    public class ForwardTrigger : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent<Collider> onTriggerEnter = new UnityEvent<Collider>();
        
        [SerializeField]
        private UnityEvent<Collider> onTriggerExit = new UnityEvent<Collider>();
        
        [SerializeField]
        private UnityEvent<Collider> onTriggerStay = new UnityEvent<Collider>();
        
        private void OnTriggerEnter(Collider other)
        {
            onTriggerEnter?.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            onTriggerExit?.Invoke(other);
        }

        private void OnTriggerStay(Collider other)
        {
            onTriggerStay?.Invoke(other);
        }
    }
}
