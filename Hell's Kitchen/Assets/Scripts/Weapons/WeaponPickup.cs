﻿using System;
using Common;
using Common.Enums;
using Common.Interfaces;
using Player;
using UnityEngine;
using Input;
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

        [Header("Damage")]
        public float damage = 10.0f;

        [Header("Throwing")]
        [SerializeField] private float throwSpeed = 35.0f;
        [SerializeField] private float throwAngularSpeed = 180.0f;
        
        [Header("References")]
        [SerializeField]
        private new Rigidbody rigidbody;
        
        private InputManager input => InputManager.Instance;
        protected PlayerController player;
        protected Animator playerAnimator;
        private bool _canBePickedUp = true;

        public bool CanPickUp => _canBePickedUp;

        public void Reset()
        {
            rigidbody = GetComponent<Rigidbody>();
        }

        public virtual void Awake()
        {
            GameObject playerObject = GameObject.FindWithTag(Tags.Player);
            player = playerObject.GetComponent<PlayerController>();
            playerAnimator = playerObject.GetComponentInChildren<Animator>();
        }

        public virtual void PickUp()
        {
            _canBePickedUp = false;
            ReparentObject();
            AddListeners();
            GameStateManager.Instance.carriedWeapon = gameObject;
            DisableRigidbody();
        }

        public void Drop(InputAction.CallbackContext callbackContext)
        {
            _canBePickedUp = true;
            transform.SetParent(null);
            transform.localScale = new Vector3(1, 1, 1);
            RemoveListeners();
            GameStateManager.Instance.carriedWeapon = null;
            EnableRigidBody();
        }

        public void Throw(InputAction.CallbackContext callbackContext)
        {
            _canBePickedUp = true;
            transform.SetParent(null);
            transform.localScale = new Vector3(1, 1, 1);
            transform.position = player.DamagePosition.position;
            transform.rotation = player.transform.rotation;
            rigidbody.velocity = player.transform.forward * throwSpeed;
            rigidbody.angularVelocity = new Vector3(
                Random.Range(-throwAngularSpeed, throwAngularSpeed), 
                Random.Range(-throwAngularSpeed, throwAngularSpeed), 
                Random.Range(-throwAngularSpeed, throwAngularSpeed)
            );
            RemoveListeners();
            GameStateManager.Instance.carriedWeapon = null;
            EnableRigidBody();
        }

        private void ReparentObject()
        {
            Transform hand = player.GetComponent<PlayerController>().CharacterHand;
            if (!hand)
                return;

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
            player.FaceTarget(mouseRay.GetPoint(dist));
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
            if (other.CompareTag(Tags.Player) && _canBePickedUp && !GameStateManager.Instance.IsCarryingWeapon)
            {
                other.GetComponent<PlayerController>().OnPickupTriggerEnter(this);
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(Tags.Player))
            {
                other.GetComponent<PlayerController>().OnPickupTriggerExit(this);
            }
        }

        private void DisableRigidbody()
        {
            rigidbody.detectCollisions = false;
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }

        private void EnableRigidBody()
        {
            rigidbody.detectCollisions = true;
            rigidbody.useGravity = true;
            rigidbody.isKinematic = false;
        }

    }
}