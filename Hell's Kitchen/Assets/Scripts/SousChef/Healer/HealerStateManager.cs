using System;
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
    public Animator animator;
    public Transform magicCircle;
    public SousChef sc { get; set; }
    public SpellManager spells { get; set; }

    private void Start()
    {
        animator = GetComponent<Animator>();
        spells = gameObject.GetComponent<SpellManager>();
        sc = gameObject.GetComponent<SousChef>();
        currentState = followState;
        currentState.EnterState(this);
    }

    float attackCooldown = 1f;
    float _attackCooldown = 0f;
    public bool canAttack() => _attackCooldown >= attackCooldown;
    public void resetAttackCD() => _attackCooldown = 0f;
    private void Update()
    {
        _attackCooldown += Time.deltaTime;
        currentState.UpdateState(this);
    }

    public void SwitchState(HealerBaseState state) {
        currentState = state;
        state.EnterState(this);
    }
}
