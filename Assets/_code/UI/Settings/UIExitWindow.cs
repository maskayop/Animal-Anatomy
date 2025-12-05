using UnityEngine;

namespace AnimalAnatomy
{
    public class UIExitWindow : UI_SettingsWindowBase
    {
        [SerializeField] string mainMenuSceneName;

        public void ExitToMainMenu()
        {
            if (scenesManager && scenesManager.IsSceneAddedToBuild(mainMenuSceneName))
                scenesManager.LoadSceneByName(mainMenuSceneName);
            else if (app)
                app.ExitGame();
        }
    }
}
