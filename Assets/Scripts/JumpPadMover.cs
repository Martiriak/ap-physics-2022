using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace APPhysics.JumpPad
{
    public class JumpPadMover : MonoBehaviour
    {
        private void Update()
        {
            transform.Translate(Vector3.forward * 0.5f * Time.deltaTime);
        }
    }
}
