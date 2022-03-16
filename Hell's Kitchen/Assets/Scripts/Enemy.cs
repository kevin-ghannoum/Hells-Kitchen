using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(PathfindingAgent))]
public class Enemy : MonoBehaviour {

    [Header("References")] 
    [SerializeField]
    private PathfindingAgent agent;
    
    [SerializeField] 
    private Animator animator;

    private void Start() {
        NewTarget();
    }

    private void Reset() {
        agent = GetComponent<PathfindingAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update() {
        animator.SetFloat(EnemyAnimator.Speed, agent.Velocity.magnitude);
    }

    private void NewTarget() {
        agent.Target = new Vector3(
            Random.Range(-20.0f, 20.0f),
            0.0f,
            Random.Range(-20.0f, 20.0f)
        );
    }

    public void OnArrive() {
        NewTarget();
    }

}
