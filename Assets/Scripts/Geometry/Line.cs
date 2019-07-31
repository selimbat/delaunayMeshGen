using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line
{
    public Vector3 OffsetPoint;
    public Vector3 Direction;

    public Line(Vector3 offsetPoint, Vector3 direction)
    {
        OffsetPoint = offsetPoint;
        float sqrNorm = direction.sqrMagnitude;
        if (sqrNorm == 0)
        {
            throw new System.Exception("Not possible to define a line with a direction of (0,0,0)");
        }
        else if (sqrNorm != 1)
        {
            Direction = direction / Mathf.Sqrt(sqrNorm);
        }
        else
        {
            Direction = direction;
        }
    }
}
