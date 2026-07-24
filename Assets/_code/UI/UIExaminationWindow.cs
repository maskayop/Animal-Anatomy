using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
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
            if (answersPanel)
                answersPanel.SetActive(false);

            if (currentTimeoutPanel)
                currentTimeoutPanel.SetActive(true);

            if (finishExamPanel)
                finishExamPanel.SetActive(false);

            if (exitButton)
                exitButton.SetActive(true);

            if (exitExamWindow)
                exitExamWindow.SetActive(false);
        }

        public void StartNextQuestion(List<BodyPartInfo> info, int correctId)
        {
            answersPanel.SetActive(true);
            finishQuestionPanel.SetActive(false);
            exitButton.SetActive(true);

            examAnswerButtons.Clear();

            foreach (Transform t in answersContainer.transform)
                Destroy(t.gameObject);

            List<int> randomIds = new List<int>();

            for (int i = 0; i < info.Count; i++)
                randomIds.Add(i);

            randomIds.Shuffle();

            int counter = 0;

            for (int i = 0; i < info.Count; i++)
            {
                if (randomIds[i] == correctId)
                {
                    GameObject correctButton = Instantiate(correctAnswerButtonPrefab, answersContainer.transform);
                    UIExamAnswerButton examButton = correctButton.GetComponent<UIExamAnswerButton>();
                    examButton.Init(info[correctId], true);
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

            if (finishQuestionPanel)
                finishQuestionPanel.SetActive(true);

            if (exitButton)
                exitButton.SetActive(false);

            if (correctAnswerPanel)
                correctAnswerPanel.SetActive(isCorrect);

            if (wrongAnswerPanel)
                wrongAnswerPanel.SetActive(!isCorrect);
        }

        public void CallStartNextQuestion()
        {
            examinationController?.StartNextQuestion();
        }

        public void FinishExamination()
        {
            if (answersPanel)
                answersPanel.SetActive(false);

            if (finishQuestionPanel)
                finishQuestionPanel.SetActive(false);

            if (currentTimeoutPanel)
                currentTimeoutPanel.SetActive(false);

            if (finishExamPanel)
                finishExamPanel.SetActive(true);

            if (exitButton)
                exitButton.SetActive(false);

            answersAmountText.text = examinationController.correctAnswers.ToString() + " / " +
                examinationController.questionsAmount.ToString();

            if (imageBest)
                imageBest.SetActive(false);

            if (imageGood)
                imageGood.SetActive(false);

            if (imageNormal)
                imageNormal.SetActive(false);

            if (imageBad)
                imageBad.SetActive(false);

            if (examinationController.correctAnswers / examinationController.questionsAmount >= 0.85f)
            {
                if (imageBest)
                    imageBest.SetActive(true);
            }
            else if (examinationController.correctAnswers / examinationController.questionsAmount >= 0.7f)
            {
                if (imageGood)
                    imageGood.SetActive(true);
            }
            else if (examinationController.correctAnswers / examinationController.questionsAmount >= 0.5f)
            {
                if (imageNormal)
                    imageNormal.SetActive(true);
            }
            else
            {
                if (imageBad)
                    imageBad.SetActive(true);
            }
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
            if (exitExamWindow)
                exitExamWindow.SetActive(true);

            if (answersPanel)
                answersPanel.SetActive(false);

            if (exitButton)
                exitButton.SetActive(false);
        }

        public void CloseExitExamWindow()
        {
            if (exitExamWindow)
                exitExamWindow.SetActive(false);

            if (answersPanel)
                answersPanel.SetActive(true);

            if (exitButton)
                exitButton.SetActive(true);
        }
    }
}
