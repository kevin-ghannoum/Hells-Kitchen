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

        private void Start()
        {
            target = GameObject.FindWithTag(Tags.Player).GetComponent<PlayerController>();
            // Hive = GameObject.Find("Hive");
            anime = gameObject.GetComponent<Animator>();
            attackRange = 10;
        }

        public override void Update()
        {
            anime.SetBool("walking", true);
            anime.SetBool("attacking", false);
            timeCounter += Time.deltaTime;
            wanderTimeCounter += Time.deltaTime;

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

            // Vector3 hiveDirection = Hive.transform.position - transform.position;
            // hiveDirection.y = 0;
            // Vector3 targetDirection = target.transform.position - transform.position;
            // targetDirection.y = 0;

            // if (targetDirection.magnitude < 8)
            // {
            //     transform.LookAt(target.transform);
            //     transform.position += targetDirection.normalized * Speed * Time.deltaTime;
            // }
            // else
            // {
            //     if (hiveDirection.magnitude > 8)
            //     {
            //         transform.LookAt(Hive.transform);
            //         transform.position += hiveDirection.normalized * Speed * Time.deltaTime;
            //     }
            //     else
            //     {
            //         //wander
            //         if (wanderCheck)
            //         {
            //             wanderPos = new Vector3(Random.Range(Hive.transform.position.x - wanderRadius, Hive.transform.position.x + wanderRadius),
            //                                     0,
            //                                     Random.Range(Hive.transform.position.z - wanderRadius, Hive.transform.position.z + wanderRadius));

            //             wanderCheck = false;
            //         }

            //         Vector3 wanderDirection = wanderPos - transform.position;
            //         wanderDirection = new Vector3(wanderDirection.x, 0, wanderDirection.z);
            //         Debug.Log(Vector3.Distance(wanderPos, transform.position));

            //         if (Vector3.Distance(wanderPos, new Vector3(transform.position.x, 0, transform.position.z)) < 2f && wanderTimeCounter > Random.Range(2, 4))
            //         {
            //             wanderCheck = true;
            //             wanderTimeCounter = 0;
            //         }
            //         else if(Vector3.Distance(wanderPos, new Vector3(transform.position.x, 0, transform.position.z)) >= 2f)
            //         {
            //             transform.rotation = Quaternion.LookRotation(wanderDirection);
            //             transform.position += wanderDirection.normalized * Speed * Time.deltaTime;
            //         }
            //     }
            // }
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
    }
}
