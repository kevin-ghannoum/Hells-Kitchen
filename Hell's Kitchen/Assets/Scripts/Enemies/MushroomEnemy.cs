using Common.Enums;
using Common.Interfaces;
using Enemies.Enums;
using Player;
using UnityEngine;
using Photon.Pun;

namespace Enemies
{
    public class MushroomEnemy : Enemy
    {
        [SerializeField] private float Speed;
        [SerializeField] private float attackRange;
        [SerializeField] private GameObject bullet;
        [SerializeField] private Transform bulletPos;
        private float timeCounter = 2;
        private Vector3 direction;

        [SerializeField] private Animator anime;

        private void Start()
        {
            if (!photonView.IsMine)
                return;

            anime = gameObject.GetComponent<Animator>();
            attackRange = 20;
        }
        public override void Update()
        {
            if (!photonView.IsMine)
                return;

            var target = FindClosestPlayer();
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
            if (!photonView.IsMine)
                return;

            animator.SetTrigger(EnemyAnimator.Attack);
            GameObject Bullet = PhotonNetwork.Instantiate(bullet.name, bulletPos.position, Quaternion.identity);
            Bullet.GetComponent<BulletControl>().direction = direction.normalized;
        }
    }
}
