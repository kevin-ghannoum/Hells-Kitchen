#pragma warning disable 659

using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

[RequireComponent(typeof(NavMeshSurface))]
public class Pathfinding : MonoBehaviour
{

    /**
     * Singleton instance of pathfinding script.
     */
    public static Pathfinding Instance;

    /**
     * Represents a single triangle of the Nav Mesh, with its 3 vertices and adjacent triangles.
     */
    private class PolygonNode
    {
        public Vector3[] Vertices;
        public Vector3 Center;
        public PolygonNode[] Successors;

        public void SetVertices(Vector3[] vertices)
        {
            Center = vertices.Aggregate(Vector3.zero, (vertex, acc) => acc + vertex) / vertices.Length;
            Vertices = vertices;
        }

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            if (other.GetType() != typeof(PolygonNode))
                return false;

            PolygonNode node = (PolygonNode) other;
            return Vertices.SequenceEqual(node.Vertices);
        }
    }

    /**
     * Used for A* pathfinding from a start polygon to an end polygon.
     */
    private class PolygonPathNode
    {
        public readonly PolygonNode Node;
        public PolygonPathNode Next;
        public PolygonPathNode Prev;

        public float F, G, H;

        public PolygonPathNode(PolygonNode node, PolygonPathNode prev = null)
        {
            Node = node;
            Prev = prev;
        }

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            if (other.GetType() != typeof(PolygonPathNode))
                return false;

            PolygonPathNode otherNode = (PolygonPathNode) other;
            return Node.Equals(otherNode.Node);
        }
    }

    /**
     * Used to represent a full line path from start to end.
     */
    public class PathNode
    {
        public Vector3 Position;
        public PathNode Prev;
        public PathNode Next;
    }

    [Header("Debug")]
    [SerializeField]
    private bool showActivePath = true;

    [SerializeField]
    private bool showNavMesh = true;
    
    [SerializeField]
    private Vector3 startPosition;

    [SerializeField]
    private Vector3 endPosition;
    
    [Header("Parameters")]
    [SerializeField]
    private bool buildAtRuntime = true;

    [Header("References")]
    [SerializeField]
    private NavMeshSurface navMeshSurface;

    private Mesh _mesh;
    private PolygonNode[] _nodes;

    private PathNode _activePath;
    private PathNode _activePathEnd;

    private PolygonPathNode _activePolyPath;
    private PolygonPathNode _activePolyPathEnd;
    
    #region Unity Events

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        Bake(true);
    }

    private void Reset()
    {
        navMeshSurface = GetComponent<NavMeshSurface>();
        Bake(true);
    }

    private void OnDrawGizmosSelected()
    {
        // Draw nav mesh
        if (_mesh != null && showNavMesh)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireMesh(_mesh);
        }

        // Draw current active path
        if (_activePath != null && _activePathEnd != null && _activePolyPath != null && _activePolyPathEnd != null && showActivePath)
        {
            // Path
            PathNode currentNode = _activePath;
            Gizmos.color = Color.blue;
            while (currentNode.Next != null)
            {
                Gizmos.DrawLine(currentNode.Position, currentNode.Next.Position);
                currentNode = currentNode.Next;
            }

            // Start
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_activePath.Position, 1.0f);

            // End
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_activePathEnd.Position, 1.0f);
            
            // Start poly
            Gizmos.color = Color.green;
            for (int i = 0; i < _activePolyPath.Node.Vertices.Length; i++)
            {
                Gizmos.DrawLine(_activePolyPath.Node.Vertices[i], _activePolyPath.Node.Vertices[(i + 1) % _activePolyPath.Node.Vertices.Length]);
            }

            // End poly
            Gizmos.color = Color.red;
            for (int i = 0; i < _activePolyPathEnd.Node.Vertices.Length; i++)
            {
                Gizmos.DrawLine(_activePolyPathEnd.Node.Vertices[i], _activePolyPathEnd.Node.Vertices[(i + 1) % _activePolyPathEnd.Node.Vertices.Length]);
            }
            
            // Portals
            Gizmos.color = Color.yellow;
            var portals = BuildPortalList(_activePolyPath, _activePathEnd.Position);
            for (int i = 0; i < portals.Length; i++)
            {
                Gizmos.DrawLine(portals[i][0], portals[i][1]);
            }
        }
    }

    private void OnValidate()
    {
        if (_nodes != null)
        {
            _activePath = FindPath(startPosition, endPosition);
            if (_activePath != null)
            {
                _activePathEnd = _activePath;
                while (_activePathEnd.Next != null)
                    _activePathEnd = _activePathEnd.Next;
            }
        }
    }

    #endregion

    #region Public Methods

    public void RandomPath()
    {
        // Reset active path
        _activePath = null;
        _activePathEnd = null;

        var position = transform.position;
        while (_activePath == null)
        {
            // Random start and end
            do
            {
                startPosition = new Vector3(
                    Random.Range(-_mesh.bounds.size.x / 2.0f, _mesh.bounds.size.x / 2.0f),
                    0,
                    Random.Range(-_mesh.bounds.size.z / 2.0f, _mesh.bounds.size.z / 2.0f)
                ) + position;
                endPosition = new Vector3(
                    Random.Range(-_mesh.bounds.size.x / 2.0f, _mesh.bounds.size.x / 2.0f),
                    0,
                    Random.Range(-_mesh.bounds.size.z / 2.0f, _mesh.bounds.size.z / 2.0f)
                ) + position;
            } while (Vector3.Distance(startPosition, endPosition) < 10.0f);

            // Compute path
            PolygonPathNode polyPath = FindPolygonPath(startPosition, endPosition);
            if (polyPath == null)
                continue;

            // Compute string pull
            _activePath = StringPull(polyPath, startPosition, endPosition);
            if (_activePath == null)
                continue;

            // Set end node to path
            _activePathEnd = _activePath;
            while (_activePathEnd.Next != null)
                _activePathEnd = _activePathEnd.Next;
        }
    }

    public void Bake(bool force = false)
    {
        if (_nodes != null && !force)
            return;

        if ((Application.isEditor && !EditorApplication.isPlaying) || buildAtRuntime)
        {
            navMeshSurface.BuildNavMesh();
        }
        UpdateNavMesh();
    }
    
    public void UpdateNavMesh()
    {
        var triangulation = NavMesh.CalculateTriangulation();
        var vertices = triangulation.vertices;
        var normals = vertices.Select(v => Vector3.up).ToArray();
        var indices = triangulation.indices;
        _mesh = new Mesh {
            vertices = vertices,
            normals = normals,
            triangles = indices
        };

        _nodes = new PolygonNode[indices.Length / 3];
        for (var i = 0; i < _nodes.Length; i++)
        {
            _nodes[i] = new PolygonNode();
            _nodes[i].SetVertices(new[] {
                vertices[indices[i * 3]],
                vertices[indices[i * 3 + 1]],
                vertices[indices[i * 3 + 2]]
            });
        }

        foreach (var node in _nodes)
        {
            node.Successors = GenerateSuccessors(node);
        }
    }


    public PathNode FindPath(Vector3 start, Vector3 end, float navMeshHitRadius = 3.0f)
    {
        // Snap start and end to navmesh
        if (!NavMesh.SamplePosition(start, out var startPos, navMeshHitRadius, NavMesh.AllAreas) ||
            !NavMesh.SamplePosition(end, out var endPos, navMeshHitRadius, NavMesh.AllAreas))
            return null;

        // Set new start and end positions
        start = startPos.position;
        end = endPos.position;

        // Find polygon path to target
        _activePolyPath = FindPolygonPath(start, end);
        if (_activePolyPath == null)
            return null;

        _activePolyPathEnd = _activePolyPath;
        while (_activePolyPathEnd.Next != null)
            _activePolyPathEnd = _activePolyPathEnd.Next;

        // Apply string pulling to get line path
        _activePath = StringPull(_activePolyPath, start, end);
        if (_activePath == null)
            return null;
        
        _activePathEnd = _activePath;
        while (_activePathEnd.Next != null)
            _activePathEnd = _activePathEnd.Next;

        return _activePath;
    }

    #endregion

    #region Private Methods

    private PolygonNode[] GenerateSuccessors(PolygonNode node)
    {
        return _nodes.Where(otherNode => !node.Equals(otherNode))
            .Where(otherNode => otherNode.Vertices
                .Where(v => !node.Vertices.Contains(v)).ToArray().Length == 1)
            .ToArray();
    }

    private PolygonNode FindClosestNode(Vector3 position)
    {
        // Find which node the position is inside
        var polyNode = _nodes.FirstOrDefault(n => Utils.CheckPointInTriangle(n.Vertices, position));
        if (polyNode != null)
            return polyNode;
        
        // Find the closest edge and return its node
        PolygonNode closestNode = null;
        float closestDist = float.MaxValue;
        foreach (var node in _nodes)
        {
            for (int i = 0; i < node.Vertices.Length; i++)
            {
                var pointOnLine = Utils.GetClosestPointOnLine(node.Vertices[i], node.Vertices[(i + 1) % node.Vertices.Length], position);
                float dist = Vector3.Distance(pointOnLine, position);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closestNode = node;
                    break;
                }
            }
        }

        return closestNode;
    }

    private PolygonPathNode FindPolygonPath(Vector3 start, Vector3 end)
    {
        List<PolygonPathNode> openList = new List<PolygonPathNode>();
        List<PolygonPathNode> closedList = new List<PolygonPathNode>();

        // Get start and end nodes
        PolygonNode startNode = FindClosestNode(start);
        PolygonNode endNode = FindClosestNode(end);
        if (startNode == null || endNode == null)
            return null;

        // No need to path find, we're on the same polygon
        if (startNode.Equals(endNode))
        {
            return new PolygonPathNode(startNode);
        }

        // Add start node to open list
        openList.Add(new PolygonPathNode(startNode));

        PolygonPathNode path = null;

        while (openList.Count > 0)
        {

            // Pop lowest f node
            PolygonPathNode q = openList[0];
            openList.RemoveAt(0);

            // Generate successors
            IEnumerable<PolygonPathNode> successors = q.Node.Successors.Select(s => new PolygonPathNode(s, q));
            foreach (var s in successors)
            {

                // Compute g, h, f
                s.G = q.G + Vector3.Distance(q.Node.Center, s.Node.Center);
                s.H = Vector3.Distance(s.Node.Center, endNode.Center);
                s.F = s.G + s.H;

                // If s is the goal node, we've found the shortest path
                if (s.Node.Equals(endNode))
                {
                    path = s;
                    goto end;
                }

                // If s is in open node list with lower f, skip
                PolygonPathNode openNode = openList.FirstOrDefault(n => n.Node.Equals(s.Node) && n.F <= s.F);
                if (openNode != null)
                    continue;

                // If s is in closed node list, skip
                // Otherwise, add it to open node list
                PolygonPathNode closedNode = closedList.FirstOrDefault(n => n.Node.Equals(s.Node) && n.F < s.F);
                if (closedNode == null)
                    openList.Add(s);

            }

            // Add node to closed list
            closedList.Add(q);

            // Sort open list by f value
            openList.Sort((a, b) => Mathf.CeilToInt(a.F - b.F));
        }

        end:
        // Reconstruct path forwards
        if (path != null)
        {
            float g = path.G;
            while (path.Prev != null)
            {
                path.Prev.Next = path;
                path = path.Prev;
            }
            path.G = g;
        }

        // Return path if found, null otherwise
        return path;
    }

    private Vector3[] GetNextCommonEdge(PolygonPathNode node)
    {
        if (node?.Next == null)
            return null;

        return node.Node.Vertices.Where(v => node.Next.Node.Vertices.Contains(v)).ToArray();
    }

    private Vector3[][] BuildPortalList(PolygonPathNode node, Vector3 end)
    {
        List<Vector3[]> portals = new List<Vector3[]>();

        while (node.Next != null)
        {
            Vector3[] nextPortals = GetNextCommonEdge(node);
            Vector3 dir = node.Next.Node.Center - node.Node.Center;
            if (Utils.SignedAngle(dir, nextPortals[0] - node.Node.Center) < 0)
            {
                var temp = nextPortals[0];
                nextPortals[0] = nextPortals[1];
                nextPortals[1] = temp;
            }
            portals.Add(nextPortals);
            node = node.Next;
        }

        portals.Add(new[] {end, end});

        return portals.ToArray();
    }

    private PathNode StringPull(PolygonPathNode path, Vector3 start, Vector3 end)
    {
        Vector3[][] portals = BuildPortalList(path, end);

        Vector3 portalApex = start;
        Vector3 portalLeft = portals[0][0];
        Vector3 portalRight = start;
        int apexIndex = 0, leftIndex = 0, rightIndex = 0;
        
        PathNode current = new PathNode() {
            Position = portalApex
        };

        // Loop over every new portal
        for (var i = 0; i < portals.Length; i++)
        {
            Vector3[] portal = portals[i];
            Vector3 left = portal[0];
            Vector3 right = portal[1];

            // Update right vertex
            // Check if point is inside funnel
            if (portalApex == portalRight || Utils.SignedAngle(right - portalApex, portalRight - portalApex) <= 0)
            {

                // Attempt to tighten the funnel
                if (Utils.SignedAngle(portalLeft - portalApex, right - portalApex) <= 0)
                {
                    portalRight = right;
                    rightIndex = i;
                }
                else
                {
                    // Cant tighten funnel, recalculate new apex and funnel
                    // Right over left, insert left to path and restart scan from portal left point
                    current = new PathNode {
                        Position = portalLeft,
                        Prev = current
                    };

                    // Make current left the new apex
                    portalApex = portalLeft;
                    apexIndex = leftIndex;

                    // Reset portal
                    portalLeft = portalApex;
                    portalRight = portalApex;
                    leftIndex = apexIndex;
                    rightIndex = apexIndex;

                    // Restart scan
                    i = apexIndex;
                    continue;
                }
            }

            // Update left vertex
            // Check if point is inside funnel
            if (portalApex == portalLeft || Utils.SignedAngle(portalLeft - portalApex, left - portalApex) <= 0)
            {

                // Attempt to tighten the funnel
                if (Utils.SignedAngle(left - portalApex, portalRight - portalApex) <= 0)
                {
                    portalLeft = left;
                    leftIndex = i;
                }
                else
                {
                    // Cant tighten funnel, recalculate new apex and funnel
                    // Left over right, insert right to path and restart scan from portal right point
                    current = new PathNode {
                        Position = portalRight,
                        Prev = current
                    };

                    // Make current right the new apex
                    portalApex = portalRight;
                    apexIndex = rightIndex;

                    // Reset portal
                    portalLeft = portalApex;
                    portalRight = portalApex;
                    leftIndex = apexIndex;
                    rightIndex = apexIndex;

                    // Restart scan
                    i = apexIndex;
                    continue;
                }
            }
        }

        // Add last point
        current = new PathNode {
            Position = end,
            Prev = current
        };

        // Reconstruct path forwards
        while (current.Prev != null)
        {
            current.Prev.Next = current;
            current = current.Prev;
        }

        return current;
    }
    
    #endregion

}
