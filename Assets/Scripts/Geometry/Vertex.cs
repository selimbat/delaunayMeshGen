using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace delaunayTriangulation
{
    public enum VertexType
    {
        Undefined = 0,
        SurfaceVertex = 1,
        VolumeVertex = 2
    }

    public class Vertex
    {
        public VertexType Type;
        public float X;
        public float Y;
        public float Z;
        public Vector3 Pos { get { return new Vector3(X, Y, Z); } private set { } }

        public Vertex(VertexType type, float x, float y, float z)
        {
            Type = type;
            X = x;
            Y = y;
            Z = z;
        }

        public Vertex(VertexType type, Vector3 v)
        {
            Type = type;
            X = v.x;
            Y = v.y;
            Z = v.z;
        }

        public override bool Equals(object obj)
        {
            Vertex vertex = obj as Vertex;
            return vertex != null
                   && X == vertex.X
                   && Y == vertex.Y
                   && Z == vertex.Z;
        }

        public override int GetHashCode()
        {
            var hashCode = -1934074354;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            hashCode = hashCode * -1521134295 + Z.GetHashCode();
            return hashCode;
        }
    }
}
