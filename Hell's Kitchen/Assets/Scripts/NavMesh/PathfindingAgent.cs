using UnityEngine;
using UnityEngine.Events;

public class PathfindingAgent : MonoBehaviour {

    [Header("Parameters")]
    [SerializeField]
    private float maxVelocity = 2.0f;
    
    [SerializeField]
    private float maxAcceleration = 10.0f;

    [SerializeField] 
    public UnityEvent onArrive;

    private Pathfinding.PathNode _path;
    private Vector3 _target;
    private Vector3 _velocity;
    private bool _onArriveInvoked;

    public Vector3 Target {
        get => _target;
        set {
            _target = value;
            RecalculatePath();
        }
    }

    private void Update() {
        // Arrived
        if (_path == null) {
            if (!_onArriveInvoked) {
                onArrive?.Invoke();
                _onArriveInvoked = true;
            }
            return;
        }
        
        // Get next point along path
        var nextPoint = _path.Position;
        
        // Compute desired velocity
        var desiredVelocity = nextPoint - transform.position;
        if (_path.Next != null || desiredVelocity.magnitude > maxVelocity)
            desiredVelocity = desiredVelocity.normalized * maxVelocity;
        
        // Compute acceleration
        var acceleration = desiredVelocity - _velocity;
        if (acceleration.magnitude > maxAcceleration)
            acceleration = acceleration.normalized * maxAcceleration;
        
        // Apply acceleration to current velocity
        _velocity += Time.deltaTime * acceleration;
        transform.position += Time.deltaTime * _velocity;
    }
    
    private void RecalculatePath() {
        if (Pathfinding.Instance != null) {
            _path = Pathfinding.Instance.FindPath(transform.position, Target);
            _onArriveInvoked = false;
        }
    }
    
}
