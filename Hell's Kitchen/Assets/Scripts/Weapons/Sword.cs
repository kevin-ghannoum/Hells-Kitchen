using System;
using Common.Interfaces;
using Input;
using Player;
using UnityEngine;

namespace Weapons
{
    public class Sword : MonoBehaviour
    {
        public float Damage = 10f;

        [SerializeField] private Vector3 position = Vector3.zero;
        [SerializeField] private Quaternion rotation = Quaternion.identity;

        private InputManager _input => InputManager.Instance;

        private void Start()
        {
            OnEquip();
        }


        public void OnEquip()
        {
            Transform hand = GameObject.FindObjectOfType<PlayerController>().CharacterHand;
            if (!hand)
                return;
            
            gameObject.SetActive(true);
            this.transform.parent.transform.parent = hand;
            this.transform.parent.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            //this.transform.parent.transform.localPosition = position;
            //this.transform.rotation = rotation;
        }
    
        public void OnUnequip()
        {
            gameObject.SetActive(true);
        }
    
        private void OnCollisionEnter(Collision collision)
        {
            var obj = collision.gameObject;
            if (_input.attack && obj.TryGetComponent(out IKillable killable))
            {
                killable.TakeDamage(Damage);
            }
        }
    }
}

