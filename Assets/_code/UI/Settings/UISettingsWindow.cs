using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AnimalAnatomy
{
    public class UISettingsWindow : UI_SettingsWindowBase
    {
        [Header("Screen Resolution")]
        [SerializeField] GameObject screenResolutionContainer;
        [SerializeField] List<Toggle> screenResolutionToggles = new List<Toggle>();
        [SerializeField] List<TextMeshProUGUI> screenResolutionTexts = new List<TextMeshProUGUI>();

        [Header("Graphics Level")]
        [SerializeField] List<Toggle> graphicsLevelToggles = new List<Toggle>();

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

        [Header("XR Sensitivity")]
        [SerializeField] Slider XR_RotationSensitivitySlider;
        [SerializeField] TextMeshProUGUI XR_RotationSensitivityValueText;
        [SerializeField] Slider XR_ScalingSensitivitySlider;
        [SerializeField] TextMeshProUGUI XR_ScalingSensitivityValueText;

        protected override void OnInit()
        {
            SetTogglesLoadedValue("GraphicsLevel", graphicsLevelToggles);
            SetTogglesLoadedValue("BackgroundColorScheme", backgroundColorSchemeToggles);

            SetSliderLoadedValue("MusicVolume", musicSlider, musicValueText, 100);
            SetSliderLoadedValue("UIVolume", UIAudioSlider, UIAudioValueText, 100);

            SetSliderLoadedValue("RotationSensitivity", rotationSensitivitySlider, rotationSensitivityValueText, 5);
            SetSliderLoadedValue("ZoomSensitivity", zoomSensitivitySlider, zoomSensitivityValueText, 7);

            SetSliderLoadedValue("XR_RotationSensitivity", XR_RotationSensitivitySlider, XR_RotationSensitivityValueText, 5);
            SetSliderLoadedValue("XR_ScalingSensitivity", XR_ScalingSensitivitySlider, XR_ScalingSensitivityValueText, 7);

            SetScreenResolutionProperties();
        }

        protected override void OnOpen()
        {
            OnInit();
        }

        void SetSliderLoadedValue(string key, Slider slider, TextMeshProUGUI valueText, float defaultValue)
        {
            float value = dataSaveLoad.GetSavedFloat(key);

            if (value != -1)
                slider.value = value;
            else
                slider.value = defaultValue;

            valueText.text = slider.value.ToString();
        }

        void SetTogglesLoadedValue(string key, List<Toggle> togglesList)
        {
            int id = dataSaveLoad.GetSavedInt(key);

            if (id != -1 && id < togglesList.Count)
            {
                for (int i = 0; i < togglesList.Count; i++)
                {
                    if (i == id)
                        togglesList[id].isOn = true;
                    else
                        togglesList[id].isOn = false;
                }
            }
        }

        public void ChangeBackgroundColorScheme(int id)
        {
            SetBackgroundColorScheme(id);
            dataSaveLoad.Save("BackgroundColorScheme", id);
        }

        void SetBackgroundColorScheme(int id)
        {
            if (lightController)
                lightController.SetSkyboxColors(id);
        }

        public void ChangeMusicVolume()
        {
            musicValueText.text = musicSlider.value.ToString();

            if (audioController)
                audioController.ChangeVolume(0, musicSlider.value);
        }

        public void ChangeUIVolume()
        {
            UIAudioValueText.text = UIAudioSlider.value.ToString();

            if (audioController)
                audioController.ChangeVolume(1, UIAudioSlider.value);
        }

        public void ChangeSFXVolume()
        {
            if (audioController)
                audioController.ChangeVolume(1, UIAudioSlider.value);
        }

        public void ChangeVoiceVolume()
        {
            if (audioController)
                audioController.ChangeVolume(1, UIAudioSlider.value);
        }

        public void ChangeGraphicsLevel(int id)
        {
            SetGraphicsLevel(id);
            dataSaveLoad.Save("GraphicsLevel", id);
        }

        void SetGraphicsLevel(int id)
        {
            if (app)
                app.SetGraphicsLevel(id);
        }

        public void ChangeResolutionLevel(int id)
        {
            SetResolutionLevel(id);
            dataSaveLoad.Save("ScreenResolution", id);
        }

        void SetResolutionLevel(int id)
        {
            if (app)
                app.SetResolution(id);
        }

        void SetScreenResolutionProperties()
        {
            SetTogglesLoadedValue("ScreenResolution", screenResolutionToggles);

            Vector2Int defaultScreenResolution = app.GetDefaultScreenResolution();

            for (int i = 0; i < screenResolutionTexts.Count; i++)
            {
                if (i == 0)
                    screenResolutionTexts[i].text = defaultScreenResolution.x * 3 / 8 + " x " + defaultScreenResolution.y * 3 / 8;
                else if (i == 1)
                    screenResolutionTexts[i].text = defaultScreenResolution.x / 2 + " x " + defaultScreenResolution.y / 2;
                else if (i == 2)
                    screenResolutionTexts[i].text = defaultScreenResolution.x * 3 / 4 + " x " + defaultScreenResolution.y * 3 / 4;
                else
                    screenResolutionTexts[i].text = defaultScreenResolution.x + " x " + defaultScreenResolution.y;
            }
        }

        public void ChangeRotationSensitivity()
        {
            rotationSensitivityValueText.text = rotationSensitivitySlider.value.ToString();

            if (cameraController)
                cameraController.ChangeRotationSensitivity(rotationSensitivitySlider.value);
        }

        public void ChangeZoomSensitivity()
        {
            zoomSensitivityValueText.text = zoomSensitivitySlider.value.ToString();

            if (cameraController)
                cameraController.ChangeZoomSensitivity(zoomSensitivitySlider.value);
        }

        public void ChangeXRRotationSensitivity()
        {
            XR_RotationSensitivityValueText.text = XR_RotationSensitivitySlider.value.ToString();

            if (gameController)
                gameController.ChangeBaseObjectRotationAngle(XR_RotationSensitivitySlider.value);
        }

        public void ChangeXRScalingSensitivity()
        {
            XR_ScalingSensitivityValueText.text = XR_ScalingSensitivitySlider.value.ToString();

            if (gameController)
                gameController.ChangeBaseObjectScalingDelta(XR_ScalingSensitivitySlider.value);
        }
    }
}
