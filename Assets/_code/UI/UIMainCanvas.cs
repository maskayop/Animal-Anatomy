using System.Collections;
using UnityEngine;
using Vopere.Common;

namespace AnimalAnatomy
{
    public class UIMainCanvas : MonoBehaviour
    {
        public static UIMainCanvas Instance;

        [SerializeField] string mainMenuSceneName;

        [Header("Loading Screen")]
        [SerializeField] GameObject loadingScreen;
        [SerializeField] float loadingTime = 1.0f;

        [Header("Windows")]
        [SerializeField] GameObject mainWindow;
        [SerializeField] GameObject menuWindow;
        [SerializeField] GameObject examWindow;

        [Header("System Activating")]
        [SerializeField] UISystemActivatingButtonsPanel systemActivatingButtonsPanel;

        [Header("Body Part Info")]
        [SerializeField] UIBodyPartDescriptionPanel bodyPartDescriptionPanel;

        [Header("Modes")]
        [SerializeField] UIButtonIsolatedMode isolatedModeButton;
        [SerializeField] UIButtonIsolatedMode transparentModeButton;
        [SerializeField] UIButtonExclusionMode exclusionModeButton;
        [SerializeField] UIButtonLightingMode lightingModeButton;

        [Header("Camera")]
        [SerializeField] GameObject fitToCenterCameraButton;

        [Header("Body Parts List")]
        [SerializeField] UIBodyPartsListPanel bodyPartsListPanel;
        [SerializeField] GameObject partsListPanelButton;

        bool bodyPartsListIsOpen = false;
        public bool BodyPartsListIsOpen { get { return bodyPartsListIsOpen; } }

        [Header("Body Parts Group")]
        [SerializeField] GameObject bodyPartsGroupPanel;
        [SerializeField] UIPartsGroupListButton bodyPartsGroupButton;

        [Header("Examination")]
        [SerializeField] GameObject examinationSettingsWindow;

        [HideInInspector]
        public bool isLoading = false;

        GameController gameController;

        void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("Cannot create UIMainCanvas");
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }
        
        void Start()
        {
            loadingScreen.SetActive(true);
            isLoading = true;

            StartCoroutine(InitializeDelayed());
            StartCoroutine(DisableLoadingScreen());
        }
        
        void Update()
        {
            if (!gameController)
                return;
            
            if (gameController.selectedBodyPart != null || gameController.selectedBodyPartsGroup != null)
                fitToCenterCameraButton.SetActive(true);
            else
                fitToCenterCameraButton.SetActive(false);

            SetGameModes();
        }

        IEnumerator InitializeDelayed()
        {
            yield return new WaitForSeconds(2.0f);

            Init();
        }

        IEnumerator DisableLoadingScreen()
        {
#if UNITY_EDITOR
            loadingTime /= 2;
#endif

            yield return new WaitForSeconds(loadingTime);

            loadingScreen.SetActive(false);
            isLoading = false;
        }

        public void Init()
        {
            gameController = GameController.Instance;

            EnableIsolatedModeButtons(false);
            examWindow.SetActive(false);

            bodyPartsListPanel.Init();
        }

        void SetGameModes()
        {
            if (gameController.exclusionMode)
            {
                isolatedModeButton.SetInteractable(false);
                transparentModeButton.SetInteractable(false);
                exclusionModeButton.SetInteractable(true);

                return;
            }

            if (gameController.selectedBodyPart || gameController.selectedBodyPartsGroup)
            {
                isolatedModeButton.SetInteractable(true);
                transparentModeButton.SetInteractable(true);
                exclusionModeButton.SetInteractable(true);
                systemActivatingButtonsPanel.gameObject.SetActive(true);

                if (gameController.isolatedMode || gameController.transparentMode)
                {
                    exclusionModeButton.SetInteractable(false);
                    systemActivatingButtonsPanel.gameObject.SetActive(false);
                }
                
                if (gameController.isolatedMode)
                    transparentModeButton.SetInteractable(false);

                if (gameController.transparentMode)
                    isolatedModeButton.SetInteractable(false);
            }
            else
            {
                isolatedModeButton.SetInteractable(false);
                transparentModeButton.SetInteractable(false);
                exclusionModeButton.SetInteractable(true);
            }
        }

        public void ActivateAllSystems(bool state)
        {
            systemActivatingButtonsPanel.ActivateAllSystems(state);
        }

