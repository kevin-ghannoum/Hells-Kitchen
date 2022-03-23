using UnityEngine;

public static class Utils {
    
    public static bool CheckPointInTriangle(Vector3[] triangle, Vector3 point) {
        if (triangle.Length != 3)
            return false;

        Vector3 d = triangle[1] - triangle[0];
        Vector3 e = triangle[2] - triangle[0];
 
        if (Mathf.Approximately(e.z, 0)) {
            e.z = 0.0001f;
        }
     
        double w1 = (e.x * (triangle[0].z - point.z) + e.z * (point.x - triangle[0].x)) / (d.x * e.z - d.z * e.x);
        double w2 = (point.z - triangle[0].z - w1 * d.z) / e.z;
        
        return (w1 >= 0.0f) && (w2 >= 0.0f) && ((w1 + w2) <= 1.0f);
    }

    public static float SignedAngle(Vector3 first, Vector3 second) {
        return Vector2.SignedAngle(new Vector2(first.x, first.z), new Vector2(second.x, second.z));
    }

}
