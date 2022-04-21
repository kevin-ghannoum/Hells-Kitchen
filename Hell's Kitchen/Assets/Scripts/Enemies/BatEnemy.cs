using Common.Enums;
using Common.Interfaces;
using Enemies.Enums;
using Photon.Pun;
using UnityEngine;

namespace Enemies
{
    public class BatEnemy : Enemy
    {
        [Header("General")]
        [SerializeField] private float aggroRadius = 20.0f;

        [Header("Melee Attack")]
        [SerializeField] private float attackRate = 0.5f;
        [SerializeField] private float attackDamage = 10f;
        [SerializeField] private float attackDamageRadius = 2f;
        [SerializeField] private Transform attackPosition;
        [SerializeField] private AudioClip attackSound;

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
            if (player != null && Vector3.Distance(player.transform.position, transform.position) < aggroRadius)
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
                    col.gameObject.GetComponent<IKillable>()?.PhotonView.RPC(nameof(IKillable.TakeDamage), RpcTarget.AllBufferedViaServer, attackDamage);
            }
        }

        [PunRPC]
        private void PlayAttackSoundRPC()
        {
            AudioSource.PlayClipAtPoint(attackSound, transform.position);
        }
    }
}
