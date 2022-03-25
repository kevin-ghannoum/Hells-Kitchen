using Common.Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace Enemies {
    
    [RequireComponent(typeof(PathfindingAgent))]
    public class Enemy : MonoBehaviour, IKillable {
        [Header("References")] 
        [SerializeField]
        protected PathfindingAgent agent;
    
        [SerializeField] 
        protected Animator animator;
        
        [Header("Parameters")] 
        [SerializeField]
        protected float hitPoints;
        
        [SerializeField]
        protected UnityEvent onKilled;

        #region Public Getters
    
        public float HitPoints => hitPoints;
        public UnityEvent Killed => onKilled;

        #endregion

        #region Unity Events

        public virtual void Update() {
            animator.SetFloat(EnemyAnimator.Speed, agent.Velocity.magnitude);
        }

        #endregion

        #region Public Methods

        public virtual void TakeDamage(float damage) {
            hitPoints -= damage;
        }

        #endregion
    }
    
}
