using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace delaunayTriangulation
{
    [CustomEditor(typeof(DelaunayMesh))]
    public class DelauneyMesgEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            DelaunayMesh delaunayMesh = (DelaunayMesh)target;
            if (GUILayout.Button("Compute Delaunay Triangulation"))
            {
                delaunayMesh.ComputeDelaunayTriangulation();
            }
        }

    }
}