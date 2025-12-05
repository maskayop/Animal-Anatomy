using UnityEngine;

namespace AnimalAnatomy
{
    public class UI3D_CentralCanvas : MonoBehaviour
    {
        public static UI3D_CentralCanvas Instance;

        [Header("UI")]
        [SerializeField] UIBodyPartDescriptionPanel bodyPartDescriptionPanel;
        [SerializeField] UIExaminationWindow examinationWindow;

        Canvas canvas;
        GameController gameController;

        void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("Cannot create UI3D_CentralCanvas");
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
            if (canvas)
                transform.LookAt(canvas.worldCamera.transform);
        }

        public void Init()
        {
            gameController = GameController.Instance;
            canvas = GetComponent<Canvas>();
            examinationWindow.gameObject.SetActive(false);
        }

        public void OpenBodyPartDescriptionPanel()
        {
            bodyPartDescriptionPanel.gameObject.SetActive(true);
        }

        public void CloseBodyPartDescriptionPanel()
        {
            bodyPartDescriptionPanel.gameObject.SetActive(false);
        }

        public void SelectBodyPart(BodyPartInfo info)
        {
            bodyPartDescriptionPanel.gameObject.SetActive(true);
            bodyPartDescriptionPanel.ShowInfo(info);
        }

        public void UnSelectBodyPart()
        {
            bodyPartDescriptionPanel.gameObject.SetActive(false);
        }

        public void SelectBodyPartGroup(BodyPartGroup info)
        {
            bodyPartDescriptionPanel.gameObject.SetActive(true);
            bodyPartDescriptionPanel.ShowInfo(info);
        }

        public void UnSelectBodyPartGroup()
        {
            bodyPartDescriptionPanel.gameObject.SetActive(false);
        }

        public void StartExamination()
        {
            /*
            CloseMenuWindow();
            menuWindow.CloseExamSettingsWindow();

            mainWindow.SetActive(false);
            */

            gameController.SetIsolatedMode(false);
            gameController.SetTransparentMode(false);            

            examinationWindow.gameObject.SetActive(true);
            ExaminationController.Instance.StartExamination();
        }

        public void StopExamination()
        {
            /*
            OpenMenuWindow();

            mainWindow.SetActive(true);
            */

            gameController.SetIsolatedMode(false);
            gameController.SetTransparentMode(false);

            examinationWindow.gameObject.SetActive(false);
            ExaminationController.Instance.StopExamination();
        }

        public UIExaminationWindow GetExaminationWindow()
        {
            return examinationWindow;
        }
    }
}
