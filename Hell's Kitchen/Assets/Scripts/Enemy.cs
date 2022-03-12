using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

[RequireComponent(typeof(PathfindingAgent))]
public class Enemy : MonoBehaviour {

    [Header("References")] 
    [SerializeField]
    private PathfindingAgent agent;

    private void Start() {
        NewTarget();
    }

    private void Reset() {
        agent = GetComponent<PathfindingAgent>();
        
        if (agent.onArrive == null)
            agent.onArrive = new UnityEvent();
        
        agent.onArrive.RemoveListener(OnArrive);
        agent.onArrive.AddListener(OnArrive);
    }

    private void OnArrive() {
        NewTarget();
    }

    private void NewTarget() {
        agent.Target = new Vector3(
            Random.Range(-20.0f, 20.0f),
            0.0f,
            Random.Range(-20.0f, 20.0f)
        );
    }

}
