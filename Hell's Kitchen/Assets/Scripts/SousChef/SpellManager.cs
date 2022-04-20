using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    [SerializeField] GameObject photonSpellPrefab;
    [SerializeField] GameObject healSpellPrefab;
    [SerializeField] GameObject knightSkill;

    private PhotonView _photonView;

    private void Start()
    {
        _photonView = GetComponent<PhotonView>();
    }

    public void HealerSpell_Photon(GameObject target)
    {
        _photonView.RPC(nameof(HealerSpell_Photon_RPC), RpcTarget.All, target);
    }
    
    [PunRPC]
    public void HealerSpell_Photon_RPC(GameObject target)
    {
        var obj = Instantiate(photonSpellPrefab, target.transform.position, Quaternion.identity);
        obj.GetComponent<PhotonSpell>().target = target;
    }

    internal void HealerSpell_Heal(Vector3 position)
    {
        _photonView.RPC(nameof(HealerSpell_Heal_RPC), RpcTarget.All, position);
    }
    
    [PunRPC]
    internal void HealerSpell_Heal_RPC(Vector3 position)
    {
        Instantiate(healSpellPrefab, position, Quaternion.identity);
    }

    public void KnightSkill()
    {
        _photonView.RPC(nameof(KnightSkill_RPC), RpcTarget.All);
    }
    
    [PunRPC]
    public void KnightSkill_RPC()
    {
        GameObject slash = Instantiate(knightSkill, transform.position + Vector3.up, Quaternion.LookRotation(transform.forward));
        slash.GetComponent<Slash>().rotation = transform.forward;
    }
}
