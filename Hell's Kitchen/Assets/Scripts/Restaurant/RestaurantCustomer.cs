using Restaurant.Enums;
using UnityEngine;

namespace Restaurant
{
    [RequireComponent(typeof(PathfindingAgent))]
    public class RestaurantCustomer : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private PathfindingAgent agent;

        [SerializeField]
        private new Rigidbody rigidbody;
        
        [SerializeField]
        private Animator animator;

        private RestaurantManager.Seat _currentSeat;
        private bool _isSitting;
        
        private void Reset()
        {
            agent = GetComponent<PathfindingAgent>();
            rigidbody = GetComponent<Rigidbody>();
            animator = GetComponentInChildren<Animator>();
        }

        private void Start()
        {
            _currentSeat = RestaurantManager.Instance.FindEmptySeat();
            _currentSeat.taken = true;
        }

        private void Update()
        {
            agent.Target = _currentSeat.transform.position;
            animator.SetFloat(CustomerAnimator.Speed, agent.Velocity.magnitude / agent.MaxVelocity);
            animator.SetBool(CustomerAnimator.Sitting, _isSitting);
            
            if (agent.IsArrived)
            {
                _isSitting = true;
                DisableRigidbody();
                transform.position = _currentSeat.transform.position;
                transform.rotation = _currentSeat.transform.rotation;
            }
        }

        private void DisableRigidbody()
        {
            rigidbody.detectCollisions = false;
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;
        }
        
        private void EnableRigidbody()
        {
            rigidbody.detectCollisions = true;
            rigidbody.useGravity = true;
            rigidbody.isKinematic = false;
        }
    }
}
