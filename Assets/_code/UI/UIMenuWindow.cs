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
            settingsWindow.menuWindow = this;
            controlsWindow.menuWindow = this;
            examinationSettingsWindow.menuWindow = this;
            exitWindow.menuWindow = this;

            CloseSettingsWindow();
        }

        public void OpenSettingsWindow()
        {
            settingsWindow.Open();
            CameraController.Instance.Freeze(true);
        }

        public void CloseSettingsWindow()
        {
            settingsWindow.Close();
            CameraController.Instance.Freeze(false);
        }

        public void OpenControlsWindow()
        {
            controlsWindow.gameObject.SetActive(true);
            CameraController.Instance.Freeze(true);
        }

        public void CloseControlsWindow()
        {
            controlsWindow.gameObject.SetActive(false);
            CameraController.Instance.Freeze(false);
        }

        public void OpenExamSettingsWindow()
        {
            examinationSettingsWindow.Open();
            UIExaminationWindow.Instance.IsOpen = true;
            CameraController.Instance.Freeze(true);
        }

        public void CloseExamSettingsWindow()
        {            
            examinationSettingsWindow.Close();
            UIExaminationWindow.Instance.IsOpen = false;
            CameraController.Instance.Freeze(false);
        }

        public void OpenExitWindow()
        {
            exitWindow.gameObject.SetActive(true);
            CameraController.Instance.Freeze(true);
        }

        public void CloseExitWindow()
        {
            exitWindow.gameObject.SetActive(false);
            CameraController.Instance.Freeze(false);
        }
    }
}
