using UnityEngine;
using UnityEngine.Events;

public class AnimationEventIntermediate : MonoBehaviour
{
    public UnityEvent shootGun;

    public void FireGun() {
        shootGun ??= new UnityEvent();
        shootGun.Invoke();
    }
}
