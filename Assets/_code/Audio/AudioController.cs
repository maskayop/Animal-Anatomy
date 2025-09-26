using UnityEngine;

namespace AnimalAnatomy
{
    public class AudioController : MonoBehaviour
    {
        public static AudioController Instance;

        void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("Cannot create AudioController");
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
