using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

namespace AnimalAnatomy
{
    public class UIMainCanvas : MonoBehaviour
    {
        public static UIMainCanvas Instance;

        [Header("Loading Screen")]
        [SerializeField] GameObject loadingScreen;
        [SerializeField] float loadingTime = 1.0f;

        [Header("Windows")]
        [SerializeField] GameObject mainWindow;
        [SerializeField] GameObject menuWindow;
        [SerializeField] GameObject examWindow;

        [Header("System Activating")]
        [SerializeField] GameObject systemActivatingButtonsPanel;

        [Header("Body Part Info")]
        [SerializeField] RectTransform bodyPartInfoPanel;
        [SerializeField] TextMeshProUGUI bodyPartNameText;
        [SerializeField] TextMeshProUGUI bodyPartScientificNameText;
        [SerializeField] TextMeshProUGUI bodyPartDescriptionText;

        [Header("Isolated Mode Buttons")]
        [SerializeField] GameObject isolatedModeButtonsContainer;
        [SerializeField] UIButtonIsolatedMode isolatedModeButton;
        [SerializeField] UIButtonIsolatedMode transparentModeButton;
        [SerializeField] GameObject exclusionModeButton;

        [Header("Body Parts List")]
        [SerializeField] GameObject bodyPartsListPanel;
        [SerializeField] Transform bodyPartsListContainer;
        [SerializeField] GameObject partsListButtonPrefab;
        [SerializeField] GameObject partsGroupListButtonPrefab;
        [SerializeField] int listButtonsContainerOffset = 5;

        [Header("Body Parts Group")]
        [SerializeField] GameObject bodyPartsGroupPanel;
        [SerializeField] UIPartsGroupListButton bodyPartsGroupButton;

        [Header("Examination")]
        [SerializeField] GameObject examinationSettingsWindow;

        [Header("Settings")]
        [SerializeField] GameObject settingsWindow;

        UIButtonSystemActivating[] systemActivatingButtons;
        List<UIPartsListButton> partsListButtons = new List<UIPartsListButton>();

        [Header("Vopere")]
        [SerializeField] GameObject vopere;

        [HideInInspector]
        public bool isLoading = false;

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
            if (GameController.Instance.isolatedMode || GameController.Instance.transparentMode)
                SetExclusionMode(false);
        }

        IEnumerator InitializeDelayed()
        {
            yield return new WaitForSeconds(2.0f);

            Init();
        }

        IEnumerator DisableLoadingScreen()
        {
            yield return new WaitForSeconds(loadingTime);

            loadingScreen.SetActive(false);
            isLoading = false;
        }

        public void Init()
        {
            systemActivatingButtons = FindObjectsByType<UIButtonSystemActivating>(FindObjectsSortMode.None);
            examWindow.SetActive(false);
            CreateBodyPartsGroupsListButtons();
            CollapseSystemActivatingButtons(true);
        }

        public void SetIsolatedMode(bool state)
        {
            if (GameController.Instance.selectedBodyPart || GameController.Instance.selectedBodyPartsGroup)
            {
                isolatedModeButton.SetActiveState(state);
                transparentModeButton.gameObject.SetActive(!state);
                systemActivatingButtonsPanel.SetActive(!state);
            }
        }

        public void SetTransparentMode(bool state)
        {
            if (GameController.Instance.selectedBodyPart || GameController.Instance.selectedBodyPartsGroup)
            {
                transparentModeButton.SetActiveState(state);
                isolatedModeButton.gameObject.SetActive(!state);
                systemActivatingButtonsPanel.SetActive(!state);
            }
        }

        public void ActivateAllSystems(bool state)
        {
            for (int i = 0; i < systemActivatingButtons.Length; i++)
            {
                systemActivatingButtons[i].SetActiveState(state);
            }
        }

        public void SetExclusionMode(bool state)
        {
            exclusionModeButton.SetActive(state);
        }

        public void ExitApp()
        {
            App.Instance.ExitGame();
        }

        void CreateBodyPartsGroupsListButtons()
        {
            partsListButtons.Clear();

            foreach (Transform t in bodyPartsListContainer)
                Destroy(t.gameObject);

            if (!GameController.Instance.baseBodyPartGroup)
                return;

            CreateBodyPartsSingleGroupListButtons(GameController.Instance.baseBodyPartGroup);
            CreateBodyPartsListButtons(GameController.Instance.baseBodyPartGroup);
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
            bodyPartInfoPanel.gameObject.SetActive(true);
            isolatedModeButtonsContainer.gameObject.SetActive(true);
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
            bodyPartInfoPanel.gameObject.SetActive(false);
            isolatedModeButtonsContainer.gameObject.SetActive(false);
            bodyPartsGroupPanel.gameObject.SetActive(false);
        }

        public void SelectBodyPartGroup(BodyPartGroup info)
        {
            bodyPartInfoPanel.gameObject.SetActive(true);
            isolatedModeButtonsContainer.gameObject.SetActive(true);
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
            bodyPartInfoPanel.gameObject.SetActive(false);
            isolatedModeButtonsContainer.gameObject.SetActive(false);
            bodyPartsGroupPanel.gameObject.SetActive(false);
        }

        public void ShowBodyPartDescription(bool state)
        {
            bodyPartDescriptionText.gameObject.SetActive(state);
        }

        public void FreezeCamera(bool state)
        {
            CameraController.Instance.Freeze(state);
        }

        public void OpenPartsListPanel()
        {
            bodyPartsListPanel.SetActive(true);
        }

        public void ClosePartsListPanel()
        {
            bodyPartsListPanel.SetActive(false);
            CameraController.Instance.Freeze(false);
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

            GameController.Instance.SetIsolatedMode(false);
            GameController.Instance.SetTransparentMode(false);

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

            GameController.Instance.SetIsolatedMode(false);
            GameController.Instance.SetTransparentMode(false);

            ExaminationController.Instance.StopExamination();
        }

        public void ShowVopere(bool state)
        {
            vopere.SetActive(state);
        }

        public void CollapseSystemActivatingButtons(bool state)
        {
            for (int i = 0; i < systemActivatingButtons.Length; i++)
            {
                if (state)
                    systemActivatingButtons[i].Collapse();
                else
                    systemActivatingButtons[i].Expand();
            }
        }

        public void OpenSettingsWindow()
        {
            settingsWindow.SetActive(true);
            UISettingsWindow.Instance.IsOpen = true;
            CameraController.Instance.Freeze(true);
        }

        public void CloseSettingsWindow()
        {
            settingsWindow.SetActive(false);
            UISettingsWindow.Instance.IsOpen = false;
            CameraController.Instance.Freeze(false);
        }
    }
}
