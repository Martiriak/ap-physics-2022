using UnityEngine;
using UnityEngine.Assertions;


namespace APPhysics.JumpPad
{
    public class JumpPadGridBuilder : MonoBehaviour
    {
        [SerializeField] private Vector2Int m_GridDimensions = new Vector2Int(4, 4);
        [SerializeField] private float m_DistanceBetweenPoints = 0.5f;
        [Space]
        [SerializeField] private GameObject m_JumpPadPointPrefab = null;
        [SerializeField] private GameObject m_BorderJumpPadPointPrefab = null;

        public int DimX => m_GridDimensions.x;
        public int DimY => m_GridDimensions.y;
        public float DistanceBetweenPoints => m_DistanceBetweenPoints;

        public GameObject JumpPadPointPrefab => m_JumpPadPointPrefab;
        public GameObject BorderJumpPadPointPrefab => m_BorderJumpPadPointPrefab;


        public Rigidbody GetPointAt(int x, int y)
        {
            Assert.IsFalse(transform.childCount == 0);
            if (x < 0 || y < 0 || x >= DimX || y >= DimY) return null;

            return transform.GetChild(FromXYToIndex(x, y)).GetComponent<Rigidbody>();
        }

        private int FromXYToIndex(int x, int y) => (DimX * y) + x;

        private void OnValidate()
        {
            if (m_GridDimensions.x < 3) m_GridDimensions.x = 3;
            if (m_GridDimensions.y < 3) m_GridDimensions.y = 3;
        }
    }
}
