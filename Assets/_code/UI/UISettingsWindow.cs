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

        [Header("Audio")]
        [SerializeField] Slider musicSlider;
        [SerializeField] TextMeshProUGUI musicValueText;
        [SerializeField] Slider UIAudioSlider;
        [SerializeField] TextMeshProUGUI UIAudioValueText;

        [Header("Color Scheme")]
        [SerializeField] List<Toggle> backgroundColorSchemeToggles = new List<Toggle>();
        
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
            float musicVolume = DataSaveLoad.Instance.GetSavedFloat("MusicVolume");

            if (musicVolume != -1)
                musicSlider.value = musicVolume;
            else
                musicSlider.value = 100;
            
            musicValueText.text = musicSlider.value.ToString();

            float UIVolume = DataSaveLoad.Instance.GetSavedFloat("UIVolume");

            if (UIVolume != -1)
                UIAudioSlider.value = UIVolume;
            else
                UIAudioSlider.value = 100;
            
            UIAudioValueText.text = UIAudioSlider.value.ToString();

            int backgroundColorSchemeId = DataSaveLoad.Instance.GetSavedInt("BackgroundColorScheme");

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
    }
}
