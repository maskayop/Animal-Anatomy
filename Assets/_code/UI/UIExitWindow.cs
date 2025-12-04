using UnityEngine;
using Vopere.Common;

namespace AnimalAnatomy
{
    public class UIExitWindow : MonoBehaviour
    {
        [SerializeField] string mainMenuSceneName;

        [HideInInspector]
        public UIMenuWindow menuWindow;

        void Start()
        {
            Init();
        }

        public void Init()
        {

        }

        public void Open()
        {
            menuWindow.OpenExitWindow();
        }

        public void Close()
        {
            menuWindow.CloseExitWindow();
        }

        public void ExitToMainMenu()
        {
            if (ScenesManager.Instance.IsSceneAddedToBuild(mainMenuSceneName))
                ScenesManager.Instance.LoadSceneByName(mainMenuSceneName);
            else
                App.Instance.ExitGame();
        }
    }
}
