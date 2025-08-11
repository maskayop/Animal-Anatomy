using UnityEngine;

namespace AnimalAnatomy
{
    public class CameraController : MonoBehaviour
    {
        public static CameraController Instance;

        void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("Cannot create CameraController");
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        void Start()
        {
            Init();
        }

        public void Init()
        {

        }
    }
}
