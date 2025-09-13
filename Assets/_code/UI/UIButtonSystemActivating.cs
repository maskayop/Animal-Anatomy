using UnityEngine;
using TMPro;

namespace AnimalAnatomy
{
    public class UIButtonSystemActivating : MonoBehaviour
    {
        public GameController.SystemType systemType;

        [Header("KeyCode")]
        [SerializeField] KeyCode keyCode;
        [SerializeField] GameObject keyCodePanel;
        [SerializeField] TextMeshProUGUI keyCodeText;
        [SerializeField] string customKeyCodeName;

        [Header("Tumbler")]
        [SerializeField] GameObject tumblerOn;
        [SerializeField] GameObject tumblerOff;

        [Header("Info")]
        public bool isActive;

        void Start()
        {
            Init();
        }

        void Update()
        {
            if (keyCode != KeyCode.None)
            {
                if (Input.GetKeyDown(keyCode))
                {
                    if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                    {
                        GameController.Instance.DisableAllSystemsExceptSystem(systemType);
                        SetActiveState(true);
                    }
                    else
                        SwitchActiveState();
                }
            }
        }

        public void Init()
        {
            isActive = false;
            SwitchActiveState();

            if (keyCode == KeyCode.None)
            {
                keyCodePanel.SetActive(false);
            }
            else
            {
                keyCodePanel.SetActive(true);

                if (customKeyCodeName == "")
                    keyCodeText.text = keyCode.ToString();
                else
                    keyCodeText.text = customKeyCodeName;
            }
        }

        public void SwitchActiveState()
        {
            SetActiveState(!isActive);

            if (isActive)
                GameController.Instance.EnableSystem(systemType);
            else
                GameController.Instance.DisableSystem(systemType);
        }

        public void SetActiveState(bool state)
        {
            isActive = state;

            tumblerOn.SetActive(isActive);
            tumblerOff.SetActive(!isActive);
        }
    }
}
