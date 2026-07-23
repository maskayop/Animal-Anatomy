using UnityEngine;
using UnityEngine.InputSystem;

namespace Vopere.UI
{
    public class UIVopere : MonoBehaviour
    {
        [SerializeField] GameObject window;

#if ENABLE_INPUT_SYSTEM
        Keyboard keyboard = Keyboard.current;
#endif

        void Update()
        {
#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftAlt) &&
                Input.GetKey(KeyCode.F) && Input.GetKey(KeyCode.Alpha0))
                ShowWindow(true);
            else
                ShowWindow(false);
#endif

#if ENABLE_INPUT_SYSTEM
            keyboard = Keyboard.current;

            if (keyboard == null)
                return;

            if (keyboard.leftCtrlKey.isPressed && keyboard.leftShiftKey.isPressed && keyboard.leftAltKey.isPressed && keyboard.fKey.isPressed && keyboard.digit0Key.isPressed)
                ShowWindow(true);
            else
                ShowWindow(false);
#endif
        }

        public void ShowWindow(bool state)
        {
            window.SetActive(state);
        }
    }
}
