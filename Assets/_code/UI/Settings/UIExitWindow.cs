using System.Collections.Generic;
using UnityEngine;

namespace AnimalAnatomy
{
    public class UIExitWindow : UI_SettingsWindowBase
    {
        [SerializeField] List<string> mainMenuSceneNames = new List<string>();

        public void ExitToMainMenu()
        {
            if (!scenesManager) return;

            for (int i = 0; i < mainMenuSceneNames.Count; i++)
            {
                if (scenesManager.IsSceneAddedToBuild(mainMenuSceneNames[i]))
                {
                    scenesManager.LoadSceneByName(mainMenuSceneNames[i]);
                    return;
                }
            }

            if (app)
                app.ExitGame();
        }
    }
}
