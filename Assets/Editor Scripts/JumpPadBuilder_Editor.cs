using UnityEngine;
using UnityEditor;
using APPhysics.JumpPad;

namespace APPhysics.Editors
{
    [CustomEditor(typeof(JumpPadBuilder))]
    public class JumpPadBuilder_Editor : Editor
    {
        public override void OnInspectorGUI()
        {
            JumpPadBuilder Builder = target as JumpPadBuilder;
            DrawDefaultInspector();

            if (GUILayout.Button("Generate Mesh")) Builder.GenerateMesh();
        }
    }
}
