using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Pathfinding))]
public class PathfindingEditor : Editor
{
    
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var script = (Pathfinding) target;
        if (GUILayout.Button("Update NavMesh Data"))
        {
            script.UpdateNavMesh();
        }
        
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Bake"))
        {
            script.Bake(true);
        }
        if (GUILayout.Button("Randomize Path"))
        {
            script.RandomPath();
        }
        GUILayout.EndHorizontal();
    }
    
}
