using UnityEngine;
using UnityEngine.Events;
using Weapons;

public class AnimationEventIntermediate : MonoBehaviour
{
    public UnityEvent shootGun = new UnityEvent();

    public void FireGun()
    {
        shootGun.Invoke();
    }
}
