using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Vopere.Common;

namespace AnimalAnatomy
{
    public class UIExaminationWindow : MonoBehaviour
    {
        [Header("Timeout Panel")]
        [SerializeField] GameObject currentTimeoutPanel;
        [SerializeField] TextMeshProUGUI currentTimeText;
        [SerializeField] TextMeshProUGUI currentAnswersAmountText;
        [SerializeField] Image clockImage;
        [SerializeField] Color clockStartColor = Color.white;
        [SerializeField] Color clockEndColor = Color.white;

        [Header("Answers")]
        [SerializeField] GameObject answersPanel;
        [SerializeField] GameObject answersContainer;
        [SerializeField] GameObject correctAnswerButtonPrefab;
        [SerializeField] GameObject wrongAnswerButtonPrefab;

        [Header("Finish Question Panel")]
        [SerializeField] GameObject finishQuestionPanel;
        [SerializeField] GameObject correctAnswerPanel;
        [SerializeField] GameObject wrongAnswerPanel;

        [Header("Finish Exam Panel")]
        [SerializeField] GameObject finishExamPanel;
        [SerializeField] TextMeshProUGUI answersAmountText;
        [SerializeField] GameObject imageBest;
        [SerializeField] GameObject imageGood;
        [SerializeField] GameObject imageNormal;
        [SerializeField] GameObject imageBad;

        [Header("Buttons")]
        [SerializeField] GameObject exitButton;
        [SerializeField] GameObject exitExamWindow;

        bool isOpen = false;
        public bool IsOpen { get { return isOpen; } set { isOpen = value; } }

        List<UIExamAnswerButton> examAnswerButtons = new List<UIExamAnswerButton>();
        ExaminationController examinationController;

        void Start()
        {
            Init();
        }

        void Update()
        {
            if (examinationController && examinationController.isExamination)
            {
                currentTimeText.text = Mathf.FloorToInt(examinationController.currentTime).ToString();
                currentAnswersAmountText.text = examinationController.totalAnswers.ToString() + " / " +
                    examinationController.questionsAmount.ToString();
                clockImage.fillAmount = examinationController.currentTime / (examinationController.timeout * 60);
                clockImage.color = Color.Lerp(clockStartColor, clockEndColor, 1 - clockImage.fillAmount);
            }
        }

        public void Init()
        {
            examinationController = ExaminationController.Instance;
        }

        public void StartExamination()
        {
            answersPanel.SetActive(false);
            currentTimeoutPanel.SetActive(true);
            finishExamPanel.SetActive(false);
            exitButton.SetActive(true);
            exitExamWindow.SetActive(false);
        }

        public void StartNextQuestion(List<BodyPartInfo> info)
        {
            answersPanel.SetActive(true);
            finishQuestionPanel.SetActive(false);
            exitButton.SetActive(true);
            examAnswerButtons.Clear();

            foreach (Transform t in answersContainer.transform)
                Destroy(t.gameObject);

            List<int> randomIds = new List<int>();

            for (int i = 0; i < info.Count; i++)
            {
                randomIds.Add(i);
            }

            randomIds.Shuffle();

            int counter = 0;

            for (int i = 0; i < info.Count; i++)
            {
                if (randomIds[i] == 0)
                {
                    GameObject correctButton = Instantiate(correctAnswerButtonPrefab, answersContainer.transform);
                    UIExamAnswerButton examButton = correctButton.GetComponent<UIExamAnswerButton>();
                    examButton.Init(info[0], true);
                    examAnswerButtons.Add(examButton);
                }
                else
                {
                    GameObject wrongButton = Instantiate(wrongAnswerButtonPrefab, answersContainer.transform);
                    UIExamAnswerButton examButton = wrongButton.GetComponent<UIExamAnswerButton>();
                    examButton.Init(info[randomIds[i]], false);
                    examAnswerButtons.Add(examButton);
                }

                counter++;
            }
        }

        public void FinishQuestion(bool isCorrect)
        {
            for (int i = 0; i < examAnswerButtons.Count; i++)
            {
                if (examAnswerButtons[i].isCorrectAnswer)
                    examAnswerButtons[i].ShowSelection(true);

                examAnswerButtons[i].SetInteractable(false);
            }

            finishQuestionPanel.SetActive(true);
            exitButton.SetActive(false);
            correctAnswerPanel.SetActive(isCorrect);
            wrongAnswerPanel.SetActive(!isCorrect);
        }

        public void CallStartNextQuestion()
        {
            examinationController.StartNextQuestion();
        }

        public void FinishExamination()
        {
            answersPanel.SetActive(false);
            finishQuestionPanel.SetActive(false);
            currentTimeoutPanel.SetActive(false);
            finishExamPanel.SetActive(true);
            exitButton.SetActive(false);

            answersAmountText.text = examinationController.correctAnswers.ToString() + " / " +
                examinationController.questionsAmount.ToString();

            imageBest.SetActive(false);
            imageGood.SetActive(false);
            imageNormal.SetActive(false);
            imageBad.SetActive(false);

            if (examinationController.correctAnswers / examinationController.questionsAmount >= 0.85f)
                imageBest.SetActive(true);
            else if (examinationController.correctAnswers / examinationController.questionsAmount >= 0.7f)
                imageGood.SetActive(true);
            else if (examinationController.correctAnswers / examinationController.questionsAmount >= 0.5f)
                imageNormal.SetActive(true);
            else
                imageBad.SetActive(true);
        }

        public void CallFinishExamination()
        {
            if (UIMainCanvas.Instance)
                UIMainCanvas.Instance.StopExamination();

            if (UI3D_CentralCanvas.Instance)
                UI3D_CentralCanvas.Instance.StopExamination();
        }

        public void OpenExitExamWindow()
        {
            exitExamWindow.gameObject.SetActive(true);
            answersPanel.gameObject.SetActive(false);
            exitButton.gameObject.SetActive(false);
        }

        public void CloseExitExamWindow()
        {
            exitExamWindow.gameObject.SetActive(false);
            answersPanel.gameObject.SetActive(true);
            exitButton.gameObject.SetActive(true);
        }
    }
}
