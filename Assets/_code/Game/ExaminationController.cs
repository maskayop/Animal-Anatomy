using System.Collections.Generic;
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

        BodyPartInfo currentQuestionBodyPartInfo;
        List<BodyPartInfo> examSystemTypeBodyPartInfos = new List<BodyPartInfo>();

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
            currentQuestionBodyPartInfo = null;
        }

        public void StartExamination()
        {
            isExamination = true;
            examSystemTypeBodyPartInfos.Clear();
            GameController.Instance.DisableAllSystemsExceptSystem(GameController.SystemType.skin);

            if (examSystemType == 0)
            {
                UpdateExamSystemTypeBodyPartInfos(GameController.SystemType.skeleton);
            }
            else if (examSystemType == 1)
            {
                UpdateExamSystemTypeBodyPartInfos(GameController.SystemType.muscles);

            }
            else
            {
                UpdateExamSystemTypeBodyPartInfos(GameController.SystemType.digestive);
                UpdateExamSystemTypeBodyPartInfos(GameController.SystemType.endocrine);
                UpdateExamSystemTypeBodyPartInfos(GameController.SystemType.excretory);
                UpdateExamSystemTypeBodyPartInfos(GameController.SystemType.reproductive);
                UpdateExamSystemTypeBodyPartInfos(GameController.SystemType.respiratory);
            }
            
            StartNextQuestion();
        }

        public void FinishExamination()
        {
            StopExamination();
        }

        public void StopExamination()
        {
            isExamination = false;
            examSystemTypeBodyPartInfos.Clear();
            currentQuestionBodyPartInfo = null;

            for (int i = 0; i < GameController.Instance.allBodyParts.Count; i++)
            {
                GameController.Instance.allBodyParts[i].SetAsTransparent(false);
            }

            GameController.Instance.ActivateAllSystems(true);
        }

        void StartNextQuestion()
        {
            for (int i = 0; i < GameController.Instance.allBodyParts.Count; i++)
            {
                GameController.Instance.allBodyParts[i].SetAsTransparent(true);
            }

            int randomValue = Random.Range(0, examSystemTypeBodyPartInfos.Count);
            currentQuestionBodyPartInfo = examSystemTypeBodyPartInfos[randomValue];
            currentQuestionBodyPartInfo.SetAsTransparent(false);
            CameraController.Instance.UpdatePositionOnBodyPart(currentQuestionBodyPartInfo);
        }

        void UpdateExamSystemTypeBodyPartInfos(GameController.SystemType systemType)
        {
            GameController.Instance.EnableSystem(systemType);

            for (int i = 0; i < GameController.Instance.bodyPartsLists.Count; i++)
            {
                if (GameController.Instance.bodyPartsLists[i].systemType == systemType)
                {
                    for (int p = 0; p < GameController.Instance.bodyPartsLists[i].bodyParts.Count; p++)
                    {
                        examSystemTypeBodyPartInfos.Add(GameController.Instance.bodyPartsLists[i].bodyParts[p]);
                    }
                }
            }
        }
    }
}
