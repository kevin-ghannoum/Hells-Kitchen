using Common.Enums;
using UnityEngine.InputSystem;

namespace Common.Interfaces
{
    public interface IPickup
    {
        public void PickUp();
        public void RemoveFromPlayer();
        public void Drop(InputAction.CallbackContext callbackContext);
        public void Throw(InputAction.CallbackContext callbackContext);
        public float Damage { get; set; }
        public float Price {get;}
        public WeaponInstance WeaponInstance { get; }
    }
}
