using Photon.Pun;
using UnityEngine;

public class KnightStateManager : MonoBehaviour
{
    public KnightBaseState currentState;
    public KnightAttackState attackState = new KnightAttackState();
    public KnightMoveToTargetState moveToTarget = new KnightMoveToTargetState();
    public KnightFollowState followState = new KnightFollowState();
    public KnightLootState lootState = new KnightLootState();
    public Animator animator;
    public GameObject weapon;
    public SousChef sc { get; set; }
    public SpellManager spells { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        spells = gameObject.GetComponent<SpellManager>();
        sc = gameObject.GetComponent<SousChef>();
        currentState = followState;
        currentState.EnterState(this);
        transform.position = sc.player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        sc.player = sc.FindClosestPlayer();
        if (!GetComponent<PhotonView>().IsMine)
            return;
            
        animator.SetBool("isWalking", sc.agent.IsMoving);
        animator.SetBool("isRunning", sc.agent.IsMoving);
        currentState.UpdateState(this);
    }

     public void SwitchState(KnightBaseState state)
     {
        currentState = state;
        state.EnterState(this);
    }

    public bool IsInLootState()
    {
        return currentState == lootState;
    }
}
