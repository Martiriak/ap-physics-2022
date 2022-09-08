using UnityEngine;

namespace APPhysics.JumpPad
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class JumpPadBuilder : MonoBehaviour
    {
        [SerializeField] private int GridXDim = 3;
        [SerializeField] private int GridYDim = 3;

        private MeshFilter ThisMeshFilter = null;

        private Transform[] Points = null;

        private void Awake()
        {
            PopulatePointsArray();
        }

        public void GenerateMesh()
        {
            if (ThisMeshFilter == null) ThisMeshFilter = GetComponent<MeshFilter>();
            if (Points == null || Points.Length == 0) PopulatePointsArray();

            Mesh mesh = new Mesh();

            Vector3[] Vertices = new Vector3[Points.Length];
            int[] Triangles = new int[(GridXDim - 1) * (GridYDim - 1) * 6];

            for (int i = 0; i < Points.Length; ++i)
            {
                Vertices[i] = Points[i].localPosition;
            }

            int iQuad = 0;
            for (int y = 0; y < GridYDim - 1; ++y)
                for (int x = 0; x < GridXDim - 1; ++x)
                {
                    Triangles[iQuad]     = FromXYToIndex(x    , y);
                    Triangles[iQuad + 1] = FromXYToIndex(x    , y + 1);
                    Triangles[iQuad + 2] = FromXYToIndex(x + 1, y    );
                    Triangles[iQuad + 3] = FromXYToIndex(x + 1, y    );
                    Triangles[iQuad + 4] = FromXYToIndex(x    , y + 1);
                    Triangles[iQuad + 5] = FromXYToIndex(x + 1, y + 1);

                    iQuad += 6;
                }

            mesh.vertices = Vertices;
            mesh.triangles = Triangles;

            mesh.RecalculateNormals();

            ThisMeshFilter.mesh = mesh;
        }

        private void LateUpdate()
        {
            if (ThisMeshFilter == null) ThisMeshFilter = GetComponent<MeshFilter>();

            Mesh mesh = ThisMeshFilter.mesh;
            if (mesh == null)
            {
                GenerateMesh();
                return;
            }

            Vector3[] Vertices = mesh.vertices;
            for (int i = 0; i < mesh.vertexCount; ++i)
            {
                Vertices[i] = Points[i].localPosition;
            }

            mesh.vertices = Vertices;
        }

        private void PopulatePointsArray()
        {
            Points = new Transform[transform.childCount];

            for (int i = 0; i < transform.childCount; ++i)
            {
                Points[i] = transform.GetChild(i);
            }
        }


        private int FromXYToIndex(int x, int y) => (GridXDim * y) + x;

        private void OnValidate()
        {
            if (GridXDim < 2) GridXDim = 2;
            if (GridYDim < 2) GridYDim = 2;
        }
    }
}
