using UnityEngine;

namespace AnimalAnatomy
{
    public class UIButtonIsolatedMode : MonoBehaviour
    {
        [SerializeField] bool transparentMode = false;

        [SerializeField] GameObject tumblerOn;
        [SerializeField] GameObject tumblerOff;

        public bool isActive;

        void Start()
        {
            Init();
        }

        public void Init()
        {
            isActive = true;
            SwitchActiveState();
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
    }
}
