using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane
{
    public Vector3 OffsetPoint;
    public Vector3 Normal;

    public Plane(Vector3 offsetPoint, Vector3 normal)
    {
        OffsetPoint = offsetPoint;
        float sqrNorm = normal.sqrMagnitude;
        if (sqrNorm == 0)
        {
            throw new System.Exception("Not possible to define a plane with a normal of (0,0,0)");
        }
        else if (sqrNorm != 1)
        {
            Normal = normal / Mathf.Sqrt(sqrNorm);
        }
        else
        {
            Normal = normal;
        }
    }

    public bool IntersectPlane(out Line resultingLine, Plane otherPlane)
    {
        Vector3 offsetPoint = Vector3.zero;
        Vector3 direction = Vector3.zero;

        //We can get the direction of the line of intersection of the two planes by calculating the 
        //cross product of the normals of the two planes. Note that this is just a direction and the line
        //is not fixed in space yet. We need a point for that to go with the line vector.
        direction = Vector3.Cross(this.Normal, otherPlane.Normal);

        //Next is to calculate a point on the line to fix it's position in space. This is done by finding a vector from
        //the plane2 location, moving parallel to it's plane, and intersecting plane1. To prevent rounding
        //errors, this vector also has to be perpendicular to lineDirection. To get this vector, calculate
        //the cross product of the normal of plane2 and the lineDirection.		
        Vector3 ldir = Vector3.Cross(otherPlane.Normal, direction);

        float denominator = Vector3.Dot(this.Normal, ldir);

        //Prevent divide by zero and rounding errors by requiring about 5 degrees angle between the planes.
        if (Mathf.Abs(denominator) > 0.006f)
        {
            Vector3 plane1ToPlane2 = this.OffsetPoint - otherPlane.OffsetPoint;
            float t = Vector3.Dot(this.Normal, plane1ToPlane2) / denominator;
            offsetPoint = otherPlane.OffsetPoint + t * ldir;
            resultingLine = new Line(offsetPoint, direction);
            return true;
        }
        // Planes are parallel (including the case where planes are equal)
        else
        {
            resultingLine = default;
            return false;
        }
    }
}
