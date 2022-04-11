using UnityEngine;

public static class Utils {
    
    public static bool CheckPointInTriangle(Vector2[] triangle, Vector2 point) {
        if (triangle.Length != 3)
            return false;

        Vector2 d = triangle[1] - triangle[0];
        Vector2 e = triangle[2] - triangle[0];
 
        if (Mathf.Approximately(e.y, 0)) {
            e.y = 0.0001f;
        }
     
        double w1 = (e.x * (triangle[0].y - point.y) + e.y * (point.x - triangle[0].x)) / (d.x * e.y - d.y * e.x);
        double w2 = (point.y - triangle[0].y - w1 * d.y) / e.y;
        
        return (w1 >= 0.0f) && (w2 >= 0.0f) && ((w1 + w2) <= 1.0f);
    }

    public static float SignedAngle(Vector3 first, Vector3 second) {
        return Vector2.SignedAngle(new Vector2(first.x, first.z), new Vector2(second.x, second.z));
    }

    public static Vector2 MapPointToPlane(Vector3 origin, Vector3 normal, Vector3 xAxis, Vector3 yAxis, Vector3 point) {
        // Normalize normal
        normal = normal.normalized;

        // World space point coords to local coords from origin
        var localPoint = point - origin;
        
        // Dot product of the localPoint vector with normalized plane normal
        var distance = Vector3.Dot(normal, localPoint);
        
        // Invert sign
        distance *= -1;
        
        // Project point on the plane
        var projectedPoint = normal * distance + point;
        var projectedLocalPoint = projectedPoint - origin;
        
        // Dot product with each axis gives x and y coords on the axis
        var x = Vector3.Dot(xAxis, projectedLocalPoint);
        var y = Vector3.Dot(yAxis, projectedLocalPoint);

        return new Vector2(x, y);
    }

    public static bool CheckPointInTriangle(Vector3[] triangle, Vector3 point) {
        if (triangle.Length != 3)
            return false;
        
        // Compute origin, normal, and axes of plane
        var origin = triangle[0];
        var normal = Vector3.Cross(triangle[1] - triangle[0], triangle[2] - triangle[0]).normalized;
        var xAxis = (triangle[1] - triangle[0]).normalized;
        var yAxis = Vector3.Cross(xAxis, normal).normalized;
        
        // Compute point and triangle mappings to plane coords
        var point2D = MapPointToPlane(origin, normal, xAxis, yAxis, point);
        var triangle2D = new Vector2[3];
        triangle2D[0] = MapPointToPlane(origin, normal, xAxis, yAxis, triangle[0]);
        triangle2D[1] = MapPointToPlane(origin, normal, xAxis, yAxis, triangle[1]);
        triangle2D[2] = MapPointToPlane(origin, normal, xAxis, yAxis, triangle[2]);
        
        // Delegate to 2D triangle check
        return CheckPointInTriangle(triangle2D, point2D);
    }

    public static Vector3 GetClosestPointOnLine(Vector3 start, Vector3 end, Vector3 point) {
        // Get direction and magnitude
        Vector3 dir = (end - start);
        float magnitudeMax = dir.magnitude;
        dir = dir.normalized;
        
        // Project point on line
        Vector3 pointDir = point - start;
        float lineProjection = Vector3.Dot(pointDir, dir);
        lineProjection = Mathf.Clamp(lineProjection, 0f, magnitudeMax);
        return start + dir * lineProjection;
    }

    public static Vector3 GetPointOnLineAtDistance(Vector3 start, Vector3 end, float distance) {
        // Get direction and magnitude
        Vector3 dir = (end - start);
        float magnitudeMax = dir.magnitude;
        dir = dir.normalized;
        
        // Take point at distance
        distance = Mathf.Clamp(distance, 0.0f, magnitudeMax);
        return start + dir * distance;
    }

}
