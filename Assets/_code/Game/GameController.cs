using UnityEngine;

namespace AnimalAnatomy
{
    public class GameController : MonoBehaviour
    {
        public static GameController Instance;

        public enum SystemType { skeleton, muscles, nervous, circulatory, sensory, respiratory, digestive, excretory, reproductive, skin }
        public SystemType systemType;

        void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("Cannot create GameController");
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
