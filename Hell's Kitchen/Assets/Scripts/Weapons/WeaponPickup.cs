using System;
using Common;
using Common.Enums;
using Common.Interfaces;
using Player;
using UnityEngine;
using Input;
using Photon.Pun;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace Weapons
{
    [RequireComponent(typeof(Rigidbody))]
    public class WeaponPickup : MonoBehaviour, IWeapon
    {
        [Header("Weapon Offset")]
        [SerializeField] private Vector3 position = Vector3.zero;
        [SerializeField] private Quaternion rotation = Quaternion.identity;
        [SerializeField] private float scale = 0.3f;

        [Header("Throwing")]
        [SerializeField] private float throwSpeed = 35.0f;
        [SerializeField] private float throwAngularSpeed = 180.0f;

        [Header("References")]
        [SerializeField]
        private new Rigidbody rigidbody;

        protected PlayerController playerController;
        protected Animator playerAnimator;
        private bool _canBePickedUp = true;

        [SerializeField] private float _damage = 10.0f;

        protected PhotonView photonView;

        public float Damage
        {
            get => _damage;
            set => _damage = value;
        }

        public virtual float Price { get; } = 10f;

        [SerializeField] private WeaponInstance weaponInstance;
        public WeaponInstance WeaponInstance { get => weaponInstance; }

        public void Reset()
        {
            rigidbody = GetComponent<Rigidbody>();
        }

        private void Awake()
        {
            photonView = GetComponent<PhotonView>();
        }

        public virtual void PickUp()
        {
            GetComponentsFromLocalPlayer();
            _canBePickedUp = false;

            TransferOwnership();
            photonView.RPC(nameof(PickUpRPC), RpcTarget.All, true);
            photonView.RPC(nameof(ReparentObjectToPlayerHandRPC), RpcTarget.All, NetworkHelper.GetLocalPlayerPhotonView().ViewID);
            photonView.RPC(nameof(DisableRigidbody), RpcTarget.All);
            GameStateData.carriedWeapon = WeaponInstance;
            AddListeners();
        }

        public void Drop(InputAction.CallbackContext callbackContext)
        {
            RemoveFromPlayer();
        }

        public void RemoveFromPlayer()
        {
            RemoveListeners();

            photonView.RPC(nameof(PickUpRPC), RpcTarget.All, false);
            photonView.RPC(nameof(RemoveObjectParentRPC), RpcTarget.All);
            photonView.RPC(nameof(EnableRigidBody), RpcTarget.All);
            GameStateData.carriedWeapon = WeaponInstance.None;
        }

        public void Throw(InputAction.CallbackContext callbackContext)
        {
            transform.position = playerController.DamagePosition.position;
            transform.rotation = playerController.transform.rotation;
            rigidbody.velocity = playerController.transform.forward * throwSpeed;
            rigidbody.angularVelocity = new Vector3(
                Random.Range(-throwAngularSpeed, throwAngularSpeed),
                Random.Range(-throwAngularSpeed, throwAngularSpeed),
                Random.Range(-throwAngularSpeed, throwAngularSpeed)
            );
            RemoveListeners();

            photonView.RPC(nameof(PickUpRPC), RpcTarget.All, false);
            photonView.RPC(nameof(RemoveObjectParentRPC), RpcTarget.All);
            photonView.RPC(nameof(EnableRigidBody), RpcTarget.All);
            GameStateData.carriedWeapon = WeaponInstance.None;
        }

        private void GetComponentsFromLocalPlayer()
        {
            GameObject playerObject = NetworkHelper.GetLocalPlayerObject();
            playerController = playerObject.GetComponent<PlayerController>();
            playerAnimator = playerObject.GetComponentInChildren<Animator>();
        }

        [PunRPC]
        public void ReparentObjectToPlayerHandRPC(int viewID)
        {
            playerController = PhotonView.Find(viewID).gameObject.GetComponent<PlayerController>();
            Transform hand = playerController?.CharacterHand;
            if (!hand)
                throw new MissingReferenceException();


            gameObject.transform.SetParent(hand, false);
            gameObject.transform.localScale = new Vector3(scale, scale, scale);
            gameObject.transform.localPosition = position;
            gameObject.transform.localRotation = rotation;
        }

        [PunRPC]
        public void RemoveObjectParentRPC()
        {
            transform.SetParent(null);
            transform.localScale = new Vector3(1, 1, 1);
        }

        private Vector3 GetGunHeightAimPoint(Ray mouseAim, RaycastHit hitInfo)
        {
            float gunHeight = playerController.ShootHeight;
            if (hitInfo.collider != null && hitInfo.point.y > gunHeight)
            {
                Vector3 heightAdjusted = hitInfo.point;
                heightAdjusted.y = gunHeight;
                return heightAdjusted;
            }

            Plane aimPlane = new Plane(Vector3.up, Vector3.up * gunHeight);
            if (aimPlane.Raycast(mouseAim, out float distance))
            {
                return mouseAim.GetPoint(distance);
            }

            return Vector3.zero;
        }

        public virtual void Use(InputAction.CallbackContext callbackContext)
        {
            Vector2 mouse = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(mouse);
            Physics.Raycast(ray, out RaycastHit hitInfo);
            var aimPoint = GetGunHeightAimPoint(ray, hitInfo);
            aimPoint.y = 0;
            playerController.FaceTarget(aimPoint);
        }

        protected virtual void AddListeners()
        {
            InputManager.Actions.Attack.performed += Use;
            InputManager.Actions.DropItem.performed += Drop;
            InputManager.Actions.ThrowItem.performed += Throw;
        }

        protected virtual void RemoveListeners()
        {
            InputManager.Actions.Attack.performed -= Use;
            InputManager.Actions.DropItem.performed -= Drop;
            InputManager.Actions.ThrowItem.performed -= Throw;
        }

        private void OnTriggerStay(Collider other)
        {
            if (_canBePickedUp && other.CompareTag(Tags.Player))
            {
                var pv = other.GetComponent<PhotonView>();
                if (pv != null && pv.IsMine)
                {
                    other.GetComponent<PlayerController>().OnPickupTriggerStay(this);
                }
            }
        }

        [PunRPC]
        public void DisableRigidbody()
        {
            rigidbody.detectCollisions = false;
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
            SetOutline(false);
        }

        [PunRPC]
        public void EnableRigidBody()
        {
            rigidbody.detectCollisions = true;
            rigidbody.useGravity = true;
            rigidbody.isKinematic = false;
            SetOutline(true);
        }

        [PunRPC]
        public void PickUpRPC(bool pickedUp)
        {
            _canBePickedUp = !pickedUp;
        }

        private void TransferOwnership()
        {
            if (!photonView.IsMine)
            {
                photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
            }
        }

        private void SetOutline(bool isEnabled)
        {
            var outline = GetComponent<Outline>();
            if (!outline)
                return;

            outline.enabled = isEnabled;
        }
    }
}
