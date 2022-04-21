using Common;
using Common.Enums;
using Common.Interfaces;
using Input;
using UI;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

namespace Player
{
    public class PlayerHealth : MonoBehaviour, IKillable
    {
        [SerializeField] private Animator animator;
        [SerializeField] private PhotonView photonView;
        [SerializeField] private AudioClip takeDamageSound;
        [SerializeField] private AudioClip lowHealthSound;
        [SerializeField] private AudioClip deathSound;
        [SerializeField] private float transitionToRestaurantTime = 4f;
        private float _invulnerabilityTime = 1;
        private float _invulnerabilityTimer = 1;
        private UnityEvent _killed;

        public float internalHealth = GameStateData.playerMaxHitPoints; //need this local variable, HitPoints can't be used becos it's a scene obj Xd
        public UnityEvent Killed => _killed ??= new UnityEvent();

        public float HitPoints
        {
            //get => GameStateData.playerCurrentHitPoints;
            //set => GameStateData.playerCurrentHitPoints = Mathf.Clamp(value, 0, GameStateData.playerMaxHitPoints);
            get => internalHealth;
            set => internalHealth = Mathf.Clamp(value, 0, GameStateData.playerMaxHitPoints);
        }

        public PhotonView PhotonView => photonView;

        void Start()
        {
            photonView = GetComponent<PhotonView>();
            //if (PhotonView.IsMine)
            //    internalHealth = GameStateData.playerCurrentHitPoints;
        }

        void Update()
        {
            //if (PhotonView.IsMine)
            //    internalHealth = GameStateData.playerCurrentHitPoints;
            if (_invulnerabilityTimer < _invulnerabilityTime)
            {
                _invulnerabilityTimer += Time.deltaTime;
            }
        }

        [PunRPC]
        public void TakeDamage(float damage)
        {
            // Invulnerability while rolling
            var animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (animatorStateInfo.IsName(PlayerAnimator.Roll))
                return;

            // Invulnerability after getting hit
            if (_invulnerabilityTimer < _invulnerabilityTime)
                return;

            _invulnerabilityTimer = 0;

            //we want to subtract internal health on all clients, for the player that got hit xDD
            // HP calculation and animation
            //if (photonView.IsMine)
            {
                animator.SetTrigger(PlayerAnimator.TakeHit);
                HitPoints -= damage;
                

                // If the player's hp is at 0 or lower, they die
                if (HitPoints <= 0)
                {
                    HitPoints = 0;
                    Die();
                    return;
                }

                if (HitPoints > 25)
                {
                    photonView.RPC(nameof(PlayTakeDamageSoundRPC), RpcTarget.All);
                }
                else
                {
                    photonView.RPC(nameof(PlayLowHealthSoundRPC), RpcTarget.All);
                }
            }

            // Damage numbers
            AdrenalinePointsUI.SpawnDamageNumbers(transform.position + 2.0f * Vector3.up, -damage);
        }

        [PunRPC]
        public void IncreaseMyHP(float amount)
        {
            HitPoints = Mathf.Clamp(HitPoints + amount, 0, GameStateData.playerMaxHitPoints);
        }

        [PunRPC]
        private void PlayTakeDamageSoundRPC()
        {
            AudioSource.PlayClipAtPoint(takeDamageSound, transform.position);
        }

        [PunRPC]
        private void PlayLowHealthSoundRPC()
        {
            AudioSource.PlayClipAtPoint(lowHealthSound, transform.position);
        }

        private void Die()
        {
            photonView.RPC(nameof(PlayDeathSoundRPC), RpcTarget.All);
            Killed.Invoke();
            animator.SetTrigger(PlayerAnimator.Dead);
            Invoke(nameof(ReturnToRestaurant), transitionToRestaurantTime);
        }

        [PunRPC]
        public void PlayDeathSoundRPC()
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position);
        }
        
        private void ReturnToRestaurant()
        {
            SceneManager.Instance.LoadRestaurantScene();
        }
    }
}
