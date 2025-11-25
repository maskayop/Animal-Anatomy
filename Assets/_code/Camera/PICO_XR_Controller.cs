using UnityEngine;

namespace AnimalAnatomy
{
    public class PICO_XR_Controller : MonoBehaviour
    {
        public static PICO_XR_Controller Instance;

        void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("Cannot create PICO_XR_Controller");
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }
    }
}
