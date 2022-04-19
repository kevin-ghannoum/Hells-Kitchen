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
        
        private InputManager input => InputManager.Instance;
        protected PlayerController playerController;
        protected Animator playerAnimator;
        private bool _canBePickedUp = true;

        private float _damage = 10.0f;

        private PhotonView _photonView;

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
            _photonView = GetComponent<PhotonView>();
        }

        public virtual void PickUp()
        {
            GetComponentsFromLocalPlayer();
            var viewId = NetworkHelper.GetLocalPlayerObject().GetComponent<PhotonView>().ViewID;
            
            _canBePickedUp = false;
            GameStateData.carriedWeapon = WeaponInstance;
            _photonView.RPC(nameof(ReparentObjectToPlayerHand), RpcTarget.All, viewId);
           // ReparentObjectToPlayerHand(viewId);
            _photonView.RPC(nameof(DisableRigidbody), RpcTarget.All);
            AddListeners();
        }

        public void Drop(InputAction.CallbackContext callbackContext)
        {
            RemoveFromPlayer();
        }

        public void RemoveFromPlayer()
        {
            _canBePickedUp = true;
            transform.SetParent(null);
            transform.localScale = new Vector3(1, 1, 1);
            RemoveListeners();
            GameStateData.carriedWeapon = WeaponInstance.None;
            _photonView.RPC(nameof(EnableRigidBody), RpcTarget.All);
        }

        public void Throw(InputAction.CallbackContext callbackContext)
        {
            _canBePickedUp = true;
            transform.SetParent(null);
            transform.localScale = new Vector3(1, 1, 1);
            transform.position = playerController.DamagePosition.position;
            transform.rotation = playerController.transform.rotation;
            rigidbody.velocity = playerController.transform.forward * throwSpeed;
            rigidbody.angularVelocity = new Vector3(
                Random.Range(-throwAngularSpeed, throwAngularSpeed), 
                Random.Range(-throwAngularSpeed, throwAngularSpeed), 
                Random.Range(-throwAngularSpeed, throwAngularSpeed)
            );
            RemoveListeners();
            GameStateData.carriedWeapon = WeaponInstance.None;
            _photonView.RPC(nameof(EnableRigidBody), RpcTarget.All);
        }

        private void GetComponentsFromLocalPlayer()
        {
            GameObject playerObject = NetworkHelper.GetLocalPlayerObject();
            playerController = playerObject.GetComponent<PlayerController>();
            playerAnimator = playerObject.GetComponentInChildren<Animator>();
        }

        [PunRPC]
        private void ReparentObjectToPlayerHand(int viewID)
        {
            playerController ??= PhotonView.Find(viewID).gameObject.GetComponent<PlayerController>();
            Transform hand = playerController?.CharacterHand;
            if (!hand)
                throw new MissingReferenceException();


            gameObject.transform.SetParent(hand, false);
            gameObject.transform.localScale = new Vector3(scale, scale, scale);
            gameObject.transform.localPosition = position;
            gameObject.transform.localRotation = rotation;
        }

        public virtual void Use(InputAction.CallbackContext callbackContext)
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            Plane floor = new Plane(Vector3.up, 0);
            floor.Raycast(mouseRay, out float dist);
            playerController.FaceTarget(mouseRay.GetPoint(dist));
        }

        protected virtual void AddListeners()
        {
            if (!input)
                throw new MissingReferenceException("Input Not Found");

            input.reference.actions["Attack"].performed += Use;
            input.reference.actions["DropItem"].performed += Drop;
            input.reference.actions["ThrowItem"].performed += Throw;
        }
        
        protected virtual void RemoveListeners()
        {
            input.reference.actions["Attack"].performed -= Use;
            input.reference.actions["DropItem"].performed -= Drop;
            input.reference.actions["ThrowItem"].performed -= Throw;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_photonView.IsMine)
                return;
            
            if (other.CompareTag(Tags.Player) && _canBePickedUp && !GameStateData.IsCarryingWeapon)
            {
                var pv = other.GetComponent<PhotonView>();
                if (pv != null && pv.IsMine)
                {
                    other.GetComponent<PlayerController>().OnPickupTriggerEnter(this);
                }
                
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (!_photonView.IsMine)
                return;

            if (other.CompareTag(Tags.Player))
            {
                var pv = other.GetComponent<PhotonView>();
                if (pv != null && pv.IsMine)
                {
                    other.GetComponent<PlayerController>().OnPickupTriggerExit(this);
                }
            }
        }

        [PunRPC]
        public void DisableRigidbody()
        {
            Debug.Log("DISABLE");
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
            Debug.Log("ENABLE");
            rigidbody.detectCollisions = true;
            rigidbody.useGravity = true;
            rigidbody.isKinematic = false;
            SetOutline(true);
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
