using Common.Enums;
using Common.Interfaces;
using Enemies.Enums;
using Player;
using UnityEngine;
using Photon.Pun;

namespace Enemies
{
    public class AlienEnemy : Enemy
    {
        [SerializeField] private float attackRange;
        private float timeCounter;
        private float damageTimeCounter;
        private float damageRate = 0.1f;
        private float distance;
        [SerializeField] private GameObject electricLine;
        private LineRenderer lr;
        [SerializeField] private float attackDamage = 1;
        private AudioSource alienAudio;
        private AudioClip attack;
        private int audioController = 0;

        private void Start()
        {
            if (!photonView.IsMine)
                return;

            attackRange = 12;

            lr = electricLine.GetComponent<LineRenderer>();
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
                            photonView.RPC(nameof(playAttackSound), RpcTarget.All);
                            audioController++;
                        }

                        Vector3 startPos = new Vector3(transform.position.x, transform.position.y * 2, transform.position.z);

                        animator.SetTrigger(EnemyAnimator.Attack);

                        setLinePosition(target);

                        if (damageTimeCounter > damageRate)
                        {
                            target.gameObject.GetComponent<IKillable>()?.PhotonView.RPC(nameof(IKillable.TakeDamage), RpcTarget.All, attackDamage);
                        }
                    }
                }
            }
            else
            {
                lr.GetComponent<LineRenderer>().positionCount = 0;
                audioController = 0;
                alienAudio.Stop();
            }
        }

        [PunRPC]
        private void playAttackSound()
        {
            alienAudio.Play();
        }

        [PunRPC]
        private void setLinePosition(GameObject target)
        {
            if (!photonView.IsMine)
                return;

            lr.GetComponent<LineRenderer>().positionCount = 2;
            lr.GetComponent<LineRenderer>().SetPosition(0, transform.position + Vector3.up * 0.5f);
            lr.GetComponent<LineRenderer>().SetPosition(1, target.transform.position + Vector3.up * 1.8f);
        }
    }

}
