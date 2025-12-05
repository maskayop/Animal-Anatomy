using UnityEngine;
using Vopere.Common;

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
            SetXRMenuPanelActive(!settingsWindow.IsOpen);
        }

        public void CloseSettingsWindow()
        {
            settingsWindow.Close();
            SetXRMenuPanelActive(!settingsWindow.IsOpen);
        }

        public void OpenControlsWindow()
        {
            controlsWindow.Open();
            SetXRMenuPanelActive(!controlsWindow.IsOpen);
        }

        public void CloseControlsWindow()
        {
            controlsWindow.Close();
            SetXRMenuPanelActive(!controlsWindow.IsOpen);
        }

        public void OpenExamSettingsWindow()
        {
            examinationSettingsWindow.Open();
            SetXRMenuPanelActive(!examinationSettingsWindow.IsOpen);

            if (UIMainCanvas.Instance)
                UIMainCanvas.Instance.GetExaminationWindow().IsOpen = true;
        }

        public void CloseExamSettingsWindow()
        {            
            examinationSettingsWindow.Close();
            SetXRMenuPanelActive(!examinationSettingsWindow.IsOpen);

            if (UIMainCanvas.Instance)
                UIMainCanvas.Instance.GetExaminationWindow().IsOpen = false;
        }

        public void OpenExitWindow()
        {
            exitWindow.Open();
            SetXRMenuPanelActive(!exitWindow.IsOpen);
        }

        public void CloseExitWindow()
        {
            exitWindow.Close();
            SetXRMenuPanelActive(!exitWindow.IsOpen);
        }

        void SetXRMenuPanelActive(bool state)
        {
            if (App.Instance && App.Instance.isXR)
                menuPanel.gameObject.SetActive(state);
        }
    }
}
