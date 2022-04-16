using Common;
using Common.Interfaces;
using UI;
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
        
        private float _invulnerabilityTime = 1;
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
            if (_invulnerabilityTimer < _invulnerabilityTime)
            {
                _invulnerabilityTimer += Time.deltaTime;
            }
        }

        public void TakeDamage(float damage)
        {
            // Invulnerability while rolling
            var animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (animatorStateInfo.IsName(PlayerAnimator.Roll))
                return;
            
            // Invulnerability after getting hit
            if (_invulnerabilityTimer < _invulnerabilityTime)
                return;
            
            // HP calculation and animation
            animator.SetTrigger(PlayerAnimator.TakeHit);
            HitPoints -= damage;
            _invulnerabilityTimer = 0;
            
            // Damage numbers
            AdrenalinePointsUI.SpawnDamageNumbers(transform.position + 2.0f * Vector3.up, -damage);

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

        private void Die()
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position);
            Killed.Invoke();
            animator.SetTrigger(PlayerAnimator.Dead);
        }


    }
}
