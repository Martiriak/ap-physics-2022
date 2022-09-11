using UnityEngine;


namespace APPhysics.JumpPad
{
    public class JumpPadGridBuilder : MonoBehaviour
    {
        [SerializeField] private Vector2Int m_GridDimensions = new Vector2Int(4, 4);
        [Space]
        [SerializeField] private GameObject m_JumpPadPointPrefab = null;
        [SerializeField] private GameObject m_BorderJumpPadPointPrefab = null;

        public int DimX => m_GridDimensions.x;
        public int DimY => m_GridDimensions.y;

        public GameObject JumpPadPointPrefab => m_JumpPadPointPrefab;
        public GameObject BorderJumpPadPointPrefab => m_BorderJumpPadPointPrefab;

        private void OnValidate()
        {
            if (m_GridDimensions.x < 3) m_GridDimensions.x = 3;
            if (m_GridDimensions.y < 3) m_GridDimensions.y = 3;
        }
    }
}
