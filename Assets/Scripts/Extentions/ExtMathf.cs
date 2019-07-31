using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtMathf
{
    public static float Determinant3x3(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        float x1 = v1.x;
        float x2 = v2.x;
        float x3 = v3.x;
        float y1 = v1.y;
        float y2 = v2.y;
        float y3 = v3.y;
        float z1 = v1.z;
        float z2 = v2.z;
        float z3 = v3.z;
        return x1*y2*z3 + x2*y3*z1 + x3*y1*z2 - x1*y3*z2 - x2*y1*z3 - x3*y2*z1;
    }
}
