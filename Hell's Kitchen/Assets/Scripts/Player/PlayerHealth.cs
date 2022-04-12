using System;
using Common;
using Common.Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    public class PlayerHealth : MonoBehaviour, IKillable
    {
        [SerializeField] private Animator animator;
        private UnityEvent _killed;
        public UnityEvent Killed => _killed ??= new UnityEvent();

        public float HitPoints
        {
            get => GameStateManager.Instance.playerCurrentHitPoints;
            set => GameStateManager.Instance.playerCurrentHitPoints = value;
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
