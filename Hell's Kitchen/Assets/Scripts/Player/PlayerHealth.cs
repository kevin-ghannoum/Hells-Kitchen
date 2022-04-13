using Common;
using Common.Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    public class PlayerHealth : MonoBehaviour, IKillable
    {
        [SerializeField] private Animator animator;
        [SerializeField] private AudioClip takeDamageSound;
        [SerializeField] private AudioClip lowHealthSound;
        [SerializeField] private AudioClip deathSound;
        private float invulnerabilityTime = 1;
        private float _invulnerabilityTimer = 1;
        private UnityEvent _killed;
        public UnityEvent Killed => _killed ??= new UnityEvent();

        public float HitPoints
        {
            get => GameStateManager.Instance.playerCurrentHitPoints;
            set => GameStateManager.Instance.playerCurrentHitPoints = value;
        }

        void Update()
        {
            if (_invulnerabilityTimer < invulnerabilityTime) 
                _invulnerabilityTimer += Time.deltaTime;
        }

        public void TakeDamage(float damage)
        {
            bool isRolling = animator.GetCurrentAnimatorStateInfo(0).IsName(PlayerAnimator.Roll);
            if (_invulnerabilityTimer >= invulnerabilityTime && !isRolling)
            {
                animator.SetTrigger(PlayerAnimator.TakeHit);
                HitPoints -= damage;
                _invulnerabilityTimer = 0;

                // If the player's hp is at 0 or lower, they die
                if (HitPoints <= 0)
                {
                    Die();
                    return;
                }

                if (HitPoints > 25)
                {
                    AudioSource.PlayClipAtPoint(takeDamageSound, transform.position);
                }
                else
                {
                    AudioSource.PlayClipAtPoint(lowHealthSound, transform.position);
                }
            }
        }

        private void Die()
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position);
            Killed.Invoke();
            animator.SetTrigger(PlayerAnimator.Dead);
        }
    }
}
