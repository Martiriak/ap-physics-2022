using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace APPhysics.Movement
{
    [RequireComponent(typeof(Rigidbody))]
    public class Jumper : MonoBehaviour
    {
        [SerializeField] private float m_MaxJumpForce = 10f;
        [SerializeField] private float m_FullChargeTime = 2f;

        private bool m_IsJumping = false;
        private float m_JumpForce = 0f;
        private float m_CurrentChargeTime = 0f;
        private float m_CurrentChargeRate = 0f;

        private Rigidbody c_Rigidbody;

        private void Awake() => c_Rigidbody = GetComponent<Rigidbody>();

        private void Update()
        {
            m_IsJumping = Input.GetKey(KeyCode.Space);
        }

        private void FixedUpdate()
        {
            if (m_IsJumping)
            {
                m_CurrentChargeTime = Mathf.Clamp(m_CurrentChargeTime + Time.fixedDeltaTime, 0f, m_FullChargeTime);
                m_CurrentChargeRate = Mathf.Clamp01(m_CurrentChargeTime / m_FullChargeTime);

                m_JumpForce = Mathf.Lerp(0f, m_MaxJumpForce, m_CurrentChargeRate);
                c_Rigidbody.AddForce(Vector3.down * m_JumpForce, ForceMode.Force);
            }
            else
            {
                m_JumpForce = 0f;
                m_CurrentChargeTime = 0f;
                m_CurrentChargeRate = 0f;
            }
        }


        private void OnValidate()
        {
            if (m_JumpForce < 0f) m_JumpForce = 0f;
            if (m_FullChargeTime < 0.1f) m_FullChargeTime = 0.1f;
        }
    }
}
