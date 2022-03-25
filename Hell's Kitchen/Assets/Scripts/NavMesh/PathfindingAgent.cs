using UnityEngine;
using UnityEngine.Events;

public class PathfindingAgent : MonoBehaviour {

    [Header("Parameters")] 
    [SerializeField]
    private float arrivalRadius = 0.5f;
    
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

    public float ArrivalRadius {
        get => arrivalRadius;
        set {
            arrivalRadius = value;
            RecalculatePath();
        }
    }

    public Vector3 Velocity => _velocity;

    private void Update() {
        // Arrived
        if (_path == null) {
            if (!_onArriveInvoked) {
                onArrive?.Invoke();
                _onArriveInvoked = true;
            }
            _velocity = Vector3.zero;
            return;
        }

        // Get next point along path
        var nextPoint = _path.Position;
        
        // Compute desired velocity
        var desiredVelocity = nextPoint - transform.position;
        if (_path?.Next != null || desiredVelocity.magnitude > maxVelocity)
            desiredVelocity = desiredVelocity.normalized * maxVelocity;
        
        // Compute acceleration
        var acceleration = desiredVelocity - _velocity;
        if (acceleration.magnitude > maxAcceleration)
            acceleration = acceleration.normalized * maxAcceleration;
        
        // Apply acceleration to current velocity
        _onArriveInvoked = false;
        _velocity += Time.deltaTime * acceleration;
        transform.position += Time.deltaTime * _velocity;
        transform.rotation = Quaternion.LookRotation(_velocity);
        
        // Reached next point
        if (Vector3.Distance(transform.position, _path.Position) < arrivalRadius)
            _path = _path.Next;
    }
    
    private void RecalculatePath() {
        if (Pathfinding.Instance != null) {
            _path = Pathfinding.Instance.FindPath(transform.position, Target);
        }
    }
    
}
