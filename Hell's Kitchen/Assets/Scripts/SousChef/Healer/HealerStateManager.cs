using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerStateManager : MonoBehaviour
{
    HealerBaseState currentState;
    public HealerAttackState attackState = new HealerAttackState();
    public HealerMoveToTargetState moveToTarget = new HealerMoveToTargetState();
    public HealerFollowState followState = new HealerFollowState();
    public HealerLootState lootState = new HealerLootState();
    public HealerHealState healState = new HealerHealState();

    public SousChef sc { get; set; }
    public SpellManager spells { get; set; }

    private void Start()
    {
        spells = gameObject.GetComponent<SpellManager>();
        sc = gameObject.GetComponent<SousChef>();
        currentState = followState;
    }

    private void Update()
    {
        currentState.UpdateState(this);
        // transform.position = new Vector3(transform.position.x, 0, transform.position.z);

    }

    public void SwitchState(HealerBaseState state) {
        currentState = state;
        state.EnterState(this);
    }
}
