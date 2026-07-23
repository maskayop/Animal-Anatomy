using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Vopere.Common;

namespace AnimalAnatomy
{
    public class UIButtonSystemActivating : MonoBehaviour
    {
        public GameController.SystemType systemType;

        [Header("KeyCode")]
        [SerializeField] KeyCode keyCode;
        [SerializeField] Key key = Key.None;
        [SerializeField] GameObject keyCodePanel;
        [SerializeField] TextMeshProUGUI keyCodeText;
        [SerializeField] string customKeyCodeName;
        [SerializeField] Image systemTypeColorImage;

        [Header("Text")]
        [SerializeField] TextMeshProUGUI systemNameText;

        [Header("Tumbler")]
        [SerializeField] GameObject tumblerOn;
        [SerializeField] GameObject tumblerOff;

        [Header("Info")]
        public bool isActive;

        [Header("Info")]
        [SerializeField] bool isXR;

        UIButtonClickAudio buttonClickAudio;

#if ENABLE_INPUT_SYSTEM
        Keyboard keyboard = Keyboard.current;
#endif

        void Update()
        {
            if (App.Instance && App.Instance.isXR)
                return;

#if ENABLE_LEGACY_INPUT_MANAGER
            if (keyCode != KeyCode.None)
            {
                if (Input.GetKeyDown(keyCode))
                    SwitchActiveState();
            }
#endif

#if ENABLE_INPUT_SYSTEM
            keyboard = Keyboard.current;

            if (keyboard == null)
                return;

            if (key != Key.None)
            {
                if (IsKeyPressed(key, keyboard))
                    SwitchActiveState();
            }
#endif
        }

        public void Init()
        {
            isActive = false;
            SwitchActiveState();

            systemTypeColorImage.color = ColorsManager.Instance.GetSystemColor(systemType);
            buttonClickAudio = GetComponent<UIButtonClickAudio>();

            if (!keyCodePanel)
                return;

#if ENABLE_LEGACY_INPUT_MANAGER
            if (keyCode == KeyCode.None)
#endif

#if ENABLE_INPUT_SYSTEM
            if (key == Key.None)
#endif
            {
                if (keyCodePanel)
                    keyCodePanel.SetActive(false);
            }
            else
            {
                if (keyCodePanel)
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
                GameController.Instance?.EnableSystem(systemType);
            else
                GameController.Instance?.DisableSystem(systemType);

            if (buttonClickAudio)
                buttonClickAudio?.OnButtonClick();
        }

        public void SetActiveState(bool state)
        {
            isActive = state;

            tumblerOn?.SetActive(isActive);
            tumblerOff?.SetActive(!isActive);
        }

        public void Collapse()
        {
            if (keyCodePanel)
                keyCodePanel.SetActive(false);

            if (!isXR)
                systemNameText.gameObject.SetActive(false);
        }

        public void Expand()
        {
            if (keyCodePanel)
                keyCodePanel.SetActive(true);

            if (!isXR)
                systemNameText.gameObject.SetActive(true);
        }

        bool IsKeyPressed(Key key, Keyboard keyboard)
        {
            return key switch
            {
                Key.Space => keyboard.spaceKey.wasPressedThisFrame,
                Key.Enter => keyboard.enterKey.wasPressedThisFrame,
                Key.Escape => keyboard.escapeKey.wasPressedThisFrame,
                Key.Tab => keyboard.tabKey.wasPressedThisFrame,

                // Буквы A-Z
                Key.A => keyboard.aKey.wasPressedThisFrame,
                Key.B => keyboard.bKey.wasPressedThisFrame,
                Key.C => keyboard.cKey.wasPressedThisFrame,
                Key.D => keyboard.dKey.wasPressedThisFrame,
                Key.E => keyboard.eKey.wasPressedThisFrame,
                Key.F => keyboard.fKey.wasPressedThisFrame,
                Key.G => keyboard.gKey.wasPressedThisFrame,
                Key.H => keyboard.hKey.wasPressedThisFrame,
                Key.I => keyboard.iKey.wasPressedThisFrame,
                Key.J => keyboard.jKey.wasPressedThisFrame,
                Key.K => keyboard.kKey.wasPressedThisFrame,
                Key.L => keyboard.lKey.wasPressedThisFrame,
                Key.M => keyboard.mKey.wasPressedThisFrame,
                Key.N => keyboard.nKey.wasPressedThisFrame,
                Key.O => keyboard.oKey.wasPressedThisFrame,
                Key.P => keyboard.pKey.wasPressedThisFrame,
                Key.Q => keyboard.qKey.wasPressedThisFrame,
                Key.R => keyboard.rKey.wasPressedThisFrame,
                Key.S => keyboard.sKey.wasPressedThisFrame,
                Key.T => keyboard.tKey.wasPressedThisFrame,
                Key.U => keyboard.uKey.wasPressedThisFrame,
                Key.V => keyboard.vKey.wasPressedThisFrame,
                Key.W => keyboard.wKey.wasPressedThisFrame,
                Key.X => keyboard.xKey.wasPressedThisFrame,
                Key.Y => keyboard.yKey.wasPressedThisFrame,
                Key.Z => keyboard.zKey.wasPressedThisFrame,

                // Цифры
                Key.Backquote => keyboard.backquoteKey.wasPressedThisFrame,

                Key.Digit0 => keyboard.digit0Key.wasPressedThisFrame,
                Key.Digit1 => keyboard.digit1Key.wasPressedThisFrame,
                Key.Digit2 => keyboard.digit2Key.wasPressedThisFrame,
                Key.Digit3 => keyboard.digit3Key.wasPressedThisFrame,
                Key.Digit4 => keyboard.digit4Key.wasPressedThisFrame,
                Key.Digit5 => keyboard.digit5Key.wasPressedThisFrame,
                Key.Digit6 => keyboard.digit6Key.wasPressedThisFrame,
                Key.Digit7 => keyboard.digit7Key.wasPressedThisFrame,
                Key.Digit8 => keyboard.digit8Key.wasPressedThisFrame,
                Key.Digit9 => keyboard.digit9Key.wasPressedThisFrame,

                // Стрелки
                Key.UpArrow => keyboard.upArrowKey.wasPressedThisFrame,
                Key.DownArrow => keyboard.downArrowKey.wasPressedThisFrame,
                Key.LeftArrow => keyboard.leftArrowKey.wasPressedThisFrame,
                Key.RightArrow => keyboard.rightArrowKey.wasPressedThisFrame,

                // Модификаторы (если нужны)
                Key.LeftCtrl => keyboard.leftCtrlKey.wasPressedThisFrame,
                Key.RightCtrl => keyboard.rightCtrlKey.wasPressedThisFrame,
                Key.LeftShift => keyboard.leftShiftKey.wasPressedThisFrame,
                Key.RightShift => keyboard.rightShiftKey.wasPressedThisFrame,
                Key.LeftAlt => keyboard.leftAltKey.wasPressedThisFrame,
                Key.RightAlt => keyboard.rightAltKey.wasPressedThisFrame,

                _ => false
            };
        }
    }
}
