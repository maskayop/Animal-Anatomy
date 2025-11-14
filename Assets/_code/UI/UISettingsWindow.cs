using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vopere.Common;

namespace AnimalAnatomy
{
    public class UISettingsWindow : MonoBehaviour
    {
        public static UISettingsWindow Instance;

        [SerializeField] GameObject window;

        [Header("Graphics")]
        [SerializeField] List<Toggle> graphicsLevelToggles = new List<Toggle>();
        [SerializeField] List<Toggle> screenResolutionToggles = new List<Toggle>();

        [Header("Color Scheme")]
        [SerializeField] List<Toggle> backgroundColorSchemeToggles = new List<Toggle>();

        [Header("Audio")]
        [SerializeField] Slider musicSlider;
        [SerializeField] TextMeshProUGUI musicValueText;
        [SerializeField] Slider UIAudioSlider;
        [SerializeField] TextMeshProUGUI UIAudioValueText;

        [Header("Sensitivity")]
        [SerializeField] Slider rotationSensitivitySlider;
        [SerializeField] TextMeshProUGUI rotationSensitivityValueText;
        [SerializeField] Slider zoomSensitivitySlider;
        [SerializeField] TextMeshProUGUI zoomSensitivityValueText;

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

            Init();
        }

        public void Init()
        {
            int graphicsLeveId = DataSaveLoad.Instance.GetSavedInt("GraphicsLevel");

            if (graphicsLeveId != -1)
                graphicsLevelToggles[graphicsLeveId].isOn = true;

            int screenResolution = DataSaveLoad.Instance.GetSavedInt("ScreenResolution");

            if (screenResolution != -1)
                screenResolutionToggles[screenResolution].isOn = true;

            int backgroundColorSchemeId = DataSaveLoad.Instance.GetSavedInt("BackgroundColorScheme");

            if (backgroundColorSchemeId != -1)
                backgroundColorSchemeToggles[backgroundColorSchemeId].isOn = true;

            SetSliderLoadedValue("MusicVolume", musicSlider, musicValueText, 100);
            SetSliderLoadedValue("UIVolume", UIAudioSlider, UIAudioValueText, 100);

            SetSliderLoadedValue("RotationSensitivity", rotationSensitivitySlider, rotationSensitivityValueText, 5);
            SetSliderLoadedValue("ZoomSensitivity", zoomSensitivitySlider, zoomSensitivityValueText, 7);

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

        void SetSliderLoadedValue(string key, Slider slider, TextMeshProUGUI valueText, float defaultValue)
        {
            float value = DataSaveLoad.Instance.GetSavedFloat(key);

            if (value != -1)
                slider.value = value;
            else
                slider.value = defaultValue;

            valueText.text = slider.value.ToString();
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

        public void ChangeMusicVolume()
        {
            musicValueText.text = musicSlider.value.ToString();

            if (AudioController.Instance)
                AudioController.Instance.ChangeVolume(0, musicSlider.value);
        }

        public void ChangeUIVolume()
        {
            UIAudioValueText.text = UIAudioSlider.value.ToString();

            if (AudioController.Instance)
                AudioController.Instance.ChangeVolume(1, UIAudioSlider.value);
        }

        public void ChangeSFXVolume()
        {
            if (AudioController.Instance)
                AudioController.Instance.ChangeVolume(1, UIAudioSlider.value);
        }

        public void ChangeVoiceVolume()
        {
            if (AudioController.Instance)
                AudioController.Instance.ChangeVolume(1, UIAudioSlider.value);
        }

        public void ChangeGraphicsLevel(int id)
        {
            SetGraphicsLevel(id);
            DataSaveLoad.Instance.Save("GraphicsLevel", id);
        }

        void SetGraphicsLevel(int id)
        {
            if (App.Instance)
                App.Instance.SetGraphicsLevel(id);
        }

        public void ChangeResolutionLevel(int id)
        {
            SetResolutionLevel(id);
            DataSaveLoad.Instance.Save("ScreenResolution", id);
        }

        void SetResolutionLevel(int id)
        {
            if (App.Instance)
                App.Instance.SetResolution(id);
        }

        public void ChangeRotationSensitivity()
        {
            rotationSensitivityValueText.text = rotationSensitivitySlider.value.ToString();

            if (CameraController.Instance)
                CameraController.Instance.ChangeRotationSensitivity(rotationSensitivitySlider.value);
        }

        public void ChangeZoomSensitivity()
        {
            zoomSensitivityValueText.text = zoomSensitivitySlider.value.ToString();

            if (CameraController.Instance)
                CameraController.Instance.ChangeZoomSensitivity(zoomSensitivitySlider.value);
        }
    }
}
