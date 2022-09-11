using UnityEngine;
using UnityEditor;
using APPhysics.MeshBuilder;


namespace APPhysics.Editors
{
    [CustomEditor(typeof(GridMeshBuilder))]
    public class GridMeshBuilder_Editor : Editor
    {
        public override void OnInspectorGUI()
        {
            GridMeshBuilder Builder = target as GridMeshBuilder;
            DrawDefaultInspector();

            if (GUILayout.Button("Generate Mesh")) Builder.GenerateMesh();
        }
    }
}
