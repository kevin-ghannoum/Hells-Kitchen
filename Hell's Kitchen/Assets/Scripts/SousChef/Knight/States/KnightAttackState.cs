using UnityEngine;

public class KnightAttackState : KnightBaseState
{
    [SerializeField] private GameObject Slash;
    float attackTime = 1f;
    float _attackTime = 0f;
    float attackAnimationTime = 1f;
    float _attackAnimationTime = 0f;
    float skillTime = 10f;
    float _skillTime = 0f;
    float skillAnimationTime = 1f;
    float _skillAnimationTime = 0f;
    bool isAttacking = false;
    bool isUsingSkill = false;

    public override void EnterState(KnightStateManager knight)
    {
        Debug.Log("@Attack state");
        attackTime = 1f;
        attackTime = 1f;
        attackAnimationTime = 1f;
        _attackAnimationTime = 0;
        skillTime = 10f;
        _skillTime = 10f;
        skillAnimationTime = 1f;
        _skillAnimationTime = 0f;
        isAttacking = false;
        isUsingSkill = false;
    }

    public override void UpdateState(KnightStateManager knight)
    {
        if(knight.sc.targetEnemy == null)
        {
            knight.sc.agent.standStill = false;
            knight.SwitchState(knight.followState);
        }
        else{
            knight.sc.faceTargetEnemy();

            if(isUsingSkill)
            {
                //is using kill
                _skillTime += Time.deltaTime;
                _attackTime += Time.deltaTime;
                _skillAnimationTime -= Time.deltaTime;
                knight.sc.agent.standStill = true;

                if(_skillAnimationTime <= 0)
                {
                    //skill ends
                    knight.spells.KnightSkill();
                    isUsingSkill = false;
                    knight.sc.agent.standStill = false;
                    return;
                }
                
                return;
            }

            if(isAttacking)
            {
                //is using basic attack
                _skillTime += Time.deltaTime;
                _attackTime += Time.deltaTime;
                _attackAnimationTime -= Time.deltaTime;
                knight.sc.agent.standStill = true;

                if(_attackAnimationTime <= 0)
                {
                    //basic attack ends
                    isAttacking = false;
                    knight.sc.agent.standStill = false;
                    knight.weapon.GetComponent<Collider>().enabled = false;
                    return;
                }
                
                return;
            }

            if(knight.sc.GetDistanceToEnemy() < knight.sc.attackRange)
            {
                // target enemy within attack range, attack
                if(!isAttacking & !isUsingSkill)
                {
                    if(_skillTime >= skillTime)
                    {
                        //skill available, use skill
                        isUsingSkill = true;
                        _skillTime = 0;
                        knight.animator.SetTrigger("Smash");
                        knight.sc.agent.standStill = true;
                        _skillAnimationTime = skillAnimationTime;
                    }
                    else
                    {
                        //skill not available
                        if(_attackTime >= attackTime)
                        {
                            //basic attack available, use basic attack
                            isAttacking = true;
                            attackTime = 0;
                            knight.animator.SetTrigger("Attack");
                            knight.sc.agent.standStill = true;
                            _attackAnimationTime = attackAnimationTime;
                            knight.weapon.GetComponent<Collider>().enabled = true;
                        }
                        else
                        {
                            //neither basic attack nor skill available, approach
                            knight.sc.agent.standStill = false;
                            knight.sc.agent.Target = knight.sc.targetEnemy.transform.position;
                        }
                    }
                }
            }
            else if(knight.sc.GetDistanceToEnemy() > knight.sc.searchRange)
            {
                //target enemy out of search range, return to follow state
                knight.sc.targetEnemy = null;
                knight.SwitchState(knight.followState);
            }
            else
            {
                //target enemy not in attack range, approach
                knight.sc.agent.standStill = false;
                knight.sc.agent.Target = knight.sc.targetEnemy.transform.position;
            }
        }
    }
}
