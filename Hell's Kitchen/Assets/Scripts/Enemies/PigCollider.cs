using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

public class PigCollider : MonoBehaviour
{
    [SerializeField]
    private EnemyPig pig;

    private void Reset()
    {
        pig = GetComponentInParent<EnemyPig>();
    }

    void OnTriggerEnter(Collider col)
    {
        pig.OnPigTrigger(col);
    }
}
