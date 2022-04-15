using System.Security.Cryptography;
using Common.Interfaces;
using Enemies.Enums;
using UI;
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

        [SerializeField]
        private GameObject damagePrefab;

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
            var animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
            agent.enabled = !animatorStateInfo.IsName(EnemyAnimator.TakeHit);
            animator.SetFloat(EnemyAnimator.Speed, agent.Velocity.magnitude);
        }

        #endregion

        #region Public Methods

        public virtual void TakeDamage(float damage)
        {
            // HP calculation and animation
            hitPoints -= damage;
            animator.SetTrigger(EnemyAnimator.TakeHit);
            
            // Damage numbers
            AdrenalinePointsUI.SpawnDamageNumbers(transform.position + 2.0f * Vector3.up, -damage);
            
            // Death
            if (hitPoints <= 0)
            {
                hitPoints = 0;
                Die();
            }
        }

        #endregion

        #region Private Methods

        protected virtual void Die()
        {
            Killed?.Invoke();
            animator.SetTrigger(EnemyAnimator.Die);
            Invoke(nameof(Destroy), deathDelay);
            agent.enabled = false;
            enabled = false;
        }

        private void Destroy()
        {
            Destroy(gameObject);
        }

        #endregion
        
    }

}
