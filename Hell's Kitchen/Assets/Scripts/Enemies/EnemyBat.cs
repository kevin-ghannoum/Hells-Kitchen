using System;
using Common.Enums;
using Common.Interfaces;
using Enemies.Enums;
using Photon.Pun;
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
        [SerializeField] private AudioClip attackSound;
        [SerializeField] private AudioClip deathSound;

        private float _lastAttack;

        private void Start()
        {
            if (!photonView.IsMine)
                return;
        }

        public override void Update()
        {
            if (!photonView.IsMine)
                return;

            base.Update();

            var player = FindClosestPlayer();
            if (Vector3.Distance(player.transform.position, transform.position) < aggroRadius)
            {
                agent.Target = player.transform.position;

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
            photonView.RPC(nameof(PlayAttackSoundRPC), RpcTarget.All);
        }

        public void InflictDamage()
        {
            if (!photonView.IsMine)
                return;

            var colliders = Physics.OverlapSphere(attackPosition.position, attackDamageRadius);
            foreach (var col in colliders)
            {
                if (!col.CompareTag(Tags.Enemy))
                    col.gameObject.GetComponent<IKillable>()?.PhotonView.RPC(nameof(IKillable.TakeDamage), RpcTarget.All, attackDamage);
            }
        }

        protected override void Die()
        {
            photonView.RPC(nameof(PlayDeathSoundRPC), RpcTarget.All);
            base.Die();
        }

        [PunRPC]
        private void PlayAttackSoundRPC()
        {
            AudioSource.PlayClipAtPoint(attackSound, transform.position);
        }

        [PunRPC]
        private void PlayDeathSoundRPC()
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position);
        }
    }
}
