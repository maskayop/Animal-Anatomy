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
        [SerializeField] GameObject bodyPartInfoPanel;
        [SerializeField] TextMeshProUGUI bodyPartNameText;
        [SerializeField] TextMeshProUGUI bodyPartDescriptionText;
        [SerializeField] UIButtonIsolatedMode isolatedModeButton;
        
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

        }
        
        public void Init()
        {

        }

        public void SelectBodyPart(BodyPartInfo info)
        {
            bodyPartInfoPanel.SetActive(true);
            bodyPartNameText.text = info.partName;
            bodyPartDescriptionText.text = info.partDescription;
        }

        public void UnSelectBodyPart()
        {
            bodyPartInfoPanel.SetActive(false);
        }

        public void SetIsolatedMode(bool state)
        {
            if (GameController.Instance.selectedBodyPart == null)
                return;

            isolatedModeButton.SetActiveState(state);
            systemActivatingButtonsPanel.SetActive(!state);
        }

        public void ActivateSystemSkeleton(bool state)
        {
            if (state)
                GameController.Instance.EnableSystem(GameController.SystemType.skeleton);
            else
                GameController.Instance.DisableSystem(GameController.SystemType.skeleton);
        }

        public void ActivateSystemMuscles(bool state)
        {
            if (state)
                GameController.Instance.EnableSystem(GameController.SystemType.muscles);
            else
                GameController.Instance.DisableSystem(GameController.SystemType.muscles);
        }

        public void ActivateSystemNervous(bool state)
        {
            if (state)
                GameController.Instance.EnableSystem(GameController.SystemType.nervous);
            else
                GameController.Instance.DisableSystem(GameController.SystemType.nervous);
        }

        public void ActivateSystemCirculatory(bool state)
        {
            if (state)
                GameController.Instance.EnableSystem(GameController.SystemType.circulatory);
            else
                GameController.Instance.DisableSystem(GameController.SystemType.circulatory);
        }
        
        public void ActivateSystemSensory(bool state)
        {
            if (state)
                GameController.Instance.EnableSystem(GameController.SystemType.sensory);
            else
                GameController.Instance.DisableSystem(GameController.SystemType.sensory);
        }

        public void ActivateSystemRespiratory(bool state)
        {
            if (state)
                GameController.Instance.EnableSystem(GameController.SystemType.respiratory);
            else
                GameController.Instance.DisableSystem(GameController.SystemType.respiratory);
        }

        public void ActivateSystemDigestive(bool state)
        {
            if (state)
                GameController.Instance.EnableSystem(GameController.SystemType.digestive);
            else
                GameController.Instance.DisableSystem(GameController.SystemType.digestive);
        }

        public void ActivateSystemExcretory(bool state)
        {
            if (state)
                GameController.Instance.EnableSystem(GameController.SystemType.excretory);
            else
                GameController.Instance.DisableSystem(GameController.SystemType.excretory);
        }

        public void ActivateSystemReproductive(bool state)
        {
            if (state)
                GameController.Instance.EnableSystem(GameController.SystemType.reproductive);
            else
                GameController.Instance.DisableSystem(GameController.SystemType.reproductive);
        }

        public void ActivateSystemSkin(bool state)
        {
            if (state)
                GameController.Instance.EnableSystem(GameController.SystemType.skin);
            else
                GameController.Instance.DisableSystem(GameController.SystemType.skin);
        }

        public void ActivateSystemLymphatic(bool state)
        {
            if (state)
                GameController.Instance.EnableSystem(GameController.SystemType.lymphatic);
            else
                GameController.Instance.DisableSystem(GameController.SystemType.lymphatic);
        }

        public void ActivateAllSystems(bool state)
        {
            GameController.Instance.ActivateAllSystems(state);
        }
    }
}
