using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;
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
        [SerializeField] RectTransform bodyPartDescriptionPanel;
        [SerializeField] TextMeshProUGUI bodyPartNameText;
        [SerializeField] TextMeshProUGUI bodyPartScientificNameText;
        [SerializeField] TextMeshProUGUI bodyPartDescriptionText;

        [Header("Modes")]
        [SerializeField] UIButtonIsolatedMode isolatedModeButton;
        [SerializeField] UIButtonIsolatedMode transparentModeButton;
        [SerializeField] UIButtonExclusionMode exclusionModeButton;
        [SerializeField] UIButtonLightingMode lightingModeButton;

        [Header("Widgets")]
        [SerializeField] GameObject isolatedModeWidget;
        [SerializeField] GameObject transparentModeWidget;
        [SerializeField] GameObject exclusionModeWidget;
        [SerializeField] GameObject lightingModeWidget;

        [Header("Camera")]
        [SerializeField] GameObject fitToCenterCameraButton;

        [Header("Body Parts List")]
        [SerializeField] GameObject bodyPartsListPanel;
        [SerializeField] Transform bodyPartsListContainer;
        [SerializeField] GameObject partsListButtonPrefab;
        [SerializeField] GameObject partsGroupListButtonPrefab;
        [SerializeField] int listButtonsContainerOffset = 5;

        bool bodyPartsListIsOpen = false;
        public bool BodyPartsListIsOpen { get { return bodyPartsListIsOpen; } }

        [Header("Body Parts Group")]
        [SerializeField] GameObject bodyPartsGroupPanel;
        [SerializeField] UIPartsGroupListButton bodyPartsGroupButton;

        [Header("Examination")]
        [SerializeField] GameObject examinationSettingsWindow;

        List<UIPartsListButton> partsListButtons = new List<UIPartsListButton>();        

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

            SetIsolatedMode();
            UpdateWidgets();
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

            examWindow.SetActive(false);
            CreateBodyPartsGroupsListButtons();
            EnableIsolatedModeButtons(false);

#if PLATFORM_ANDROID
            exclusionModeButton.SetInteractable(true);
            lightingModeButton.SetInteractable(true);
#else
            exclusionModeButton.SetInteractable(false);
            lightingModeButton.SetInteractable(false);
#endif
        }

        void SetIsolatedMode()
        {
            if (gameController.selectedBodyPart || gameController.selectedBodyPartsGroup)
            {
                isolatedModeButton.SetActiveState(gameController.isolatedMode);
                transparentModeButton.SetActiveState(gameController.transparentMode);
                exclusionModeButton.SetInteractable(true);

                if (gameController.isolatedMode)
                {
                    transparentModeButton.SetInteractable(false);
                    exclusionModeButton.SetInteractable(false);
                    systemActivatingButtonsPanel.gameObject.SetActive(false);
                }
                else if (gameController.transparentMode)
                {
                    isolatedModeButton.SetInteractable(false);
                    exclusionModeButton.SetInteractable(false);
                    systemActivatingButtonsPanel.gameObject.SetActive(true);
                }
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

        void CreateBodyPartsGroupsListButtons()
        {
            partsListButtons.Clear();

            foreach (Transform t in bodyPartsListContainer)
                Destroy(t.gameObject);

            if (!gameController.baseBodyPartGroup)
                return;

            CreateBodyPartsSingleGroupListButtons(gameController.baseBodyPartGroup);
            CreateBodyPartsListButtons(gameController.baseBodyPartGroup);
        }

        void CreateBodyPartsSingleGroupListButtons(BodyPartGroup group)
        {
            GameObject newGroupGO = Instantiate(partsGroupListButtonPrefab, bodyPartsListContainer);
            UIPartsGroupListButton groupListButton = newGroupGO.GetComponent<UIPartsGroupListButton>();
            groupListButton.Init(group);
            group.partGroupListButton = groupListButton;

            if (group.parentBodyPartGroup)
                groupListButton.containerTransform.offsetMin = 
                    new Vector2(group.parentBodyPartGroup.partGroupListButton.containerTransform.offsetMin.x + listButtonsContainerOffset, 0);

            CreateBodyPartsGroupsListButtons(group.bodyPartsGroups);
        }

        void CreateBodyPartsGroupsListButtons(List<BodyPartGroup> groups)
        {
            for (int i = 0; i < groups.Count; i++)
            {
                CreateBodyPartsSingleGroupListButtons(groups[i]);
                CreateBodyPartsListButtons(groups[i]);
            }
        }
        
        void CreateBodyPartsListButtons(BodyPartGroup group)
        {
            if (group.bodyParts.Count != 0)
            {
                for (int b = 0; b < group.bodyParts.Count; b++)
                {
                    GameObject newGO = Instantiate(partsListButtonPrefab, bodyPartsListContainer);
                    UIPartsListButton listButton = newGO.GetComponent<UIPartsListButton>();
                    listButton.Init(group.bodyParts[b]);
                    group.bodyParts[b].partListButton = listButton;
                    listButton.containerTransform.offsetMin = 
                        new Vector2(group.parentBodyPartGroup.partGroupListButton.containerTransform.offsetMin.x + listButtonsContainerOffset, 0);
                }
            }
        }

        public void SelectBodyPart(BodyPartInfo info)
        {
            EnableIsolatedModeButtons(true);

            bodyPartDescriptionPanel.gameObject.SetActive(true);
            bodyPartNameText.text = info.GetFullRussianName();
            bodyPartScientificNameText.text = info.GetFullScientificName();
            bodyPartDescriptionText.text = info.info.description;

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
            bodyPartNameText.text = info.GetFullRussianName();
            bodyPartScientificNameText.text = info.GetFullScientificName();
            bodyPartDescriptionText.text = info.info.description;

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

        public void ShowBodyPartDescription(bool state)
        {
            bodyPartDescriptionText.gameObject.SetActive(state);
        }

        public void FreezeCamera(bool state)
        {
#if PLATFORM_ANDROID
            return;
#else
            CameraController.Instance.Freeze(state);
#endif
        }

        public void OpenPartsListPanel()
        {
            bodyPartsListPanel.SetActive(true);
            bodyPartsListIsOpen = true;
        }

        public void ClosePartsListPanel()
        {
            bodyPartsListPanel.SetActive(false);
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

        void UpdateWidgets()
        {
            isolatedModeWidget.SetActive(gameController.isolatedMode);
            transparentModeWidget.SetActive(gameController.transparentMode);
            exclusionModeWidget.SetActive(gameController.exclusionMode);
            lightingModeWidget.SetActive(gameController.lightingMode);
        }
    }
}
