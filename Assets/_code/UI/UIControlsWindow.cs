using UnityEngine;

namespace AnimalAnatomy
{
    public class UIControlsWindow : MonoBehaviour
    {
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
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}
