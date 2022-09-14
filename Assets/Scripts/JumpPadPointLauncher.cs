using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;


namespace APPhysics.JumpPad
{
    [RequireComponent(typeof(Rigidbody))]
    public class JumpPadPointLauncher : MonoBehaviour
    {
        [SerializeField] private ConfigurableJoint m_SpringCoefficientHolder = null;
        [Space]
        [SerializeField] private float m_Tolerance = 0.05f;

        private float m_RejectionForce = 0f;
        private bool m_CanLaunch = false;
        private float m_HighestYPos;

        private Vector3 c_RestingPosition;
        private Rigidbody c_Rigidbody;
        private float c_SpringCoefficient;


        public Vector3 RestingPosition => c_RestingPosition;


        private void Awake()
        {
            Assert.IsNotNull(m_SpringCoefficientHolder);
            c_Rigidbody = GetComponent<Rigidbody>();
            c_RestingPosition = transform.position;
            c_SpringCoefficient = m_SpringCoefficientHolder.yDrive.positionSpring;

            m_HighestYPos = c_RestingPosition.y;
        }


        private void FixedUpdate()
        {
            if (transform.position.y < c_RestingPosition.y - m_Tolerance)
            {
                float CurrentSpringForce = c_SpringCoefficient * Mathf.Abs(transform.position.y - c_RestingPosition.y);
                if (CurrentSpringForce > m_RejectionForce)
                    m_RejectionForce = CurrentSpringForce;
            }
            else if (m_RejectionForce > 0f)
            {
                if (transform.position.y > m_HighestYPos + m_Tolerance)
                {
                    m_HighestYPos = transform.position.y;
                }
                else if (transform.position.y < m_HighestYPos - m_Tolerance)
                {
                    m_CanLaunch = true;
                    StartCoroutine(ResetLaunch());
                    m_HighestYPos = c_RestingPosition.y;
                }
            }
        }

        private void OnCollisionStay(Collision Other)
        {
            if (m_CanLaunch)
            {
                Other.rigidbody.AddForce(Vector3.up * m_RejectionForce, ForceMode.Force);
            }
        }


        private IEnumerator ResetLaunch()
        {
            yield return new WaitForFixedUpdate();
            m_CanLaunch = false;
            m_RejectionForce = 0f;
        }
    }
}
