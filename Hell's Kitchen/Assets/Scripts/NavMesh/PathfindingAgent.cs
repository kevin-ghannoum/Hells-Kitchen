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

    private Pathfinding.PathNode _lastPath;
    private Pathfinding.PathNode _path;
    private Vector3 _target;
    private Vector3 _velocity;
    private bool _onArriveInvoked;

    public Vector3 Target {
        get => _target;
        set {
            if (_target != value) {
                _target = value;
                RecalculatePath();
            }
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
        var closestPointOnPath = Utils.GetClosestPointOnLine(_lastPath.Position, _path.Position, transform.position);
        var nextPoint = Utils.GetPointOnLineAtDistance(closestPointOnPath, _path.Position, 1.0f);
        Debug.DrawLine(transform.position, nextPoint, Color.red);
        
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
        if (_velocity != Vector3.zero) {
            transform.rotation = Quaternion.LookRotation(_velocity.normalized);
        }

        // Reached next point
        if (Vector3.Distance(transform.position, _path.Position) < arrivalRadius) {
            _lastPath = _path;
            _path = _path.Next;
        }
    }
    
    private void RecalculatePath() {
        if (Pathfinding.Instance != null) {
            _lastPath = Pathfinding.Instance.FindPath(transform.position, Target);
            _path = _lastPath.Next;
        }
    }
    
}
