using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace AnimalAnatomy
{
    public class UIExaminationSettingsWindow : MonoBehaviour
    {
        [HideInInspector]
        public UIMenuWindow menuWindow;

        [Header("Questions Amount")]
        [SerializeField] Slider questionsAmountSlider;
        [SerializeField] TextMeshProUGUI questionsAmountText;

        [Header("Timeout Settings")]
        [SerializeField] Slider timeoutSlider;
        [SerializeField] TextMeshProUGUI timeoutValueText;

        ExaminationController examinationController;

        bool isOpen = false;
        public bool IsOpen { get { return isOpen; } set { isOpen = value; } }

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
            examinationController = ExaminationController.Instance;

            SetExamMode(0);
            SetExamDifficulty(0);
            SetExamSystemType(0);
            SetExamQuestionsAmount();
            SetExamTimeOut();

            questionsAmountText.text = questionsAmountSlider.value.ToString();
            timeoutValueText.text = timeoutSlider.value.ToString();
        }

        public void Open()
        {
            gameObject.SetActive(true);
            isOpen = true;
        }

        public void Close()
        {
            gameObject.SetActive(false);
            isOpen = false;
        }

        public void StartExamination()
        {
            if (UIMainCanvas.Instance)
                UIMainCanvas.Instance.StartExamination();
        }

        public void SetExamMode(int value)
        {
            examinationController.examMode = value;
        }

        public void SetExamDifficulty(int value)
        {
            examinationController.examDifficulty = value;
        }

        public void SetExamSystemType(int value)
        {
            examinationController.examSystemType = value;
        }

        public void SetExamQuestionsAmount()
        {
            examinationController.questionsAmount = Mathf.FloorToInt(questionsAmountSlider.value);
        }

        public void SetExamTimeOut()
        {
            examinationController.timeout = Mathf.FloorToInt(timeoutSlider.value);
        }
    }
}
