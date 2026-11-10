using Vopere.UI;

namespace AnimalAnatomy
{
    public class UIGameSelectionButton : UISelectionButtonBase
    {
        public string gameName;
        public string sceneName;

        UIMainMenuCanvas mainMenuCanvas;

        protected override void OnInit()
        {
            base.OnInit();
            mainMenuCanvas = UIMainMenuCanvas.Instance;
        }

        public void SelectGame()
        {
            mainMenuCanvas?.SelectGameBySceneName(sceneName);
        }
    }
}
