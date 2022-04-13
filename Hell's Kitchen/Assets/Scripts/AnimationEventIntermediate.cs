using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    public UnityEvent fireGun = new UnityEvent();
    public UnityEvent meleeDamage = new UnityEvent();
    public UnityEvent pickUp = new UnityEvent();

    public void FireGun()
    {
        fireGun.Invoke();
    }

    public void MeleeDamage()
    {
        meleeDamage.Invoke();
    }

    public void PickUp()
    {
        pickUp.Invoke();
    }

}
