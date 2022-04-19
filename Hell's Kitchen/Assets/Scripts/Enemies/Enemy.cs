using Common.Interfaces;
using Enemies.Enums;
using Photon.Pun;
using UI;
using UnityEngine;

namespace Enemies
{
    
    [RequireComponent(typeof(PathfindingAgent))]
    public class Enemy : MonoBehaviour, IKillable, IPunObservable
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
        protected GameObject dropObject;

        [SerializeField] protected int maxDropsToSpawn = 3;
        
        [SerializeField][Range(0f, 1f)] private float multipleSpawnRate = 0.3f;

        [SerializeField] protected PhotonView photonView;

        private bool isKilled = false;

        #region Public Getters

        public float HitPoints => hitPoints;

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
            animator.SetTrigger(EnemyAnimator.Die);
            Invoke(nameof(Destroy), deathDelay);
            agent.enabled = false;
            enabled = false;
            ItemDropOnDeath();
            isKilled = true;
        }

        private void ItemDropOnDeath()
        {
            if (isKilled)
                return;
            
            var spawnPosition = new Vector3(gameObject.transform.position.x, 0.2f, gameObject.transform.position.z);
            var offset = new Vector3(1f, 0f, 0.5f);
            
            var shouldSpawnMore = Random.value < multipleSpawnRate;
            var numSpawned = 0;

            do {
                numSpawned++;
                PhotonNetwork.Instantiate(dropObject.name, spawnPosition, Quaternion.identity);
                spawnPosition += offset;
            } while (shouldSpawnMore && numSpawned < maxDropsToSpawn);
        }

        private void Destroy()
        {
            if (photonView.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
        
        #endregion

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(hitPoints);
            }
            else if (stream.IsReading)
            {
                stream.ReceiveNext();
            }
        }
    }

}
