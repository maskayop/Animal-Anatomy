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
            OpenBodyPartDescriptionPanel();
            bodyPartDescriptionPanel.ShowInfo(info);
        }

        public void UnSelectBodyPart()
        {
            CloseBodyPartDescriptionPanel();
        }

        public void SelectBodyPartGroup(BodyPartGroup info)
        {
            OpenBodyPartDescriptionPanel();
            bodyPartDescriptionPanel.ShowInfo(info);
        }

        public void UnSelectBodyPartGroup()
        {
            CloseBodyPartDescriptionPanel();
        }

        public void StartExamination()
        {
            examinationWindow.gameObject.SetActive(true);
            ExaminationController.Instance.StartExamination();
            CloseBodyPartDescriptionPanel();

            if (UI3D_BodyPartsListCanvas.Instance)
                UI3D_BodyPartsListCanvas.Instance.gameObject.SetActive(false);

            if (UI3D_MenuCanvas.Instance)
                UI3D_MenuCanvas.Instance.gameObject.SetActive(false);

            if (UI3D_LeftControllerCanvas.Instance)
                UI3D_LeftControllerCanvas.Instance.gameObject.SetActive(false);

            if (UI3D_RightControllerCanvas.Instance)
                UI3D_RightControllerCanvas.Instance.gameObject.SetActive(false);
        }

        public void StopExamination()
        {
            examinationWindow.gameObject.SetActive(false);
            ExaminationController.Instance.StopExamination();
            CloseBodyPartDescriptionPanel();

            if (UI3D_BodyPartsListCanvas.Instance)
                UI3D_BodyPartsListCanvas.Instance.gameObject.SetActive(true);

            if (UI3D_MenuCanvas.Instance)
                UI3D_MenuCanvas.Instance.gameObject.SetActive(true);

            if (UI3D_LeftControllerCanvas.Instance)
                UI3D_LeftControllerCanvas.Instance.gameObject.SetActive(true);

            if (UI3D_RightControllerCanvas.Instance)
                UI3D_RightControllerCanvas.Instance.gameObject.SetActive(true);
        }

        public UIExaminationWindow GetExaminationWindow()
        {
            return examinationWindow;
        }
    }
}
