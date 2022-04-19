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
        [Header("General")]
        [SerializeField] private float aggroRadius = 20.0f;

        [Header("Melee Attack")]
        [SerializeField] private float attackRate = 0.5f;
        [SerializeField] private float attackDamage = 10f;
        [SerializeField] private float attackDamageRadius = 2f;
        [SerializeField] private Transform attackPosition;
        
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

                if (agent.IsArrived && Time.time - _lastAttack > (1.0f / attackRate))
                {
                    PerformAttack();
                    _lastAttack = Time.time;
                }
            }
        }

        private void PerformAttack()
        {
            animator.SetTrigger(EnemyAnimator.Attack);
        }

        public void InflictDamage()
        {
            if (!photonView.IsMine)
                return;
            
            var colliders = Physics.OverlapSphere(attackPosition.position, attackDamageRadius);
            foreach(var col in colliders)
            {
                if (!col.CompareTag(Tags.Enemy))
                    col.gameObject.GetComponent<IKillable>()?.TakeDamage(attackDamage);
            }
        }
    }
}
