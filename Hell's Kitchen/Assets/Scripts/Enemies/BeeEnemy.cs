using Common.Enums;
using UnityEngine;
using Common.Interfaces;
using Enemies.Enums;
using Player;
using Photon.Pun;

namespace Enemies
{
    public class BeeEnemy : Enemy
    {
        private float Speed = 5;
        [SerializeField] private float attackRange;
        [SerializeField] private float attackDamage;
        private float timeCounter = 2;
        private float wanderTimeCounter;
        private float attackDur = 2;
        private GameObject Hive;
        private Vector3 wanderPos;
        private float wanderRadius = 7f;
        private bool wanderCheck = true;
        private GameObject[] hiveList;
        private GameObject targetHive;
        [SerializeField] private AudioClip attackSound;

        private void Start()
        {
            if (!photonView.IsMine)
                return;

            hiveList = GameObject.FindGameObjectsWithTag("Hive");
            attackRange = 10;
        }

        public override void Update()
        {
            if (!photonView.IsMine)
                return;

            var target = FindClosestPlayer();
            hiveList = GameObject.FindGameObjectsWithTag(Tags.Hive);
            timeCounter += Time.deltaTime;
            if (hiveList.Length > 0)
            {
                targetHive = checkHive(hiveList);

                if (target != null && Vector3.Distance(transform.position, target.transform.position) < attackRange)
                {
                    agent.enabled = true;
                    agent.Target = target.transform.position;
                    if (Vector3.Distance(target.transform.position, transform.position) < 2 && timeCounter > 2)
                    {
                        animator.SetTrigger(EnemyAnimator.Attack);
                        Attack();
                        timeCounter = 0;
                    }
                }
                else if (Vector3.Distance(transform.position, targetHive.transform.position) > 7)
                {
                    agent.enabled = true;
                    agent.Target = targetHive.transform.position;
                }
                else
                {
                    agent.enabled = false;
                }
            }
            else
            {
                if (target != null && Vector3.Distance(transform.position, target.transform.position) < attackRange)
                {
                    agent.Target = target.transform.position;

                    if (Vector3.Distance(target.transform.position, transform.position) < 1.2 && timeCounter > 1)
                    {
                        Attack();
                        timeCounter = 0;
                    }
                }
            }
        }

        private void Attack()
        {
            if (!photonView.IsMine)
                return;

            photonView.RPC(nameof(PlayAttackSoundRPC), RpcTarget.All);
            animator.SetTrigger(EnemyAnimator.Attack);
            var colliders = Physics.OverlapSphere(transform.position, 3);

            foreach (var col in colliders)
            {
                if (col.gameObject.CompareTag(Tags.Player))
                    col.gameObject.GetComponent<IKillable>()?.PhotonView.RPC(nameof(IKillable.TakeDamage), RpcTarget.All, attackDamage);
            }
        }

        private GameObject checkHive(GameObject[] hiveList)
        {

            GameObject currentTarget = hiveList[0];
            foreach (GameObject hive in hiveList)
            {
                if (Vector3.Distance(transform.position, hive.transform.position) < Vector3.Distance(transform.position, currentTarget.transform.position))
                {
                    currentTarget = hive;
                }
            }
            return currentTarget;
        }

        [PunRPC]
        private void PlayAttackSoundRPC()
        {
            AudioSource.PlayClipAtPoint(attackSound, transform.position);
        }
    }
}
