using UnityEngine;

namespace AnimalAnatomy
{
    public class ExaminationController : MonoBehaviour
    {
        public static ExaminationController Instance;

        public int examMode = 0;
        public int examDifficulty = 0;
        public int examSystemType = 0;
        public int questionsAmount = 0;
        public int timeout = 0;

        public bool isExamination = false;

        float currentTime;

        void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("Cannot create ExaminationController");
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        void Start()
        {
            Init();
        }

        void Update()
        {
            
        }

        public void Init()
        {

        }
    }
}
