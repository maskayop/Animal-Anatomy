using UnityEngine;

namespace AnimalAnatomy
{
    public class UISettingsWindow : MonoBehaviour
    {
        public static UISettingsWindow Instance;

        bool isOpen = false;
        public bool IsOpen { get { return isOpen; } set { isOpen = value; } }

        void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("Cannot create UISettingsWindow");
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        void Start()
        {
            Init();
        }

        public void Init()
        {

        }

        public void ChangeBackgroundColorScheme(int id)
        {
            LightController.Instance.SetSkyboxColors(id);
        }
    }
}