using UnityEngine;

namespace AnimalAnatomy
{
    public class UIAnimalSelectionButton : MonoBehaviour
    {
        public string gameName;
        public string sceneName;

        [SerializeField] GameObject selected;

        UIMainMenuCanvas mainMenuCanvas;

        void Start()
        {
            mainMenuCanvas = UIMainMenuCanvas.Instance;

            Init();   
        }

        public void Init()
        {
            Select(false);
        }

        public void Select(bool state)
        {
            selected.SetActive(state);
        }

        public void SelectGame()
        {
            if (!mainMenuCanvas)
                return;

            mainMenuCanvas.SelectGameBySceneName(sceneName);
        }
    }
}
