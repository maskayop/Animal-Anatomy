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

        ExaminationController examinationController;
        InputController inputController;
        UI3D_MenuCanvas menuCanvas_3D;
        UI3D_LeftControllerCanvas leftCanvas_3D;
        UI3D_RightControllerCanvas rightCanvas_3D;
        UI3D_BodyPartsListCanvas bodyPartsListCanvas_3D;

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
            examinationController = ExaminationController.Instance;
            inputController = InputController.Instance;

            menuCanvas_3D = UI3D_MenuCanvas.Instance;
            leftCanvas_3D = UI3D_LeftControllerCanvas.Instance;
            rightCanvas_3D = UI3D_RightControllerCanvas.Instance;
            bodyPartsListCanvas_3D = UI3D_BodyPartsListCanvas.Instance;

            canvas = GetComponent<Canvas>();
            examinationWindow.gameObject.SetActive(false);
        }

        public void OpenBodyPartDescriptionPanel()
        {
            if (bodyPartDescriptionPanel)
                bodyPartDescriptionPanel.gameObject.SetActive(true);
        }

        public void CloseBodyPartDescriptionPanel()
        {
            if (bodyPartDescriptionPanel)
                bodyPartDescriptionPanel.gameObject.SetActive(false);
        }

        public void SelectBodyPart(BodyPartInfo info)
        {
            OpenBodyPartDescriptionPanel();
            bodyPartDescriptionPanel?.ShowInfo(info);
        }

        public void UnSelectBodyPart()
        {
            CloseBodyPartDescriptionPanel();
        }

        public void SelectBodyPartGroup(BodyPartGroup info)
        {
            OpenBodyPartDescriptionPanel();
            bodyPartDescriptionPanel?.ShowInfo(info);
        }

        public void UnSelectBodyPartGroup()
        {
            CloseBodyPartDescriptionPanel();
        }

        public void StartExamination()
        {
            examinationWindow.gameObject.SetActive(true);
            examinationController?.StartExamination();
            CloseBodyPartDescriptionPanel();

            if (bodyPartsListCanvas_3D)
                bodyPartsListCanvas_3D.gameObject.SetActive(false);

            if (menuCanvas_3D)
                menuCanvas_3D.gameObject.SetActive(false);

            leftCanvas_3D?.SetExaminationMode();
            rightCanvas_3D?.SetExaminationMode();

            if (inputController)
                inputController.isAlternativeInput = false;
        }

        public void StopExamination()
        {
            if (examinationWindow)
                examinationWindow.gameObject.SetActive(false);

            examinationController?.StopExamination();
            CloseBodyPartDescriptionPanel();

            if (bodyPartsListCanvas_3D)
                bodyPartsListCanvas_3D.gameObject.SetActive(true);

            if (menuCanvas_3D)
                menuCanvas_3D.gameObject.SetActive(true);

            if (inputController)
                inputController.isAlternativeInput = false;
        }

        public UIExaminationWindow GetExaminationWindow()
        {
            return examinationWindow;
        }
    }
}
