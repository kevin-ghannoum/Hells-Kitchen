using Common.Enums;
using Common.Interfaces;
using Enemies.Enums;
using Player;
using UnityEngine;
using Photon.Pun;

namespace Enemies
{
    public class DragonEnemy : Enemy
    {
        [Header("Attack")]
        [SerializeField] private ParticleSystem particle;
        [SerializeField] private float attackRange;
        [SerializeField] private float followRange;
        [SerializeField] private float attackDamage;
        [SerializeField] private float attackRate;
        private float distance;
        private float timeCounter = 5;
        private AudioClip attack;
        private AudioSource dragonAudio;
        private int audioController = 0;

        private void Start()
        {
            if (!photonView.IsMine)
                return;

            dragonAudio = gameObject.GetComponent<AudioSource>();
            attack = dragonAudio.clip;
        }
        public override void Update()
        {
            if (!photonView.IsMine)
                return;

            var target = FindClosestPlayer();
            timeCounter += Time.deltaTime;
            distance = Vector3.Distance(transform.position, target.transform.position);

            if (distance < followRange)
            {
                agent.Target = target.transform.position;

                if (distance < attackRange && timeCounter > attackRate)
                {
                    agent.enabled = false;
                    particle.Play();
                    photonView.RPC(nameof(playAttackSound), RpcTarget.All);
                    animator.SetTrigger(EnemyAnimator.Attack);
                    setCircle();
                    Invoke("Attack", 0.3f);
                    Invoke("resumeFollow", 1f);
                    timeCounter = 0;
                }
            }
            else
            {
                audioController = 0;
            }
        }

        private void setCircle()
        {
            GameObject spell = PhotonNetwork.Instantiate(particle.name, transform.position, Quaternion.identity);
            spell.GetComponent<DragonParticleController>().parent = this.gameObject;
        }

        private void Attack()
        {
            if (!photonView.IsMine)
                return;
            
            var colliders = Physics.OverlapSphere(transform.position, 3);

            foreach (var col in colliders)
            {
                if (col.gameObject.CompareTag(Tags.Player))
                {
                    col.gameObject.GetComponent<IKillable>()?.PhotonView.RPC(nameof(IKillable.TakeDamage), RpcTarget.All, attackDamage);
                }
            }
        }

        private void resumeFollow()
        {
            agent.enabled = true;
        }

        [PunRPC]
        private void playAttackSound()
        {
            AudioSource.PlayClipAtPoint(attack, transform.position);
        }
    }
}