using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace delaunayTriangulation
{
    public class DelaunayMesh : MonoBehaviour
    {
        [SerializeField] private MeshFilter _meshFilter;

        private Vector3[] _vector3vertices;
        private HashSet<Vertex> _vertices;
        private int _numberOfVertices;
        private int _numberOfTriangles;
        private Vector3[] _normals;
        private int[] _triangles;

        private Tetra _superTetra;

        private int _debugIndex = 0;
        private HashSet<Tetra> _triangulation;

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
            _vertices = new HashSet<Vertex>();
            for (int i = 0; i < _numberOfVertices; i++)
            {
                _vertices.Add(new Vertex(VertexType.SurfaceVertex, _vector3vertices[i]));
            }
        }


        public void ComputeDelaunayTriangulation()
        {
            HashSet<Tetra> triangulation = new HashSet<Tetra> { _superTetra };
            foreach (Vertex vertex in _vertices)
            {
                HashSet<Tetra> badTetras = GetBadTetrasInCurrentTriangulation(triangulation, vertex);
                HashSet<TriangleFace> polyHole = GetPolygonalHoleFromBadTetras(badTetras);
                RemoveBadTetras(triangulation, badTetras);
                HashSet<Tetra> tetrasToAdd = GetNewTetrasFromPolygonalHole(vertex, polyHole);
                AddNewTetrasToTriangulation(triangulation, tetrasToAdd);
                _triangulation = triangulation;
            }
            RemoveTetrasFromSuperTetra(triangulation);
            _triangulation = triangulation;
            Tetra.Show(triangulation, Color.green, 1000);
        }

        public void ShowTriangulationTetraByTetra()
        {
            List<Tetra> triangulation = new List<Tetra>(_triangulation);
            triangulation[_debugIndex].Show(_debugIndex % 2 == 0 ? Color.yellow : Color.blue, 1);
            _debugIndex++;
            if (_debugIndex >= triangulation.Count)
            {
                _debugIndex = 0;
            }
        }

        private HashSet<Tetra> GetBadTetrasInCurrentTriangulation(HashSet<Tetra> triangulation, Vertex vertex)
        {
            HashSet<Tetra> badTetras = new HashSet<Tetra>();
            foreach (Tetra tetra in triangulation)
            {
                if (tetra.IsPointInCircumcircle(vertex.Pos))
                {
                    badTetras.Add(tetra);
                }
            }
            return badTetras;
        }

        private HashSet<TriangleFace> GetPolygonalHoleFromBadTetras(HashSet<Tetra> badTetras)
        {
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
            return polyHole;
        }

        private void RemoveBadTetras(HashSet<Tetra> triangulation, HashSet<Tetra> badTetras)
        {
            foreach (Tetra badTetra in badTetras)
            {
                triangulation.Remove(badTetra);
            }
        }

        private HashSet<Tetra> GetNewTetrasFromPolygonalHole(Vertex vertex, HashSet<TriangleFace> polyHole)
        {
            HashSet<Tetra> tetrasToAdd = new HashSet<Tetra>();
            foreach (TriangleFace face in polyHole)
            {
                Tetra newTetra = new Tetra(face, vertex);
                tetrasToAdd.Add(newTetra);
            }
            foreach (Tetra tetra in tetrasToAdd)
            {
                foreach (Tetra otherTetra in tetrasToAdd)
                {
                    if (tetra.IsNeighborOf(otherTetra))
                    {
                        TriangleFace face = new TriangleFace(tetra, otherTetra);
                        tetra.AddNeighbor(face);
                    }
                }
            }

            return tetrasToAdd;
        }

        private void AddNewTetrasToTriangulation(HashSet<Tetra> triangulation, HashSet<Tetra> tetrasToAdd)
        {
            foreach (Tetra tetra in tetrasToAdd)
            {
                triangulation.Add(tetra);
            }
        }

        private void RemoveTetrasFromSuperTetra(HashSet<Tetra> triangulation)
        {
            HashSet<Tetra> triangulationDuplicate = new HashSet<Tetra>(triangulation);
            foreach (Tetra tetra in triangulationDuplicate)
            {
                if (tetra.HasACommonVertexWith(_superTetra))
                {
                    triangulation.Remove(tetra);
                }
            }
        }

    }
}