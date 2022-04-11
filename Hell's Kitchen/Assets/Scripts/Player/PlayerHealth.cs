using Common.Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    public class PlayerHealth : MonoBehaviour, IKillable
    {
        public const float MaxHitPoints = 100f;
        [SerializeField] private Animator animator;
        [SerializeField] private float _hitPoints = 100;
        private UnityEvent _killed;
        public UnityEvent Killed => _killed ??= new UnityEvent();

        public float HitPoints
        {
            get => _hitPoints;
            set => _hitPoints = value;
        }
    
        public void TakeDamage(float damage)
        {
            animator.SetTrigger(PlayerAnimator.TakeHit);
            HitPoints -= damage;

            // If the player's hp is at 0 or lower, they die
            if (HitPoints <= 0)
                Die();
        }

        private void Die()
        {
            Killed.Invoke();
        }
    }
}
