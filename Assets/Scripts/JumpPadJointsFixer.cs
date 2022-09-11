using UnityEngine;
using UnityEngine.Assertions;
using APPhysics.Utility;


namespace APPhysics.JumpPad
{
    public class JumpPadJointsFixer : MonoBehaviour
    {
        [SerializeField] private Vector2Int m_GridDim = new Vector2Int(3, 3);
        [SerializeField] private ConfigurableJoint m_JointToAdd = null;

        [SerializeField] private bool m_DisableJointsSetup = false;

        private Rigidbody[] m_JumpPadPoints = null;

        private void Awake()
        {
            if (!m_DisableJointsSetup) SetupJoints();
        }

        public void SetupJoints()
        {
            Assert.IsNotNull(m_JointToAdd);
            Assert.AreEqual(m_GridDim.x * m_GridDim.y, transform.childCount);

            if (m_JumpPadPoints == null || m_JumpPadPoints.Length == 0)
            {
                m_JumpPadPoints = new Rigidbody[transform.childCount];

                for (int idx = 0; idx < m_JumpPadPoints.Length; ++idx)
                {
                    m_JumpPadPoints[idx] = transform.GetChild(idx).GetComponent<Rigidbody>();
                    Assert.IsNotNull(m_JumpPadPoints[idx]);
                }
            }


            int i;

            for (int y = 0; y < m_GridDim.y; ++y)
                for (int x = 0; x < m_GridDim.x; ++x)
                {
                    i = FromXYToIndex(x, y);

                    foreach (ConfigurableJoint Joint in m_JumpPadPoints[i].gameObject.GetComponents<ConfigurableJoint>())
                    {
                        Destroy(Joint);
                    }

                    Rigidbody NorthPoint = null;
                    Rigidbody SouthPoint = null;
                    Rigidbody EastPoint  = null;
                    Rigidbody WestPoint  = null;


                    if (IsBounded(x    , y + 1)) NorthPoint = m_JumpPadPoints[FromXYToIndex(x    , y + 1)];
                    if (IsBounded(x    , y - 1)) SouthPoint = m_JumpPadPoints[FromXYToIndex(x    , y - 1)];
                    if (IsBounded(x + 1, y    )) EastPoint  = m_JumpPadPoints[FromXYToIndex(x + 1, y    )];
                    if (IsBounded(x - 1, y    )) WestPoint  = m_JumpPadPoints[FromXYToIndex(x - 1, y    )];


                    if (NorthPoint != null)
                    {
                        ConfigurableJoint NewJoint = APUtility.CopyComponentInto(m_JointToAdd, m_JumpPadPoints[i].gameObject);
                        NewJoint.connectedBody = NorthPoint;
                    }

                    if (SouthPoint != null)
                    {
                        ConfigurableJoint NewJoint = APUtility.CopyComponentInto(m_JointToAdd, m_JumpPadPoints[i].gameObject);
                        NewJoint.connectedBody = SouthPoint;
                    }

                    if (EastPoint != null)
                    {
                        ConfigurableJoint NewJoint = APUtility.CopyComponentInto(m_JointToAdd, m_JumpPadPoints[i].gameObject);
                        NewJoint.connectedBody = EastPoint;
                    }

                    if (WestPoint != null)
                    {
                        ConfigurableJoint NewJoint = APUtility.CopyComponentInto(m_JointToAdd, m_JumpPadPoints[i].gameObject);
                        NewJoint.connectedBody = WestPoint;
                    }
                }
        }

        private int FromXYToIndex(int x, int y) => (m_GridDim.x * y) + x;

        private bool IsBounded(int x, int y) => x >= 0 && y >= 0 && x < m_GridDim.x && y < m_GridDim.y;

        private void OnValidate()
        {
            if (m_GridDim.x < 2) m_GridDim.x = 2;
            if (m_GridDim.y < 2) m_GridDim.y = 2;
        }
    }
}
