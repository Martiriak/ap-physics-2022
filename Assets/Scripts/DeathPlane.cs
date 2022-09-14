using UnityEngine;


namespace APPhysics
{
    public class DeathPlane : MonoBehaviour
    {
        private void OnTriggerEnter(Collider Other)
        {
            Destroy(Other.gameObject);
        }
    }
}