        public void ExitApp()
        {
            App.Instance.ExitGame();
        }

        public void ExitToMainMenu()
        {
            if (ScenesManager.Instance.IsSceneAddedToBuild(mainMenuSceneName))
                ScenesManager.Instance.LoadSceneByName(mainMenuSceneName);
            else
                ExitApp();
        }

        public void SelectBodyPart(BodyPartInfo info)
        {
            EnableIsolatedModeButtons(true);

            bodyPartDescriptionPanel.gameObject.SetActive(true);
            bodyPartDescriptionPanel.ShowInfo(info);

            if (info.bodyPartGroup)
            {
                bodyPartsGroupPanel.gameObject.SetActive(true);
                bodyPartsGroupButton.Init(info.bodyPartGroup);
            }
            else
                bodyPartsGroupPanel.gameObject.SetActive(false);
        }

        public void UnSelectBodyPart()
        {
            EnableIsolatedModeButtons(false);

            bodyPartDescriptionPanel.gameObject.SetActive(false);
            bodyPartsGroupPanel.gameObject.SetActive(false);
        }

        public void SelectBodyPartGroup(BodyPartGroup info)
        {
            EnableIsolatedModeButtons(true);

            bodyPartDescriptionPanel.gameObject.SetActive(true);
            bodyPartDescriptionPanel.ShowInfo(info);

            if (info.parentBodyPartGroup)
            {
                bodyPartsGroupPanel.gameObject.SetActive(true);
                bodyPartsGroupButton.Init(info.parentBodyPartGroup);
            }
            else
                bodyPartsGroupPanel.gameObject.SetActive(false);
        }

        public void UnSelectBodyPartGroup()
        {
            EnableIsolatedModeButtons(false);

            bodyPartDescriptionPanel.gameObject.SetActive(false);
            bodyPartsGroupPanel.gameObject.SetActive(false);
        }

        public void FreezeCamera(bool state)
        {
#if PLATFORM_ANDROID
            return;
#else
            if (CameraController.Instance)
                CameraController.Instance.Freeze(state);
#endif
        }

        public void OpenPartsListPanel()
        {
            bodyPartsListPanel.gameObject.SetActive(true);
            partsListPanelButton.SetActive(false);
            bodyPartsListIsOpen = true;
        }

        public void ClosePartsListPanel()
        {
            bodyPartsListPanel.gameObject.SetActive(false);
            partsListPanelButton.SetActive(true);
            CameraController.Instance.Freeze(false);
            bodyPartsListIsOpen = false;
        }

        public void OpenExamWindow()
        {
            examinationSettingsWindow.SetActive(true);
            UIExaminationWindow.Instance.IsOpen = true;
            CameraController.Instance.Freeze(true);
        }

        public void CloseExamWindow()
        {
            examinationSettingsWindow.SetActive(false);
            UIExaminationWindow.Instance.IsOpen = false;
            CameraController.Instance.Freeze(false);
        }

        public void StartExamination()
        {
            CloseExamWindow();
            mainWindow.SetActive(false);
            menuWindow.SetActive(false);
            examWindow.SetActive(true);

            gameController.SetIsolatedMode(false);
            gameController.SetTransparentMode(false);

            ExaminationController.Instance.StartExamination();
        }

        public void FinishExamination()
        {
            StopExamination();
        }

        public void StopExamination()
        {
            mainWindow.SetActive(true);
            menuWindow.SetActive(true);
            examWindow.SetActive(false);

            gameController.SetIsolatedMode(false);
            gameController.SetTransparentMode(false);

            ExaminationController.Instance.StopExamination();
        }

        public void OpenSettingsWindow()
        {
            UISettingsWindow.Instance.Open();
        }

        public void CloseSettingsWindow()
        {
            UISettingsWindow.Instance.Close();            
        }

        public float GetBodyPartsListPanelMaxAnchor()
        {
            if (bodyPartsListPanel)
            {
                RectTransform rt = bodyPartsListPanel.GetComponent<RectTransform>();
                return rt.anchorMax.x;
            }
            else
                return 0;
        }

        public void CallBodyPartSelection()
        {
            InputController.Instance.CallBodyPartSelection();
        }

        public void CameraFitToCenter()
        {
            CameraController.Instance.UpdatePosition();
        }

        void EnableIsolatedModeButtons(bool state)
        {
            isolatedModeButton.SetInteractable(state);
            transparentModeButton.SetInteractable(state);
        }
    }
}
