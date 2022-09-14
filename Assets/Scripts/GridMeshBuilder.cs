using UnityEngine;


namespace APPhysics.MeshBuilder
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class GridMeshBuilder : MonoBehaviour
    {
        [SerializeField] private Vector2Int m_GridDim = new Vector2Int(3, 3);

        private MeshFilter c_ThisMeshFilter = null;

        private Transform[] m_Points = null;

        private void Awake()
        {
            PopulatePointsArray();
        }

        public void GenerateMesh()
        {
            if (c_ThisMeshFilter == null) c_ThisMeshFilter = GetComponent<MeshFilter>();
            if (m_Points == null || m_Points.Length == 0) PopulatePointsArray();

            Mesh mesh = new Mesh();
            Vector2[] UVs = new Vector2[m_Points.Length];

            Vector3[] Vertices = new Vector3[m_Points.Length];
            int[] Triangles = new int[(m_GridDim.x - 1) * (m_GridDim.y - 1) * 6];

            for (int i = 0; i < m_Points.Length; ++i)
            {
                Vertices[i] = m_Points[i].localPosition;
                UVs[i] = new Vector2
                    (
                        FromIndexToX(i) / m_GridDim.x - 1f,
                        FromIndexToY(i) / m_GridDim.y - 1f
                    );
            }

            // Upper side
            {
                int iQuad = 0;

                for (int y = 0; y < m_GridDim.y - 1; ++y)
                    for (int x = 0; x < m_GridDim.x - 1; ++x)
                    {
                        Triangles[iQuad]     = FromXYToIndex(x    , y    );
                        Triangles[iQuad + 1] = FromXYToIndex(x    , y + 1);
                        Triangles[iQuad + 2] = FromXYToIndex(x + 1, y    );
                        Triangles[iQuad + 3] = FromXYToIndex(x + 1, y    );
                        Triangles[iQuad + 4] = FromXYToIndex(x    , y + 1);
                        Triangles[iQuad + 5] = FromXYToIndex(x + 1, y + 1);

                        iQuad += 6;
                    }
            }

            /* Downer side
            {
                int iQuad = (m_GridDim.x - 1) * (m_GridDim.y - 1) * 6;

                for (int y = m_GridDim.y - 1; y > 1; --y)
                    for (int x = m_GridDim.x - 1; x > 1; --x)
                    {
                        Triangles[iQuad]     = FromXYToIndex(x    , y    );
                        Triangles[iQuad + 1] = FromXYToIndex(x - 1, y    );
                        Triangles[iQuad + 2] = FromXYToIndex(x    , y - 1);
                        Triangles[iQuad + 3] = FromXYToIndex(x    , y - 1);
                        Triangles[iQuad + 4] = FromXYToIndex(x - 1, y    );
                        Triangles[iQuad + 5] = FromXYToIndex(x - 1, y - 1);

                        iQuad += 6;
                    }
            }*/

            mesh.MarkDynamic();

            mesh.vertices = Vertices;
            mesh.triangles = Triangles;
            mesh.SetUVs(0, UVs);

            mesh.RecalculateNormals();
            mesh.RecalculateUVDistributionMetrics();
            mesh.RecalculateTangents();

            c_ThisMeshFilter.mesh = mesh;
        }

        private void LateUpdate()
        {
            if (c_ThisMeshFilter == null) c_ThisMeshFilter = GetComponent<MeshFilter>();

            Mesh mesh = c_ThisMeshFilter.mesh;
            if (mesh == null)
            {
                GenerateMesh();
                return;
            }

            Vector3[] Vertices = mesh.vertices;
            for (int i = 0; i < mesh.vertexCount; ++i)
            {
                Vertices[i] = m_Points[i].localPosition;
            }

            mesh.MarkDynamic();

            mesh.vertices = Vertices;

            mesh.RecalculateTangents();
        }

        private void PopulatePointsArray()
        {
            m_Points = new Transform[transform.childCount];

            for (int i = 0; i < transform.childCount; ++i)
            {
                m_Points[i] = transform.GetChild(i);
            }
        }


        private int FromXYToIndex(int x, int y) => (m_GridDim.x * y) + x;
        private int FromIndexToY(int i) => i / m_GridDim.x;
        private int FromIndexToX(int i) => i % m_GridDim.x;

        private void OnValidate()
        {
            if (m_GridDim.x < 2) m_GridDim.x = 2;
            if (m_GridDim.y < 2) m_GridDim.y = 2;
        }
    }
}
