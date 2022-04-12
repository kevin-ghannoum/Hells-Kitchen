using System;
using Common.Enums;
using Common.Interfaces;
using Enemies.Enums;
using Player;
using UnityEngine;

namespace Enemies
{
    public class EnemyBat : Enemy
    {
        [Header("Parameters")]
        [SerializeField] private float attackRate = 0.5f;

        [SerializeField] private float aggroRadius = 20.0f;

        [SerializeField] private float attackDamage = 10f;
        
        [SerializeField] private float attackAnimationDelay = 0.05f;
        
        [SerializeField] private float attackDamageRadius = 2f;

        private PlayerController _player;
        private float _lastAttack;
        

        private void Start()
        {
            _player = GameObject.FindWithTag(Tags.Player).GetComponent<PlayerController>();
        }

        public override void Update()
        {
            base.Update();

            if (Vector3.Distance(_player.transform.position, transform.position) < aggroRadius)
            {
                agent.Target = _player.transform.position;
            
                if (agent.Arrived && Time.time - _lastAttack > (1.0f / attackRate))
                {
                    PerformAttack();
                    _lastAttack = Time.time;
                }
            }
        }

        private void PerformAttack()
        {
            animator.SetTrigger(EnemyAnimator.Attack);
            Invoke(nameof(InflictDamage), attackAnimationDelay);
        }

        private void InflictDamage()
        {
            var colliders = Physics.OverlapSphere(transform.position, attackDamageRadius);
            
            foreach(var col in colliders)
            {
                if (col.gameObject.CompareTag(Tags.Player))
                    col.gameObject.GetComponent<IKillable>().TakeDamage(attackDamage);
            }
        }
    }
}
