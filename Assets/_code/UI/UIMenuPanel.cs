using UnityEngine;

namespace AnimalAnatomy
{
    public class UIMenuPanel : MonoBehaviour
    {
        [HideInInspector]
        public UIMenuWindow menuWindow;

        public void CloseMenuWindow()
        {
            if (UIMainCanvas.Instance)
                UIMainCanvas.Instance.CloseMenuWindow();
        }

        public void OpenSettingsWindow()
        {
            menuWindow.OpenSettingsWindow();
        }

        public void OpenControlsWindow()
        {
            menuWindow.OpenControlsWindow();
        }

        public void OpenExamSettingsWindow()
        {
            menuWindow.OpenExamSettingsWindow();
        }

        public void OpenExitWindow()
        {
            menuWindow.OpenExitWindow();
        }
    }
}
