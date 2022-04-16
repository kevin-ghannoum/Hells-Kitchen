using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PathfindingAgent : MonoBehaviour
{

    [Header("Parameters")]
    [SerializeField]
    public float ArrivalRadius = 0.5f;

    [SerializeField]
    private float time2target = 0.5f;

    [SerializeField]
    private float maxVelocity = 2.0f;

    [Header("References")]
    [SerializeField]
    private new Rigidbody rigidbody;

    public bool Arrived => _path == null;

    private Pathfinding.PathNode _path;
    private Vector3 _target;

    public bool standStill = false;

    public Vector3 Target
    {
        get => _target;
        set
        {
            if (_target != value)
            {
                _target = value;
                RecalculatePath();
            }
        }
    }

    public Vector3 Velocity => rigidbody.velocity;

    private void Reset()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Arrived
        if (_path == null || standStill)
        {
            rigidbody.velocity = Vector3.zero;
            return;
        }

        // Get next point along path
        // var closestPointOnPath = Utils.GetClosestPointOnLine(_lastPath.Position, _path.Position, transform.position);
        var nextPoint = _path.Position;
        Debug.DrawLine(transform.position, nextPoint, Color.red);

        // Compute desired velocity
        var desiredVelocity = (nextPoint - transform.position) / time2target;
        if (_path.Next != null || desiredVelocity.magnitude > maxVelocity)
        {
            desiredVelocity = desiredVelocity.normalized * maxVelocity;
        }

        // Apply velocity and rotation
        rigidbody.velocity = desiredVelocity;
        if (rigidbody.velocity != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(rigidbody.velocity.normalized);
        }

        // Reached next point
        if (Vector3.Distance(transform.position, _path.Position) < (_path.Next == null ? ArrivalRadius : 0.5f))
        {
            _path = _path.Next;
        }
    }

    private void RecalculatePath()
    {
        if (Pathfinding.Instance != null && Vector3.Distance(transform.position, Target) > ArrivalRadius)
        {
            _path = Pathfinding.Instance.FindPath(transform.position, Target)?.Next;
        }
    }

    public bool IsMoving() => rigidbody.velocity != Vector3.zero;

    public bool PathIsNull() => _path == null;

}