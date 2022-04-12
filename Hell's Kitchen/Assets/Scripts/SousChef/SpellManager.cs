using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    [SerializeField] GameObject photonSpellPrefab;

    public void HealerSpell_Photon(Transform target) {
        var obj = Instantiate(photonSpellPrefab, target.position, Quaternion.identity);
        obj.GetComponent<PhotonSpell>().target = target;

    }
}
