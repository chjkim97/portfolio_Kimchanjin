using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BezierCurve
{
    public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0; // (1-t)^3*p0
        p += 3 * uu * t * p1; // 3*t*(1-t)^2*p1
        p += 3 * u * tt * p2; // 3*t^2*(1-t)*p2
        p += ttt * p3;        // t^3*p3

        return p;
    }
}
