using UnityEngine;

namespace AnimalAnatomy
{
    public class UIButtonLightingMode : MonoBehaviour
    {
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
            LightController.Instance.SetLightingMode(isActive);
            UIMainCanvas.Instance.SetLightingMode(isActive);
        }

        public void SetActiveState(bool state)
        {
            isActive = state;

            tumblerOn.SetActive(isActive);
            tumblerOff.SetActive(!isActive);
        }
    }
}
