using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerLootState : HealerBaseState
{
    public override void EnterState(HealerStateManager healer)
    {
        Debug.Log("@Loot state");
    }

    public override void UpdateState(HealerStateManager healer)
    {
        throw new System.NotImplementedException();
    }
}
