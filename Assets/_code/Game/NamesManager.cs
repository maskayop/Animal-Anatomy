using UnityEngine;

namespace AnimalAnatomy
{
    public class NamesManager : MonoBehaviour
    {
        public static NamesManager Instance;

        void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("Cannot create NamesManager");
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
