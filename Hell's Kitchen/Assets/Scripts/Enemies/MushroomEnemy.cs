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
        [SerializeField] private Animator anime;

        [SerializeField] private AudioClip attackSound;

        private float timeCounter = 2;
        private Vector3 direction;

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
            if (target == null)
                return;

            timeCounter += Time.deltaTime;
            direction = target.transform.position - transform.position;

            if (direction.magnitude < attackRange)
            {
                agent.Target = target.transform.position;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 200f);

                if (timeCounter > 2)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position + Vector3.up * 0.5f, (target.transform.position - transform.position).normalized, out hit))
                    {
                        if (hit.collider.gameObject.CompareTag(Tags.Player))
                        {
                            animator.SetTrigger(EnemyAnimator.Attack);
                            Shoot();
                            timeCounter = 0;
                        }
                    }
                }
            }
        }

        private void Shoot()
        {
            if (!photonView.IsMine)
                return;

            photonView.RPC(nameof(PlayAttackSoundRPC), RpcTarget.All);
            GameObject Bullet = PhotonNetwork.Instantiate(bullet.name, bulletPos.position, Quaternion.identity);
            Bullet.GetComponent<BulletControl>().direction = direction.normalized;
        }

        [PunRPC]
        private void PlayAttackSoundRPC()
        {
            AudioSource.PlayClipAtPoint(attackSound, transform.position);
        }
    }
}
