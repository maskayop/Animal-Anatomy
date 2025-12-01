using UnityEngine;

namespace AnimalAnatomy
{
    public class UIButtonIsolatedMode : MonoBehaviour
    {
        [SerializeField] bool transparentMode = false;

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

            if (transparentMode)
                GameController.Instance.SetTransparentMode(isActive);
            else
                GameController.Instance.SetIsolatedMode(isActive);
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

            if (state)
                canvasGroup.alpha = 1.0f;
            else
                canvasGroup.alpha = 0.5f;
        }
    }
}
