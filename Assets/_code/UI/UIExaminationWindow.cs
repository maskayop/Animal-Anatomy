using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace AnimalAnatomy
{
    public class UIExaminationWindow : MonoBehaviour
    {
        public static UIExaminationWindow Instance;

        [Header("Questions Amount")]
        [SerializeField] Slider questionsAmountSlider;
        [SerializeField] TextMeshProUGUI questionsAmountText;

        [Header("Timeout")]
        [SerializeField] Slider timeoutSlider;
        [SerializeField] TextMeshProUGUI timeoutValueText;

        bool isOpen = false;
        public bool IsOpen { get { return isOpen; } set { isOpen = value; } }

        void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("Cannot create UIExaminationWindow");
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
            if (!isOpen)
                return;

            questionsAmountText.text = questionsAmountSlider.value.ToString();
            timeoutValueText.text = timeoutSlider.value.ToString();
        }

        public void Init()
        {
            questionsAmountText.text = questionsAmountSlider.value.ToString();
            timeoutValueText.text = timeoutSlider.value.ToString();
        }
    }
}
