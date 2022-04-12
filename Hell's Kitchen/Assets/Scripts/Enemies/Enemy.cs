using System.Security.Cryptography;
using Common.Interfaces;
using Enemies.Enums;
using UnityEngine;
using UnityEngine.Events;

namespace Enemies
{
    
    [RequireComponent(typeof(PathfindingAgent))]
    public class Enemy : MonoBehaviour, IKillable
    {
        [Header("References")]
        [SerializeField]
        protected PathfindingAgent agent;

        [SerializeField]
        protected Animator animator;

        [Header("Parameters")]
        [SerializeField]
        protected float hitPoints;

        [SerializeField]
        private float deathDelay;

        [SerializeField]
        protected UnityEvent onKilled;

        #region Public Getters

        public float HitPoints => hitPoints;
        public UnityEvent Killed => onKilled;

        #endregion

        #region Unity Events

        public virtual void Reset()
        {
            agent = GetComponent<PathfindingAgent>();
            animator = GetComponentInChildren<Animator>();
        }

        public virtual void Update()
        {
            animator.SetFloat(EnemyAnimator.Speed, agent.Velocity.magnitude);
        }

        #endregion

        #region Public Methods

        public virtual void TakeDamage(float damage)
        {
            animator.SetTrigger(EnemyAnimator.OnTakeDamage);
            hitPoints -= damage;
            if (hitPoints <= 0)
            {
                hitPoints = 0;
                Die();
            }
        }

        #endregion

        #region Private Methods

        private void Die()
        {
            Killed?.Invoke();
            animator.SetTrigger(EnemyAnimator.Die);
            Invoke(nameof(Destroy), deathDelay);
        }

        private void Destroy()
        {
            Destroy(gameObject);
        }

        #endregion
        
    }

}
