using Common.Enums;
using Common.Interfaces;
using Enemies.Enums;
using UnityEngine;
using Photon.Pun;

namespace Enemies
{
    public class AlienEnemy : Enemy
    {
        [Header("Alien Properties")]
        [SerializeField] private float attackRange;
        [SerializeField] private float attackDamage = 1;
        [SerializeField] private GameObject photonLine;
        [SerializeField] private AudioClip attackSound;

        private float timeCounter;
        private float damageTimeCounter;
        private float damageRate = 0.1f;
        private float distance;
        private LineRenderer lr;
        private int audioController = 0;

        private void Start()
        {
            if (!photonView.IsMine)
                return;

            attackRange = 12;
        }

        public override void Update()
        {
            if (!photonView.IsMine)
                return;

            var target = FindClosestPlayer();
            if (target == null)
                return;

            timeCounter += Time.deltaTime;
            damageTimeCounter += Time.deltaTime;
            distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance < attackRange)
            {
                transform.LookAt(target.transform);
                RaycastHit hit;
                if (Physics.Raycast(transform.position + Vector3.up * 0.5f, transform.forward, out hit))
                {
                    if (hit.collider.gameObject.CompareTag(Tags.Player))
                    {
                        if (audioController == 0)
                        {
                            photonView.RPC(nameof(PlayAttackSoundRPC), RpcTarget.AllBufferedViaServer);
                            audioController++;
                        }

                        lr = photonLine.GetComponent<LineRenderer>();
                        photonLine.GetComponent<LineController>().targetPhotonViewID = target.GetComponent<PhotonView>().ViewID;
                        animator.SetTrigger(EnemyAnimator.Attack);

                        if (damageTimeCounter > damageRate)
                        {
                            target.gameObject.GetComponent<IKillable>()?.PhotonView.RPC(nameof(IKillable.TakeDamage), RpcTarget.All, attackDamage);
                        }
                    }
                }
            }
            else
            {
                audioController = 0;
                photonLine.GetComponent<LineController>().targetPhotonViewID = 0;
            }
        }

        [PunRPC]
        private void PlayAttackSoundRPC()
        {
            AudioSource.PlayClipAtPoint(attackSound, transform.position);
        }
    }
}
