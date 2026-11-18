using System.Collections.Generic;
using UnityEngine;
using Vopere.Common;

namespace AnimalAnatomy
{
    public class ExaminationController : MonoBehaviour
    {
        public static ExaminationController Instance;

        public bool allInOne = false;

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

        GameController gameController;
        BodyPartInfo currentInfo;
        BodyPartInfo previousInfo;

        CameraController cameraController;
        UIMainCanvas mainCanvas;
        UI3D_CentralCanvas centralCanvas_3D;

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
            gameController = GameController.Instance;
            cameraController = CameraController.Instance;
            mainCanvas = UIMainCanvas.Instance;
            centralCanvas_3D = UI3D_CentralCanvas.Instance;

            currentQuestionBodyPartInfos.Clear();
        }

        public void StartExamination()
        {
            if (!gameController) return;

            isExamination = true;
            examSystemTypeBodyPartInfos.Clear();
            currentQuestionBodyPartInfos.Clear();

            gameController.DisableAllSystemsExceptSystem(GameController.SystemType.skin);
            gameController.SetIsolatedMode(false);
            gameController.SetTransparentMode(false);

            if (allInOne)
                UpdateExamSystemTypeBodyPartInfos(GameController.SystemType.none);
            else
            {
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
                else
                    UpdateExamSystemTypeBodyPartInfos(GameController.SystemType.none);
            }

            correctAnswers = 0;
            wrongAnswers = 0;
            totalAnswers = 0;
            currentTime = timeout * 60;

            mainCanvas?.GetExaminationWindow().StartExamination();
            centralCanvas_3D?.GetExaminationWindow().StartExamination();

            if (InputController.Instance)
                InputController.Instance.isAlternativeInput = false;

            StartNextQuestion();
        }

        public void FinishExamination()
        {
            StopExamination();

            mainCanvas?.GetExaminationWindow().FinishExamination();
            centralCanvas_3D?.GetExaminationWindow().FinishExamination();
        }

        public void StopExamination()
        {
            if (!gameController) return;

            isExamination = false;
            examSystemTypeBodyPartInfos.Clear();
            currentQuestionBodyPartInfos.Clear();

            gameController.SetIsolatedMode(false);
            gameController.SetTransparentMode(false);

            for (int i = 0; i < gameController.allBodyParts.Count; i++)
                gameController.allBodyParts[i].SetAsTransparent(false);

            gameController.ActivateAllSystems(true);
            cameraController?.UpdatePosition();
        }

        public void StartNextQuestion()
        {
            if (totalAnswers >= questionsAmount)
            {
                FinishExamination();
                return;
            }

            examSystemTypeBodyPartInfos.Shuffle();

            for (int i = 0; i < gameController.allBodyParts.Count; i++)
                gameController.allBodyParts[i].SetAsTransparent(true);

            currentQuestionBodyPartInfos.Clear();

            for (int i = 0; i < 4; i++)
                currentQuestionBodyPartInfos.Add(examSystemTypeBodyPartInfos[i]);

            int correctId = 0;

            if (previousInfo == currentQuestionBodyPartInfos[0])
                correctId = 1;

            currentInfo = currentQuestionBodyPartInfos[correctId];
            currentInfo.SetAsTransparent(false);

            cameraController?.UpdatePositionOnBodyPart(currentInfo);
            mainCanvas?.GetExaminationWindow().StartNextQuestion(currentQuestionBodyPartInfos, correctId);
            centralCanvas_3D?.GetExaminationWindow().StartNextQuestion(currentQuestionBodyPartInfos, correctId);

            previousInfo = currentInfo;
        }

        void UpdateExamSystemTypeBodyPartInfos(GameController.SystemType systemType)
        {
            gameController.EnableSystem(systemType);

            for (int i = 0; i < gameController.bodyPartsLists.Count; i++)
            {
                if (gameController.bodyPartsLists[i].systemType == systemType)
                {
                    for (int p = 0; p < gameController.bodyPartsLists[i].bodyParts.Count; p++)
                        examSystemTypeBodyPartInfos.Add(gameController.bodyPartsLists[i].bodyParts[p]);
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

            mainCanvas?.GetExaminationWindow().FinishQuestion(isCorrect);
            centralCanvas_3D?.GetExaminationWindow().FinishQuestion(isCorrect);
        }
    }
}
