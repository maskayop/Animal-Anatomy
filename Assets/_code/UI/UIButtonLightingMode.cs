using UnityEngine;

namespace AnimalAnatomy
{
    public class UIButtonLightingMode : MonoBehaviour
    {
        [SerializeField] GameObject tumblerOn;
        [SerializeField] GameObject tumblerOff;

        public bool isActive;

        CanvasGroup canvasGroup;

        void Start()
        {
            Init();
        }

        public void Init()
        {
            isActive = true;
            SwitchActiveState();

            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void SwitchActiveState()
        {
            SetActiveState(!isActive);
            LightController.Instance.SetLightingMode(isActive);
            GameController.Instance.SetLightingMode(isActive);
        }

        public void SetActiveState(bool state)
        {
            isActive = state;

            tumblerOn.SetActive(isActive);
            tumblerOff.SetActive(!isActive);
        }

        public void SetInteractable(bool state)
        {
            if (!canvasGroup)
                return;

            canvasGroup.interactable = state;
            canvasGroup.blocksRaycasts = state;

            if (state)
                canvasGroup.alpha = 1.0f;
            else
                canvasGroup.alpha = 0.5f;
        }
    }
}
