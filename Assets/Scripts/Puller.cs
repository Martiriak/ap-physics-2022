using System.Collections.Generic;
using UnityEngine;


namespace APPhysics.JumpPad
{
    [RequireComponent(typeof(JumpPadGridBuilder))]
    public class Puller : MonoBehaviour
    {
        [SerializeField] private float m_MaxPullForce = 10f;
        [SerializeField] private float m_FullChargeTime = 2f;

        private bool m_IsPulling = false;
        private float m_PullForce = 0f;
        private float m_CurrentChargeTime = 0f;
        private float m_CurrentChargeRate = 0f;

        private JumpPadGridBuilder c_Grid;
        private List<Rigidbody> c_CentralPoints;

        private void Awake()
        {
            c_Grid = GetComponent<JumpPadGridBuilder>();
            c_CentralPoints = new List<Rigidbody>();

            // Get central points of JumpPad
            {
                int x1, x2, y1, y2;
                x2 = y2 = -1;

                x1 = c_Grid.DimX / 2;
                if (c_Grid.DimX % 2 == 0) x2 = x1 - 1;

                y1 = c_Grid.DimY / 2;
                if (c_Grid.DimY % 2 == 0) y2 = y1 - 1;

                c_CentralPoints.Add(c_Grid.GetPointAt(x1, y1));
                if (x2 > -1) c_CentralPoints.Add(c_Grid.GetPointAt(x2, y1));
                if (y2 > -1)
                {
                    c_CentralPoints.Add(c_Grid.GetPointAt(x1, y2));
                    if (x2 > -1) c_CentralPoints.Add(c_Grid.GetPointAt(x2, y2));
                }
            }
        }

        private void Update()
        {
            m_IsPulling = Input.GetKey(KeyCode.Space);
        }

        private void FixedUpdate()
        {
            if (m_IsPulling)
            {
                m_CurrentChargeTime = Mathf.Clamp(m_CurrentChargeTime + Time.fixedDeltaTime, 0f, m_FullChargeTime);
                m_CurrentChargeRate = Mathf.Clamp01(m_CurrentChargeTime / m_FullChargeTime);

                m_PullForce = Mathf.Lerp(0f, m_MaxPullForce, m_CurrentChargeRate);

                foreach (Rigidbody Point in c_CentralPoints)
                {
                    /*Vector3 NewPosition = Point.GetComponent<JumpPadPointLauncher>().RestingPosition;
                    NewPosition.y -= m_PullDistance;
                    Point.transform.position = NewPosition;*/

                    Point.AddForce(Vector3.down * m_PullForce, ForceMode.Impulse);
                }
            }
            else
            {
                m_PullForce = 0f;
                m_CurrentChargeTime = 0f;
                m_CurrentChargeRate = 0f;
            }
        }


        private void OnValidate()
        {
            if (m_MaxPullForce < 0f) m_MaxPullForce = 0f;
            if (m_FullChargeTime < 0.1f) m_FullChargeTime = 0.1f;
        }
    }
}
