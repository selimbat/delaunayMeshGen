using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace delaunayTriangulation
{
    public class DelaunayMesh : MonoBehaviour
    {
        [SerializeField] private MeshFilter _meshFilter;

        private Vector3[] _vector3vertices;
        private Vertex[] _vertices;
        private int _numberOfVertices;
        private int _numberOfTriangles;
        private Vector3[] _normals;
        private int[] _triangles;

        private Tetra _superTetra;

        public void Init()
        {
            Mesh mesh = _meshFilter.sharedMesh;
            _vector3vertices = mesh.vertices;
            _numberOfVertices = _vector3vertices.Length;
            FillVeticesArray();
            _normals = mesh.normals;
            _triangles = mesh.triangles;
            _numberOfTriangles = _triangles.Length;
            Bounds meshBounds = mesh.bounds;
            _superTetra = Tetra.ComputeCircumTetraOfSphere(meshBounds.center, meshBounds.extents.magnitude);
        }

        private void FillVeticesArray()
        {
            _vertices = new Vertex[_numberOfVertices];
            for (int i = 0; i < _numberOfVertices; i++)
            {
                _vertices[i] = new Vertex(VertexType.SurfaceVertex, _vector3vertices[i]);
            }
        }

        public void ComputeDelaunayTriangulation()
        {
            StartCoroutine(ComputeDelaunayTriangulationCoroutine());
        }

        public IEnumerator ComputeDelaunayTriangulationCoroutine()
        {
            HashSet<Tetra> triangulation = new HashSet<Tetra> { _superTetra };
            foreach (Vertex vertex in _vertices)
            {
                HashSet<Tetra> badTetras = new HashSet<Tetra>();
                foreach (Tetra tetra in triangulation)
                {
                    if (tetra.IsPointInCircumcircle(vertex.Pos))
                    {
                        badTetras.Add(tetra);
                    }
                }
                HashSet<TriangleFace> polyHole = new HashSet<TriangleFace>();
                foreach (Tetra badTetra in badTetras)
                {
                    foreach (TriangleFace face in badTetra.Faces)
                    {
                        Tetra otherTetraOfFace = face.GetOtherTetraThan(badTetra);
                        if (!badTetras.Contains(otherTetraOfFace))
                        {
                            TriangleFace newFace = new TriangleFace(face.P1, face.P2, face.P3, otherTetraOfFace);
                            polyHole.Add(newFace);
                        }
                    }
                }
                foreach (Tetra badTetra in badTetras)
                {
                    triangulation.Remove(badTetra);
                }
                //debug
                foreach(Tetra tetra in triangulation)
                {
                    tetra.Show(Color.green, 0.2f);
                }
                //!debug
                HashSet<Tetra> newTetras = new HashSet<Tetra>();
                foreach (TriangleFace face in polyHole)
                {
                    newTetras.Add(new Tetra(face, vertex));
                }
                foreach (Tetra tetra in newTetras)
                {
                    foreach (Tetra otherTetra in newTetras)
                    {
                        if (tetra.IsNeighborOf(otherTetra))
                        {
                            TriangleFace face = new TriangleFace(tetra, otherTetra);
                            tetra.AddNeighbor(face);
                        }
                    }
                    tetra.Show(Color.cyan, 0.2f);
                    triangulation.Add(tetra);
                }
                yield return new WaitForSeconds(0.2f);
            }
            foreach (Tetra tetra in triangulation)
            {
                if (tetra.HasACommonVertexWith(_superTetra))
                {
                    triangulation.Remove(tetra);
                }
            }
        }

    }
}