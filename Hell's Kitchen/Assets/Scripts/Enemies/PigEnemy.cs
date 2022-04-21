using Common.Enums;
using Common.Interfaces;
using Enemies.Enums;
using Photon.Pun;
using UnityEngine;

namespace Enemies
{
    public class PigEnemy : Enemy
    {
        [Header("Parameters")]
        [SerializeField] private float attackRate = 0.5f;
        [SerializeField] private float chargeDamage = 15.0f;
        [SerializeField] private float attackDamage = 5.0f;
        [SerializeField] private float aggroRadius = 20.0f;

        [SerializeField] private float chargeSpeed = 10;
        [SerializeField] private float chargeTime = 3;
        [SerializeField] private float chargeRange = 10.0f;
        [SerializeField] private AudioClip idleSound;
        [SerializeField] private AudioClip attackSound;

        private AudioSource _as;
        private float _currentChargeTime;
        private float _lastAttack;
        private Vector3 _chargeDirection;
        private Rigidbody _rb;

        private void Start()
        {
            if (!photonView.IsMine)
                return;

            _rb = GetComponent<Rigidbody>();
            _as = GetComponent<AudioSource>();
            _as.loop = true;
            photonView.RPC(nameof(PlayIdleSoundRPC), RpcTarget.All);
        }

        public override void Update()
        {
            if (!photonView.IsMine)
                return;

            var player = FindClosestPlayer();
            if (player != null)
            {
                if (Vector3.Distance(player.transform.position, transform.position) < aggroRadius)
                {
                    agent.Target = player.transform.position;
                    PerformAttack(player);
                }
            }
        }

        private void PerformAttack(GameObject player)
        {
            if (_currentChargeTime >= chargeTime && Time.time - _lastAttack > (1 / attackRate))
            {
                animator.SetBool(EnemyAnimator.Attack, false);
                _rb.velocity = Vector3.zero;
                if (Physics.Raycast(transform.position + Vector3.up * 0.5f, (player.transform.position - transform.position).normalized, out var hit,
                    chargeRange))
                {
                    if (hit.collider.CompareTag(Tags.Player))
                    {
                        agent.enabled = false;
                        _chargeDirection = (player.transform.position - transform.position).normalized;
                        _currentChargeTime = -1;
                        _lastAttack = Time.time;
                        animator.SetBool(EnemyAnimator.Attack, true);
                        transform.rotation = Quaternion.LookRotation(_chargeDirection);
                        photonView.RPC(nameof(PlayAttackSoundRPC), RpcTarget.All);
                    }
                }
            }
            else
            {
                _currentChargeTime += Time.deltaTime;
                if (_currentChargeTime < 0)
                {
                    _rb.velocity = _chargeDirection * chargeSpeed;
                }
                else
                {
                    agent.enabled = true;
                    if (_as.clip == attackSound)
                    {
                        photonView.RPC(nameof(PlayIdleSoundRPC), RpcTarget.All);
                    }
                }
            }
        }

        [PunRPC]
        private void PlayIdleSoundRPC()
        {
            if (_as != null)
            {
                _as.clip = idleSound;
                _as.Play();
            }
        }

        [PunRPC]
        private void PlayAttackSoundRPC()
        {
            if (_as != null)
            {
                _as.clip = attackSound;
                _as.Play();
            }
        }

        protected override void Die()
        {
            Destroy(GetComponentInChildren<PigCollider>());
            base.Die();
        }

        public void OnPigTrigger(Collider col)
        {
            if (photonView.IsMine && !col.CompareTag(Tags.Enemy) && _currentChargeTime >= 0)
            {
                col.gameObject.GetComponent<IKillable>()?.PhotonView.RPC(nameof(IKillable.TakeDamage), RpcTarget.AllBufferedViaServer, attackDamage);
            }
        }

        public void OnPigChargeTrigger(Collider col)
        {
            if (photonView.IsMine && !col.CompareTag(Tags.Enemy) && _currentChargeTime < 0)
            {
                col.gameObject.GetComponent<IKillable>()?.PhotonView.RPC(nameof(IKillable.TakeDamage), RpcTarget.AllBufferedViaServer, chargeDamage);
            }
        }
    }
}
