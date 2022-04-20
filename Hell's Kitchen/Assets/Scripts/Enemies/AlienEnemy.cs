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
        [SerializeField] private PlayerController target;
        private float timeCounter;
        private float damageTimeCounter;
        private float damageRate = 0.1f;
        private float distance;
        private LineRenderer lr;
        [SerializeField] private float attackDamage = 1;
        private AudioSource audio;
        private int audioController = 0;

        private void Start()
        {
            if (!photonView.IsMine)
                return;

            attackRange = 12;
            target = GameObject.FindWithTag(Tags.Player).GetComponent<PlayerController>();
            lr = gameObject.GetComponent<LineRenderer>();
            audio = gameObject.GetComponent<AudioSource>();
        }

        public override void Update()
        {
            if (!photonView.IsMine)
                return;

            timeCounter += Time.deltaTime;
            damageTimeCounter += Time.deltaTime;
            distance = Vector3.Distance(transform.position, target.transform.position);

            if (distance < attackRange)
            {
                transform.LookAt(new Vector3(target.transform.position.x, 0, target.transform.position.z));
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
                        lr.positionCount = 2;
                        Vector3 startPos = new Vector3(transform.position.x, transform.position.y * 2, transform.position.z);

                        animator.SetTrigger(EnemyAnimator.Attack);

                        lr.SetPosition(0, transform.position + Vector3.up * 0.5f);
                        lr.SetPosition(1, target.transform.position + Vector3.up * 1.8f);

                        if (damageTimeCounter > damageRate)
                        {
                            target.gameObject.GetComponent<IKillable>().TakeDamage(attackDamage);
                        }
                    }
                }
            }
            else
            {
                lr.positionCount = 0;
                audioController = 0;
                audio.Stop();
            }
        }

        [PunRPC]
        private void playAttackSound()
        {
            audio.Play();
        }
    }

}
