using UnityEngine;

public class Utils {

    public static float TriangleArea(Vector3 a, Vector3 b, Vector3 c) {
        Vector3 ab = b - a;
        Vector3 ac = c - a;
        return Vector3.Cross(ab, ac).magnitude / 2.0f;
    } 

    public static bool CheckPointInTriangle(Vector3[] triangle, Vector3 point) {
        if (triangle.Length != 3)
            return false;
        
        // float area = TriangleArea(triangle[0], triangle[1], triangle[2]);
        // float areaPAB = TriangleArea(point, triangle[0], triangle[1]);
        // float areaPBC = TriangleArea(point, triangle[1], triangle[2]);
        // float areaPAC = TriangleArea(point, triangle[0], triangle[2]);
        //
        // return Mathf.Abs(area - (areaPAB + areaPBC + areaPAC)) <= 0.5f;
        
        double w1, w2;
        Vector3 d = triangle[1] - triangle[0];
        Vector3 e = triangle[2] - triangle[0];
 
        if (Mathf.Approximately(e.z, 0)) {
            e.z = 0.0001f;
        }
     
        w1 = (e.x * (triangle[0].z - point.z) + e.z * (point.x - triangle[0].x)) / (d.x * e.z - d.z * e.x);
        w2 = (point.z - triangle[0].z - w1 * d.z) / e.z;
        return (w1 >= 0.0f) && (w2 >= 0.0f) && ((w1 + w2) <= 1.0f);
    }

    public static float SignedAngle(Vector3 first, Vector3 second) {
        return Vector2.SignedAngle(new Vector2(first.x, first.z), new Vector2(second.x, second.z));
    }

}
