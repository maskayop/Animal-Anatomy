using System.Collections.Generic;
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
        [SerializeField] TextMeshProUGUI bodyPartDescriptionText;
        [SerializeField] UIButtonIsolatedMode isolatedModeButton;

        RectTransform bodyPartDescriptionTransform;

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
            ForceExpandBodyPartInfoPanel();
        }
        
        public void Init()
        {
            bodyPartDescriptionTransform = bodyPartDescriptionText.GetComponent<RectTransform>();
            systemActivatingButtons = FindObjectsByType<UIButtonSystemActivating>(FindObjectsSortMode.None);
        }

        public void SelectBodyPart(BodyPartInfo info)
        {
            bodyPartInfoPanel.gameObject.SetActive(true);
            bodyPartNameText.text = info.partName;
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
            systemActivatingButtonsPanel.SetActive(!state);
        }

        public void ActivateAllSystems(bool state)
        {
            for (int i = 0; i < systemActivatingButtons.Length; i++)
            {
                systemActivatingButtons[i].SetActiveState(state);
            }
        }

        void ForceExpandBodyPartInfoPanel()
        {
            bodyPartInfoPanel.sizeDelta = new Vector2 (bodyPartInfoPanel.sizeDelta.x,
                bodyPartDescriptionTransform.offsetMin.y +
                -bodyPartDescriptionTransform.offsetMax.y +
                bodyPartDescriptionText.GetRenderedValues().y);
        }
    }
}
