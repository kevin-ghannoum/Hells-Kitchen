using UnityEngine;

public static class Utils
{

    public static float SignedAngle(Vector3 first, Vector3 second)
    {
        return Vector2.SignedAngle(new Vector2(first.x, first.z), new Vector2(second.x, second.z));
    }

    public static Vector3 GetClosestPointOnLine(Vector3 start, Vector3 end, Vector3 point)
    {
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

    public static Vector3 GetPointOnLineAtDistance(Vector3 start, Vector3 end, float distance)
    {
        // Get direction and magnitude
        Vector3 dir = (end - start);
        float magnitudeMax = dir.magnitude;
        dir = dir.normalized;

        // Take point at distance
        distance = Mathf.Clamp(distance, 0.0f, magnitudeMax);
        return start + dir * distance;
    }

    public static bool CheckPointInTriangle(Vector3[] triangle, Vector3 p)
    {
        Vector3 a = triangle[0], b = triangle[1], c = triangle[2];
        if (SameSide(p, a, b, c) && SameSide(p, b, a, c) && SameSide(p, c, a, b))
        {
            Vector3 vc1 = Vector3.Cross(a - b, a - c);
            if (Mathf.Abs(Vector3.Dot((a - p).normalized, vc1.normalized)) <= .01f)
                return true;
        }

        return false;
    }

    private static bool SameSide(Vector3 p1, Vector3 p2, Vector3 a, Vector3 b)
    {
        Vector3 cp1 = Vector3.Cross(b - a, p1 - a);
        Vector3 cp2 = Vector3.Cross(b - a, p2 - a);
        return Vector3.Dot(cp1, cp2) >= 0;
    }

}