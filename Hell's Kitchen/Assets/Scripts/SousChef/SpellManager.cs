using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    [SerializeField] GameObject photonSpellPrefab;
    public void HealerSpell_Photon(GameObject target) {
        var obj = Instantiate(photonSpellPrefab, target.transform.position, Quaternion.identity);
        obj.GetComponent<PhotonSpell>().target = target;

    }
}
