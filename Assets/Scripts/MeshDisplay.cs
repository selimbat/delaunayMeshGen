using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace delaunayTriangulation
{
    public class MeshDisplay : MonoBehaviour
    {
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private MeshRenderer _sphereMesh;
        [SerializeField] private Material _redMaterial;
        [SerializeField] private Material _greenMaterial;

        private Mesh _mesh;
        private int _numberOfVertices;
        private int _numberOfTriangles;
        private Vector3[] _vertices;
        private Vector3[] _normals;
        private int[] _triangles;

        private Tetra _superTetra;
        private bool _isPreviouslyInsideOfCircumsphere = false;

        public void Init()
        {
            _mesh = _meshFilter.sharedMesh;
            _vertices = _mesh.vertices;
            _numberOfVertices = _vertices.Length;
            _normals = _mesh.normals;
            _triangles = _mesh.triangles;
            _numberOfTriangles = _triangles.Length;
        }

        public void DrawMesh()
        {
            for (int triIndex = 0; triIndex < _numberOfTriangles; triIndex += 3)
            {
                Debug.DrawLine(_vertices[_triangles[triIndex]], _vertices[_triangles[triIndex + 1]], Color.green, 1000);
                Debug.DrawLine(_vertices[_triangles[triIndex + 1]], _vertices[_triangles[triIndex + 2]], Color.green, 1000);
                Debug.DrawLine(_vertices[_triangles[triIndex + 2]], _vertices[_triangles[triIndex]], Color.green, 1000);
            }
        }

        public void DrawNormals()
        {
            for (int i = 0; i < _numberOfVertices; i++)
            {
                Debug.DrawRay(_vertices[i], _normals[i] / 100f , Color.cyan, 1000);
            }
        }

        public void DrawCircumSphereOfMesh()
        {
            Bounds meshBounds = _mesh.bounds;
            ExtDrawGuizmos.DebugWireSphere(meshBounds.center, Color.blue, meshBounds.extents.magnitude, 1000);
        }

        public void DrawSupertetrahedron()
        {
            Bounds meshBounds = _mesh.bounds;
            _superTetra = Tetra.ComputeCircumTetraOfSphere(meshBounds.center, meshBounds.extents.magnitude);
            _superTetra.Show(Color.red, 1000);
        }

        private void Update()
        {
            if (_superTetra != null)
            {
                bool isInsideOfCircumsphere = _superTetra.IsPointInCircumcircle(_sphereMesh.transform.position);
                if (isInsideOfCircumsphere != _isPreviouslyInsideOfCircumsphere)
                {
                    _sphereMesh.material = isInsideOfCircumsphere ? _greenMaterial : _redMaterial;
                    ExtDrawGuizmos.DebugWireSphere(_sphereMesh.transform.position, Color.cyan, 0.01f, 1000, false);
                }
                _isPreviouslyInsideOfCircumsphere = isInsideOfCircumsphere;
            }
        }
    }
}
