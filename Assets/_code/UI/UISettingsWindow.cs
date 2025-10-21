using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vopere.Common;

namespace AnimalAnatomy
{
    public class UISettingsWindow : MonoBehaviour
    {
        public static UISettingsWindow Instance;

        [SerializeField] GameObject window;

        [Header("Audio")]

        [Header("Color Scheme")]
        [SerializeField] List<Toggle> backgroundColorSchemeToggles = new List<Toggle>();
        
        bool isOpen = false;
        public bool IsOpen { get { return isOpen; } set { isOpen = value; } }

        int backgroundColorSchemeId;

        void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("Cannot create UISettingsWindow");
                Destroy(gameObject);
                return;
            }

            Instance = this;

            Init();
        }

        public void Init()
        {
            backgroundColorSchemeId = DataSaveLoad.Instance.GetSavedInt("BackgroundColorScheme");

            if (backgroundColorSchemeId != -1)
                backgroundColorSchemeToggles[backgroundColorSchemeId].isOn = true;

            Close();
        }

        public void Open()
        {
            isOpen = true;
            window.SetActive(true);

            if (CameraController.Instance)
                CameraController.Instance.Freeze(true);
        }

        public void Close()
        {
            isOpen = false;
            window.SetActive(false);

            if (CameraController.Instance)
                CameraController.Instance.Freeze(false);
        }

        public void ChangeBackgroundColorScheme(int id)
        {
            SetBackgroundColorScheme(id);            
            DataSaveLoad.Instance.Save("BackgroundColorScheme", id);
        }

        void SetBackgroundColorScheme(int id)
        {
            if (LightController.Instance)
                LightController.Instance.SetSkyboxColors(id);
        }
    }
}
