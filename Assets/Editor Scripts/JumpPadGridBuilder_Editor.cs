using UnityEngine;
using UnityEditor;
using UnityEngine.Assertions;
using APPhysics.JumpPad;


namespace APPhysics.Editors
{
    [CustomEditor(typeof(JumpPadGridBuilder))]
    public class JumpPadGridBuilder_Editor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Build Grid")) BuildGrid();
            if (GUILayout.Button("Destroy Grid")) DestroyGrid();
        }

        private void BuildGrid()
        {
            JumpPadGridBuilder Builder = target as JumpPadGridBuilder;
            Assert.IsNotNull(Builder);

            if (Builder.transform.childCount != 0) DestroyGrid();

            bool IsPointBorder;
            GameObject PrefabToInstantiate;

            for (int y = 0; y < Builder.DimY; ++y)
                for (int x = 0; x < Builder.DimX; ++x)
                {
                    IsPointBorder = x == 0 || y == 0 || y == Builder.DimY - 1 || x == Builder.DimX - 1;
                    PrefabToInstantiate = IsPointBorder ? Builder.BorderJumpPadPointPrefab : Builder.JumpPadPointPrefab;

                    GameObject NewPoint = PrefabUtility.InstantiatePrefab(PrefabToInstantiate) as GameObject;
                    NewPoint.transform.SetParent(Builder.transform);
                    NewPoint.transform.position = Builder.transform.position;

                    NewPoint.transform.localPosition = new Vector3(0f + x, 0f, 0f + y);
                }
        }

        private void DestroyGrid()
        {
            JumpPadGridBuilder Builder = target as JumpPadGridBuilder;
            Assert.IsNotNull(Builder);

            while (Builder.transform.childCount > 0)
            {
                DestroyImmediate(Builder.transform.GetChild(0).gameObject);
            }
        }
    }
}
