using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    [SerializeField] GameObject photonSpellPrefab;

    public void HealerSpell_Photon(Vector3 targetPosition) {
        Instantiate(photonSpellPrefab, targetPosition, Quaternion.identity);
    }
}
