using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace delaunayTriangulation
{
    [CustomEditor(typeof(MeshDisplay))]
    public class MeshDisplayEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            MeshDisplay meshDisplay = (MeshDisplay)target;
            if (GUILayout.Button("Init"))
            {
                meshDisplay.Init();
            }
            if (GUILayout.Button("Draw Mesh"))
            {
                meshDisplay.DrawMesh();
            }
            if (GUILayout.Button("Draw Normals"))
            {
                meshDisplay.DrawNormals();
            }
            if (GUILayout.Button("Draw Circum Sphere Of Mesh"))
            {
                meshDisplay.DrawCircumSphereOfMesh();
            }
            if (GUILayout.Button("Draw Supertetrahedron"))
            {
                meshDisplay.DrawSupertetrahedron();
            }
        }

    }
}