using System;
using Common.Enums;
using Common.Interfaces;
using Enemies.Enums;
using Player;
using UnityEngine;

namespace Enemies
{
    public class EnemyPig : Enemy
    {
        [Header("Parameters")]
        [SerializeField] private float attackRate = 0.5f;
        [SerializeField] private float attackDamage = 15.0f;
        [SerializeField] private float aggroRadius = 20.0f;

        [SerializeField] private float chargeSpeed = 10;
        [SerializeField] private float chargeTime = 3;
        [SerializeField] private float chargeRange = 10.0f;

        private PlayerController _player;
        private float _currentChargeTime;
        private float _lastAttack;
        private Vector3 _chargeDirection;
        private Rigidbody _rb;

        private void Start()
        {
            _player = GameObject.FindWithTag(Tags.Player).GetComponent<PlayerController>();
            _rb = GetComponent<Rigidbody>();
        }

        public override void Update()
        {
            if (Vector3.Distance(_player.transform.position, transform.position) < aggroRadius)
            {
                agent.Target = _player.transform.position;
                PerformAttack();
            }
        }

        private void PerformAttack()
        {
            if (_currentChargeTime >= chargeTime && Time.time -_lastAttack > (1 / attackRate))
            {
                animator.SetBool(EnemyAnimator.Attack, false);
                _rb.velocity = Vector3.zero;
                if (Physics.Raycast(transform.position + Vector3.up * 0.5f, (_player.transform.position - transform.position).normalized, out var hit,
                    chargeRange))
                {
                    if (hit.collider.CompareTag(Tags.Player))
                    {
                        agent.enabled = false;
                        _chargeDirection = (_player.transform.position - transform.position).normalized;
                        _currentChargeTime = -1;
                        _lastAttack = Time.time;
                        animator.SetBool(EnemyAnimator.Attack, true);
                        transform.rotation = Quaternion.LookRotation(_chargeDirection);
                    }
                }
            }
            else
            {
                _currentChargeTime += Time.deltaTime;
                if (_currentChargeTime < 0)
                {
                    _rb.velocity = _chargeDirection * chargeSpeed;
                }
                else
                {
                    agent.enabled = true;
                }
            }
        }

        protected override void Die()
        {
            base.Die();
            Destroy(GetComponentInChildren<PigCollider>());
        }

        public void OnPigTrigger(Collider col)
        {
            if (_currentChargeTime < chargeTime && col.CompareTag(Tags.Player))
            {
                col.GetComponent<IKillable>().TakeDamage(attackDamage);
            }
        }
    }
}
