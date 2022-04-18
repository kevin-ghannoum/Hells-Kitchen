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

        public UnityEvent Killed => _killed ??= new UnityEvent();

        public float HitPoints
        {
            get => GameStateData.playerCurrentHitPoints;
            set => GameStateData.playerCurrentHitPoints = value;
        }

        void Start()
        {
            photonView = GetComponent<PhotonView>();
        }

        void Update()
        {
            if (_invulnerabilityTimer < _invulnerabilityTime)
            {
                _invulnerabilityTimer += Time.deltaTime;
            }
        }

        public void TakeDamage(float damage)
        {
            // Invulnerability while rolling
            var animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (animatorStateInfo.IsName(PlayerAnimator.Roll))
                return;

            // Invulnerability after getting hit
            if (_invulnerabilityTimer < _invulnerabilityTime)
                return;

            // HP calculation and animation
            photonView.RPC("TakeHitRPC", RpcTarget.All);
            HitPoints -= damage;
            _invulnerabilityTimer = 0;

            // Damage numbers
            AdrenalinePointsUI.SpawnDamageNumbers(transform.position + 2.0f * Vector3.up, -damage);

            // If the player's hp is at 0 or lower, they die
            if (HitPoints <= 0)
            {
                HitPoints = 0;
                Die();
                return;
            }

            if (HitPoints > 25)
            {
                AudioSource.PlayClipAtPoint(takeDamageSound, transform.position);
            }
            else
            {
                AudioSource.PlayClipAtPoint(lowHealthSound, transform.position);
            }
        }

        private void Die()
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position);
            Killed.Invoke();
            photonView.RPC("DeadRPC", RpcTarget.All);
            Invoke(nameof(ReturnToRestaurant), transitionToRestaurantTime);
        }

        private void ReturnToRestaurant()
        {
            var playerController = gameObject.GetComponent<PlayerController>();
            if (playerController)
            {
                var heldWeapon = playerController.GetComponentInChildren<IPickup>();
                if (heldWeapon != null)
                {
                    GameStateData.carriedWeapon = WeaponInstance.None;
                    heldWeapon.RemoveFromPlayer();
                }
            }

            GameStateData.playerCurrentHitPoints = GameStateData.playerMaxHitPoints;
            SceneManager.Instance.LoadRestaurantScene();
        }

        [PunRPC]
        private void TakeHitRPC()
        {
            animator.SetTrigger(PlayerAnimator.TakeHit);
        }

        [PunRPC]
        private void DeadRPC()
        {
            animator.SetTrigger(PlayerAnimator.Dead);
        }
    }
}
