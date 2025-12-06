using UnityEngine;

namespace AnimalAnatomy
{
    public class UIStartGamePanel : MonoBehaviour
    {
        public void StartGame()
        {
            if (UIMainMenuCanvas.Instance)
                UIMainMenuCanvas.Instance.StartGame();

            if (XR_MainMenu.Instance)
                XR_MainMenu.Instance.StartGame();
        }
    }
}
