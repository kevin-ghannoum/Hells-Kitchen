using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

public class PigCollider : MonoBehaviour
{
    [SerializeField]
    private PigEnemy pig;

    private void Reset()
    {
        pig = GetComponentInParent<PigEnemy>();
    }

    void OnTriggerEnter(Collider col)
    {
        pig.OnPigTrigger(col);
    }

    void OnTriggerStay(Collider col)
    {
        pig.OnPigChargeTrigger(col);
    }
}
