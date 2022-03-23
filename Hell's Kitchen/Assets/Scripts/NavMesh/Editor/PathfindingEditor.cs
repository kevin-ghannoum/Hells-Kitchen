using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Pathfinding))]
public class PathfindingEditor : Editor {
    
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        
        var script = (Pathfinding) target;
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Bake")) {
            script.Bake();
        }
        if (GUILayout.Button("Randomize Path")) {
            script.RandomPath();
        }
        GUILayout.EndHorizontal();
    }
    
}
