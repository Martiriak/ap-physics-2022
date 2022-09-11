using UnityEngine;
using UnityEditor;
using APPhysics.JumpPad;


namespace APPhysics.Editors
{
    [CustomEditor(typeof(JumpPadJointsFixer))]
    public class JumpPadJointsFixer_Editor : Editor
    {
        public override void OnInspectorGUI()
        {
            JumpPadJointsFixer JointsFixer = target as JumpPadJointsFixer;
            DrawDefaultInspector();

            if (GUILayout.Button("Setup Joints")) JointsFixer.SetupJoints();
            if (GUILayout.Button("Remove all Joints"))
            {
                ConfigurableJoint[] AllConfigurableJoints = JointsFixer.transform.GetComponentsInChildren<ConfigurableJoint>();

                foreach (ConfigurableJoint Joint in AllConfigurableJoints)
                    DestroyImmediate(Joint);
            }
        }
    }
}
