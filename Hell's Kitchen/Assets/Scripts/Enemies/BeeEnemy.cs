using Common.Enums;
using UnityEngine;
using Common.Interfaces;
using Enemies.Enums;
using Player;

namespace Enemies
{
    public class BeeEnemy : Enemy
    {
        private float Speed = 5;
        [SerializeField] private float attackRange;
        private PlayerController target;
        private float timeCounter = 2;
        private float wanderTimeCounter;
        private float attackDur = 2;
        private GameObject Hive;
        private Vector3 wanderPos;
        private float wanderRadius = 7f;
        private bool wanderCheck = true;
        [SerializeField] private Animator anime;
        private GameObject[] hiveList;
        private GameObject targetHive;

        private void Start()
        {
            target = GameObject.FindWithTag(Tags.Player).GetComponent<PlayerController>();
            hiveList = GameObject.FindGameObjectsWithTag("Hive");
            anime = gameObject.GetComponent<Animator>();
            attackRange = 10;
        }

        public override void Update()
        {
            if (hiveList.Length > 0)
            {
                targetHive = checkHive(hiveList);
            }

            if (Vector3.Distance(transform.position, targetHive.transform.position) > 7)
            {
                anime.SetBool("walking", true);
                agent.Target = targetHive.transform.position;
            }
            else
            {
                anime.SetBool("walking", true);
                anime.SetBool("attacking", false);
                timeCounter += Time.deltaTime;

                if (Vector3.Distance(target.transform.position, transform.position) < attackRange)
                {
                    agent.Target = target.transform.position;

                    if (Vector3.Distance(target.transform.position, transform.position) < 1.2 && timeCounter > 1)
                    {
                        anime.SetBool("walking", false);
                        anime.SetBool("attacking", true);
                        Attack();
                        timeCounter = 0;
                    }
                }
            }
        }

        private void Attack()
        {
            var colliders = Physics.OverlapSphere(transform.position, 3);

            foreach (var col in colliders)
            {
                if (col.gameObject.CompareTag(Tags.Player))
                    col.gameObject.GetComponent<IKillable>().TakeDamage(5);
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
    }
}
