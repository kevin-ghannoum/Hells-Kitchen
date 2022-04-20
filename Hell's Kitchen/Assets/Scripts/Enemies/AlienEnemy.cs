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
        
        private float timeCounter;
        private float damageTimeCounter;
        private float damageRate = 0.1f;
        private float distance;
        private LineRenderer lr;
        private AudioSource alienAudio;
        private AudioClip attack;
        private int audioController = 0;

        private void Start()
        {
            if (!photonView.IsMine)
                return;

            attackRange = 12;
            alienAudio = gameObject.GetComponent<AudioSource>();
        }

        public override void Update()
        {
            if (!photonView.IsMine)
                return;

            var target = FindClosestPlayer();
            timeCounter += Time.deltaTime;
            damageTimeCounter += Time.deltaTime;
            distance = Vector3.Distance(transform.position, target.transform.position);

            if (target != null && distance < attackRange)
            {
                transform.LookAt(target.transform);
                RaycastHit hit;
                if (Physics.Raycast(transform.position + Vector3.up * 0.5f, transform.forward, out hit))
                {
                    if (hit.collider.gameObject.CompareTag(Tags.Player))
                    {
                        if (audioController == 0)
                        {
                            photonView.RPC(nameof(playAttackSound), RpcTarget.All);
                            audioController++;
                        }

                        lr = photonLine.GetComponent<LineRenderer>();
                        Vector3 startPos = new Vector3(transform.position.x, transform.position.y * 2, transform.position.z);
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
                alienAudio.Stop();
                photonLine.GetComponent<LineController>().targetPhotonViewID = 0;
            }
        }

        [PunRPC]
        private void playAttackSound()
        {
            alienAudio.Play();
        }
    }
}
