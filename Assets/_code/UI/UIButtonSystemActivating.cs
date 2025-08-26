using UnityEngine;

namespace AnimalAnatomy
{
    public class UIButtonSystemActivating : MonoBehaviour
    {
        public GameController.SystemType systemType;

        [SerializeField] GameObject tumblerOn;
        [SerializeField] GameObject tumblerOff;

        public bool isActive;

        void Start()
        {
            Init();
        }

        public void Init()
        {
            isActive = false;
            SwitchActiveState();
        }

        public void SwitchActiveState()
        {
            isActive = !isActive;

            tumblerOn.SetActive(isActive);
            tumblerOff.SetActive(!isActive);

            if (isActive)
                GameController.Instance.EnableSystem(systemType);
            else
                GameController.Instance.DisableSystem(systemType);
        }
    }
}
