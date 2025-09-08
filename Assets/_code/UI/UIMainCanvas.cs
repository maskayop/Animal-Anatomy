using UnityEngine;
using TMPro;

namespace AnimalAnatomy
{
    public class UIMainCanvas : MonoBehaviour
    {
        public static UIMainCanvas Instance;

        [Header("System Activating")]
        [SerializeField] GameObject systemActivatingButtonsPanel;

        [Header("Body Part Info")]
        [SerializeField] RectTransform bodyPartInfoPanel;
        [SerializeField] TextMeshProUGUI bodyPartNameText;
        [SerializeField] TextMeshProUGUI bodyPartScientificNameText;
        [SerializeField] TextMeshProUGUI bodyPartDescriptionText;

        [Header("Isolated Mode Buttons")]
        [SerializeField] UIButtonIsolatedMode isolatedModeButton;
        [SerializeField] UIButtonIsolatedMode transparentModeButton;
        [SerializeField] GameObject exclusionModeButton;

        UIButtonSystemActivating[] systemActivatingButtons;

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
            Init();
        }
        
        void Update()
        {
            if (GameController.Instance.isolatedMode || GameController.Instance.transparentMode)
                SetExclusionMode(false);
        }
        
        public void Init()
        {
            systemActivatingButtons = FindObjectsByType<UIButtonSystemActivating>(FindObjectsSortMode.None);
        }

        public void SelectBodyPart(BodyPartInfo info)
        {
            bodyPartInfoPanel.gameObject.SetActive(true);
            bodyPartNameText.text = info.partName;
            bodyPartScientificNameText.text = info.partScientificName;
            bodyPartDescriptionText.text = info.partDescription;
        }

        public void UnSelectBodyPart()
        {
            bodyPartInfoPanel.gameObject.SetActive(false);
        }

        public void SetIsolatedMode(bool state)
        {
            if (GameController.Instance.selectedBodyPart == null)
                return;

            isolatedModeButton.SetActiveState(state);
            transparentModeButton.gameObject.SetActive(!state);
            systemActivatingButtonsPanel.SetActive(!state);
        }

        public void SetTransparentMode(bool state)
        {
            if (GameController.Instance.selectedBodyPart == null)
                return;

            transparentModeButton.SetActiveState(state);
            isolatedModeButton.gameObject.SetActive(!state);
            systemActivatingButtonsPanel.SetActive(!state);
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
    }
}
