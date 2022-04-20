using Common.Enums;
using Common.Interfaces;
using Enemies.Enums;
using Player;
using UnityEngine;

namespace Enemies
{
    public class MushroomEnemy : Enemy
    {
        [SerializeField] private float Speed;
        [SerializeField] private float attackRange;
        private PlayerController target;
        [SerializeField] private GameObject bullet;
        [SerializeField] private Transform bulletPos;
        private float timeCounter = 2;
        private Vector3 direction;

        [SerializeField] private Animator anime;

        private void Start()
        {
            target = GameObject.FindWithTag(Tags.Player).GetComponent<PlayerController>();
            anime = gameObject.GetComponent<Animator>();
            attackRange = 20;
        }
        public override void Update()
        {
            timeCounter += Time.deltaTime;
            agent.Target = target.transform.position;
            direction = target.transform.position - transform.position;

            if (direction.magnitude < attackRange)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 200f);

                if (timeCounter > 2)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position + Vector3.up * 0.5f, (target.transform.position - transform.position).normalized, out hit))
                    {
                        if (hit.collider.gameObject.CompareTag(Tags.Player))
                        {
                            Invoke("shoot", 0.5f);
                            timeCounter = 0;
                        }
                    }
                }
            }
        }

        private void shoot()
        {
            animator.SetTrigger(EnemyAnimator.Attack);
            GameObject Bullet = GameObject.Instantiate(bullet, bulletPos.position, Quaternion.identity);
            Bullet.GetComponent<BulletControl>().direction = direction.normalized;
        }
    }
}
