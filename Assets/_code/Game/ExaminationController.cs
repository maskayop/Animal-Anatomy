using System.Collections.Generic;
using UnityEngine;
using Vopere.Common;

namespace AnimalAnatomy
{
    public class ExaminationController : MonoBehaviour
    {
        public static ExaminationController Instance;

        [Header("Settings")]
        public int examMode = 0;
        public int examDifficulty = 0;
        public int examSystemType = 0;
        public int questionsAmount = 0;
        public int timeout = 0;

        public bool isExamination = false;

        List<BodyPartInfo> currentQuestionBodyPartInfos = new List<BodyPartInfo>();
        List<BodyPartInfo> examSystemTypeBodyPartInfos = new List<BodyPartInfo>();

        [Header("Info")]
        public int correctAnswers = 0;
        public int wrongAnswers = 0;
        public int totalAnswers = 0;

        public float currentTime;

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
            if (!isExamination)
                return;

            currentTime -= Time.deltaTime;

            if (currentTime <= 0)
                FinishExamination();
        }

        public void Init()
        {
            currentQuestionBodyPartInfos.Clear();
        }

        public void StartExamination()
        {
            isExamination = true;
            examSystemTypeBodyPartInfos.Clear();
            currentQuestionBodyPartInfos.Clear();
            GameController.Instance.DisableAllSystemsExceptSystem(GameController.SystemType.skin);

            if (examSystemType == 0)
                UpdateExamSystemTypeBodyPartInfos(GameController.SystemType.skeleton);
            else if (examSystemType == 1)
                UpdateExamSystemTypeBodyPartInfos(GameController.SystemType.muscles);
            else if (examSystemType == 2)
            {
                UpdateExamSystemTypeBodyPartInfos(GameController.SystemType.digestive);
                UpdateExamSystemTypeBodyPartInfos(GameController.SystemType.endocrine);
                UpdateExamSystemTypeBodyPartInfos(GameController.SystemType.excretory);
                UpdateExamSystemTypeBodyPartInfos(GameController.SystemType.reproductive);
                UpdateExamSystemTypeBodyPartInfos(GameController.SystemType.respiratory);
            }
            
            correctAnswers = 0;
            wrongAnswers = 0;
            totalAnswers = 0;
            currentTime = timeout * 60;

            if (UIMainCanvas.Instance)
                UIMainCanvas.Instance.GetExaminationWindow().StartExamination();

            StartNextQuestion();
        }

        public void FinishExamination()
        {
            StopExamination();

            if (UIMainCanvas.Instance)
                UIMainCanvas.Instance.GetExaminationWindow().FinishExamination();
        }

        public void StopExamination()
        {
            isExamination = false;
            examSystemTypeBodyPartInfos.Clear();
            currentQuestionBodyPartInfos.Clear();

            for (int i = 0; i < GameController.Instance.allBodyParts.Count; i++)
                GameController.Instance.allBodyParts[i].SetAsTransparent(false);

            GameController.Instance.ActivateAllSystems(true);
            CameraController.Instance.UpdatePosition();
        }

        public void StartNextQuestion()
        {
            if (totalAnswers >= questionsAmount)
            {
                FinishExamination();
                return;
            }

            examSystemTypeBodyPartInfos.Shuffle();

            for (int i = 0; i < GameController.Instance.allBodyParts.Count; i++)
                GameController.Instance.allBodyParts[i].SetAsTransparent(true);

            currentQuestionBodyPartInfos.Clear();

            for (int i = 0; i < 4; i++)
                currentQuestionBodyPartInfos.Add(examSystemTypeBodyPartInfos[i]);

            currentQuestionBodyPartInfos[0].SetAsTransparent(false);
            CameraController.Instance.UpdatePositionOnBodyPart(currentQuestionBodyPartInfos[0]);

            if (UIMainCanvas.Instance)
                UIMainCanvas.Instance.GetExaminationWindow().StartNextQuestion(currentQuestionBodyPartInfos);
        }

        void UpdateExamSystemTypeBodyPartInfos(GameController.SystemType systemType)
        {
            GameController.Instance.EnableSystem(systemType);

            for (int i = 0; i < GameController.Instance.bodyPartsLists.Count; i++)
            {
                if (GameController.Instance.bodyPartsLists[i].systemType == systemType)
                {
                    for (int p = 0; p < GameController.Instance.bodyPartsLists[i].bodyParts.Count; p++)
                        examSystemTypeBodyPartInfos.Add(GameController.Instance.bodyPartsLists[i].bodyParts[p]);
                }
            }
        }

        public void FinishQuestion(bool isCorrect)
        {
            if (isCorrect)
                correctAnswers++;
            else
                wrongAnswers++;

            totalAnswers++;

            if (UIMainCanvas.Instance)
                UIMainCanvas.Instance.GetExaminationWindow().FinishQuestion(isCorrect);
        }
    }
}
