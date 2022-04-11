using System;
using Common.Enums;
using Enemies.Enums;
using Player;
using UnityEngine;

namespace Enemies
{
    public class EnemyBat : Enemy
    {
        [Header("Parameters")]
        [SerializeField]
        private float attackRate = 0.5f;

        [SerializeField]
        private float aggroRadius = 20.0f;

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
        }

    }
}
