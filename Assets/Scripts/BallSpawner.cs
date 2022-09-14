using UnityEngine;
using UnityEngine.Assertions;


namespace APPhysics.Spawner
{
    public class BallSpawner : MonoBehaviour
    {
        [SerializeField] private Rigidbody m_BaseObjectToSpawn = null;

        [Tooltip("How many objects to spawn per physics step. [0, 20]")]
        [SerializeField] private Vector2Int m_NumberOfSpawnRange = new Vector2Int(1, 6);

        [Space]

        [Tooltip("Spawned Objects will have their size in this range. [0.1f, 100f]")]
        [SerializeField] private Vector2 m_SizeRange = new Vector2(0.1f, 1.5f);

        [Tooltip("Spawned Objects will have their mass in this range. [0.1f, 100f]")]
        [SerializeField] private Vector2 m_MassRange = new Vector2(0.1f, 1.5f);

        [Space]

        [Tooltip("Objects will spawn at random offsets from the spawner's position. Only 0f or positive values.")]
        [SerializeField] private Vector3 m_Offsets = new Vector3(4f, 2f, 4f);


        private bool m_Spawning = false;


        private void Awake()
        {
            Assert.IsNotNull(m_BaseObjectToSpawn);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftAlt)) m_Spawning = !m_Spawning;
        }

        private void FixedUpdate()
        {
            if (m_Spawning) SpawnObjects(Random.Range(m_NumberOfSpawnRange.x, m_NumberOfSpawnRange.y));
        }


        private void SpawnObjects(int NumberToSpawn)
        {
            Vector3 SpawnPosition;
            float ObjectScale;

            for (int i = 0; i < NumberToSpawn; ++i)
            {
                SpawnPosition = transform.position;
                SpawnPosition.x += Random.Range(m_Offsets.x, -m_Offsets.x);
                SpawnPosition.y += Random.Range(m_Offsets.y, -m_Offsets.y);
                SpawnPosition.z += Random.Range(m_Offsets.z, -m_Offsets.z);

                Rigidbody NewObjectRB = Instantiate(m_BaseObjectToSpawn, SpawnPosition, Quaternion.identity);

                NewObjectRB.mass = Random.Range(m_MassRange.x, m_MassRange.y);

                ObjectScale = Random.Range(m_SizeRange.x, m_SizeRange.y);
                NewObjectRB.transform.localScale = new Vector3(ObjectScale, ObjectScale, ObjectScale);
            }
        }


        private void OnValidate()
        {
            m_NumberOfSpawnRange.y = Mathf.Clamp(m_NumberOfSpawnRange.y, 0, 20);
            m_NumberOfSpawnRange.x = Mathf.Clamp(m_NumberOfSpawnRange.x, 0, m_NumberOfSpawnRange.y);

            m_SizeRange.y = Mathf.Clamp(m_SizeRange.y, 0.1f, 100f);
            m_SizeRange.x = Mathf.Clamp(m_SizeRange.x, 0.1f, m_SizeRange.y);

            m_MassRange.y = Mathf.Clamp(m_MassRange.y, 0.1f, 100f);
            m_MassRange.x = Mathf.Clamp(m_MassRange.x, 0.1f, m_MassRange.y);

            if (m_Offsets.x < 0f) m_Offsets.x = 0f;
            if (m_Offsets.y < 0f) m_Offsets.y = 0f;
            if (m_Offsets.z < 0f) m_Offsets.z = 0f;
        }
    }
}
