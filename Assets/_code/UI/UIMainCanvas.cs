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

        [Header("Body Parts List")]
        [SerializeField] Transform bodyPartsListContainer;
        [SerializeField] GameObject partsListButtonPrefab;

        UIButtonSystemActivating[] systemActivatingButtons;
        List<UIPartsListButton> partsListButtons = new List<UIPartsListButton>();
        List<BodyPartsList> bodyPartsLists = new List<BodyPartsList>();

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
            StartCoroutine(InitializeDelayed());
        }
        
        void Update()
        {
            if (GameController.Instance.isolatedMode || GameController.Instance.transparentMode)
                SetExclusionMode(false);
        }
        
        public void Init()
        {
            systemActivatingButtons = FindObjectsByType<UIButtonSystemActivating>(FindObjectsSortMode.None);
            CreateBodyPartsListButtons();
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

        void CreateBodyPartsListButtons()
        {
            partsListButtons.Clear();

            foreach (Transform t in bodyPartsListContainer)
                Destroy(t.gameObject);

            bodyPartsLists = GameController.Instance.bodyPartsLists;

            for (int i = 0; i < bodyPartsLists.Count; i++)
            {
                if (bodyPartsLists[i].bodyParts.Count != 0)
                {
                    GameObject newSystemTypeGO = Instantiate(partsListButtonPrefab, bodyPartsListContainer);
                    UIPartsListButton systemTypeListButton = newSystemTypeGO.GetComponent<UIPartsListButton>();
                    systemTypeListButton.GetComponent<Button>().interactable = false;
                    systemTypeListButton.Init(bodyPartsLists[i].systemType);

                    for (int b = 0; b < bodyPartsLists[i].bodyParts.Count; b++)
                    {
                        GameObject newGO = Instantiate(partsListButtonPrefab, bodyPartsListContainer);
                        UIPartsListButton listButton = newGO.GetComponent<UIPartsListButton>();
                        listButton.Init(bodyPartsLists[i].bodyParts[b]);
                    }
                }
            }
        }

        IEnumerator InitializeDelayed()
        {
            yield return new WaitForSeconds(2.0f);

            Init();
        }
    }
}
