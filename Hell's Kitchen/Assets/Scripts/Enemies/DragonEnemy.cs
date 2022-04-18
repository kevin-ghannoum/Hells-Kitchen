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
        [SerializeField] private float attackDamage;
        [SerializeField] private float attackRate;
        private float distance;
        private float timeCounter = 5;
        private AudioSource audio;
        private int audioController = 0;

        private void Awake()
        {
            target = GameObject.FindWithTag(Tags.Player).GetComponent<PlayerController>();
            audio = gameObject.GetComponent<AudioSource>();
        }
        public override void Update()
        {
            timeCounter += Time.deltaTime;
            distance = Vector3.Distance(transform.position, target.transform.position);

            if (distance < followRange)
            {
                agent.Target = target.transform.position;

                if (distance < attackRange && timeCounter > attackRate)
                {
                    agent.enabled = false;
                    particle.Play();
                    audio.Play();
                    animator.SetTrigger(EnemyAnimator.Attack);
                    Invoke("Attack", 0.3f);
                    Invoke("resumeFollow", 1f);
                    timeCounter = 0;
                }
            }
            else
            {
                audioController = 0;
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

        private void resumeFollow()
        {
            agent.enabled = true;
        }
    }
}