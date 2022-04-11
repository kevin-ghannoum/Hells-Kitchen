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
<<<<<<< HEAD
=======
        [SerializeField] private float _hitPoints = 100;
        [SerializeField] private AudioClip takeDamageSound;
        [SerializeField] private AudioClip lowHealthSound;
        [SerializeField] private AudioClip deathSound;
        private float invulnerabilityTime = 1;
        private float invulnerabilityTimer = 1;
>>>>>>> d80bcad (Added the pig and added to the dungeon. Fixed animation and added audio)
        private UnityEvent _killed;
        public UnityEvent Killed => _killed ??= new UnityEvent();

        public float HitPoints
        {
            get => GameStateManager.Instance.playerCurrentHitPoints;
            set => GameStateManager.Instance.playerCurrentHitPoints = value;
        }
<<<<<<< HEAD
        
=======

        void Update()
        {
            if (invulnerabilityTimer < invulnerabilityTime) invulnerabilityTimer += Time.deltaTime;
        }

>>>>>>> d80bcad (Added the pig and added to the dungeon. Fixed animation and added audio)
        public void TakeDamage(float damage)
        {
            if (invulnerabilityTimer >= invulnerabilityTime)
            {
                animator.SetTrigger(PlayerAnimator.TakeHit);
                HitPoints -= damage;
                invulnerabilityTimer = 0;

                // If the player's hp is at 0 or lower, they die
                if (HitPoints <= 0)
                {
                    Die();
                    return;
                }

                if (HitPoints > 10)
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
