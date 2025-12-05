using UnityEngine;

namespace AnimalAnatomy
{
    public class UIMenuWindow : MonoBehaviour
    {
        [SerializeField] UIMenuPanel menuPanel;
        [SerializeField] UISettingsWindow settingsWindow;
        [SerializeField] UIControlsWindow controlsWindow;
        [SerializeField] UIExaminationSettingsWindow examinationSettingsWindow;
        [SerializeField] UIExitWindow exitWindow;

        void Start()
        {
            Init();
        }

        public void Init()
        {
            menuPanel.menuWindow = this;
        }

        public void OpenSettingsWindow()
        {
            settingsWindow.Open();
        }

        public void CloseSettingsWindow()
        {
            settingsWindow.Close();
        }

        public void OpenControlsWindow()
        {
            controlsWindow.Open();
        }

        public void CloseControlsWindow()
        {
            controlsWindow.Close();
        }

        public void OpenExamSettingsWindow()
        {
            examinationSettingsWindow.Open();

            if (UIMainCanvas.Instance)
                UIMainCanvas.Instance.GetExaminationWindow().IsOpen = true;
        }

        public void CloseExamSettingsWindow()
        {            
            examinationSettingsWindow.Close();

            if (UIMainCanvas.Instance)
                UIMainCanvas.Instance.GetExaminationWindow().IsOpen = false;
        }

        public void OpenExitWindow()
        {
            exitWindow.Open();
        }

        public void CloseExitWindow()
        {
            exitWindow.Close();
        }
    }
}
