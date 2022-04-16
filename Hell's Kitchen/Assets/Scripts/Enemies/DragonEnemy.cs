using Common.Enums;
using Common.Interfaces;
using Enemies.Enums;
using Player;
using UnityEngine;

namespace Enemies
{
    public class DragonEnemy : Enemy
    {
        [Header("Attack")]
        [SerializeField] private ParticleSystem particle;
        [SerializeField] private float attackRange;
        [SerializeField] private float followRange;
        [SerializeField] private PlayerController target;
        [SerializeField] private Animator anime;
        [SerializeField] private float attackDamage;
        [SerializeField] private float attackRate;
        private float distance;
        private float timeCounter = 5;
        private float eventCounter = 2;
        private bool isAttacking = false;

        private void Awake()
        {
            target = GameObject.FindWithTag(Tags.Player).GetComponent<PlayerController>();
            anime = gameObject.GetComponent<Animator>();
        }
        public override void Update()
        {
            timeCounter += Time.deltaTime;
            eventCounter += Time.deltaTime;
            distance = Vector3.Distance(transform.position, target.transform.position);

            if (distance < followRange)
            {
                agent.Target = target.transform.position;

                if (distance < attackRange && timeCounter > attackRate)
                {
                    eventCounter = 0;
                    particle.Play();
                    anime.SetBool("attacking", true);
                    Invoke("Attack", 0.3f);
                    timeCounter = 0;
                }
                else
                {
                    anime.SetBool("attacking", false);
                }
            }
        }

        private void Attack()
        {
            var colliders = Physics.OverlapSphere(transform.position, 3);

            foreach (var col in colliders)
            {
                if (col.gameObject.CompareTag(Tags.Player))
                {
                    col.gameObject.GetComponent<IKillable>().TakeDamage(attackDamage);
                }
            }
        }

        protected override void Die()
        {
            base.Die();
            Destroy(GetComponentInChildren<PigCollider>());
        }
    }
}