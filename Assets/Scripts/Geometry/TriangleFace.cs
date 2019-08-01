using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace delaunayTriangulation
{
    public class TriangleFace
    {
        public Vertex P1;
        public Vertex P2;
        public Vertex P3;

        public Tetra RightTetra;
        public Tetra LeftTetra;

        public TriangleFace(Vertex p1, Vertex p2, Vertex p3)
        {
            P1 = p1;
            P2 = p2;
            P3 = p3;
        }

        public TriangleFace(Vertex p1, Vertex p2, Vertex p3, Tetra tetra)
        {
            P1 = p1;
            P2 = p2;
            P3 = p3;
            RightTetra = tetra;
        }

        public TriangleFace(Tetra tetra, Tetra otherTetra)
        {
            tetra.GetCommonVerticesWith(otherTetra, out Vertex p1, out Vertex p2, out Vertex p3);
            P1 = p1;
            P2 = p2;
            P3 = p3;
            LeftTetra = tetra;
            RightTetra = otherTetra;
        }

        public Tetra GetOtherTetraThan(Tetra tetra)
        {
            if (tetra == RightTetra)
            {
                return LeftTetra;
            }
            else if (tetra == LeftTetra)
            {
                return RightTetra;
            }
            else
            {
                throw new System.Exception("The given tetrahedron is not one of the neighbors of the triangle face");
            }
        }

        public override bool Equals(object obj)
        {
            TriangleFace edge = obj as TriangleFace;
            return edge != null &&
                   EqualityComparer<Vertex>.Default.Equals(P1, edge.P1) &&
                   EqualityComparer<Vertex>.Default.Equals(P2, edge.P2) &&
                   EqualityComparer<Vertex>.Default.Equals(P3, edge.P3) &&
                   EqualityComparer<Tetra>.Default.Equals(RightTetra, edge.RightTetra) &&
                   EqualityComparer<Tetra>.Default.Equals(LeftTetra, edge.LeftTetra);
        }

        public override int GetHashCode()
        {
            var hashCode = -1794492244;
            hashCode = hashCode * -1521134295 + EqualityComparer<Vertex>.Default.GetHashCode(P1);
            hashCode = hashCode * -1521134295 + EqualityComparer<Vertex>.Default.GetHashCode(P2);
            hashCode = hashCode * -1521134295 + EqualityComparer<Vertex>.Default.GetHashCode(P3);
            hashCode = hashCode * -1521134295 + EqualityComparer<Tetra>.Default.GetHashCode(RightTetra);
            hashCode = hashCode * -1521134295 + EqualityComparer<Tetra>.Default.GetHashCode(LeftTetra);
            return hashCode;
        }
    }
}
