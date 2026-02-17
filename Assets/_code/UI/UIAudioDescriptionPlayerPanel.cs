using UnityEngine;
using UnityEngine.UI;

namespace AnimalAnatomy
{
    public class UIAudioDescriptionPlayerPanel : MonoBehaviour
    {
        [SerializeField] GameObject audioPlayerPanel;
        [SerializeField] GameObject playButton;
        [SerializeField] GameObject stopButton;
        [SerializeField] GameObject pauseButton;
        [SerializeField] Slider slider;

        float currentTime = 0;
        bool isPlaying = false;

        AudioClip currentNameClip;
        AudioClip currentClip;

        void Start()
        {
            Init();
        }

        void Update()
        {
            if (isPlaying)
            {
                currentTime += Time.deltaTime;
                slider.value = currentTime;

                if (currentNameClip && currentTime > currentNameClip.length)
                    PlayVoiceClip(currentClip);

                if (currentTime >= slider.maxValue)
                    OnStopButtonClicked();
            }
        }

        public void Init()
        {
            playButton.SetActive(true);
            stopButton.SetActive(false);
            pauseButton.SetActive(false);

            OnStopButtonClicked();
        }

        public void OnPlayButtonClicked()
        {
            playButton.SetActive(false);
            pauseButton.SetActive(true);
            stopButton.SetActive(true);

            isPlaying = true;

            if (currentTime > 0)
            {
                AudioController.Instance.PlayCurrentVoice();
                return;
            }

            if (currentNameClip)
                PlayVoiceClip(currentNameClip);
            else if (currentClip)
                PlayVoiceClip(currentClip);
        }

        public void OnStopButtonClicked()
        {
            playButton.SetActive(true);
            pauseButton.SetActive(false);
            stopButton.SetActive(false);

            AudioController.Instance.StopCurrentVoice();

            slider.minValue = 0;
            slider.value = 0;

            currentTime = 0;
            isPlaying = false;
        }

        public void OnPauseButtonClicked()
        {
            playButton.SetActive(true);
            pauseButton.SetActive(false);

            AudioController.Instance.PauseCurrentVoice();

            isPlaying = false;
        }

        public void SetCurrentDescriptionAudio(AudioClip nameClip, AudioClip clip)
        {
            if (!nameClip && !clip)
            {
                audioPlayerPanel.SetActive(false);
                slider.gameObject.SetActive(false);
                return;
            }
            else
            {
                OnStopButtonClicked();
                audioPlayerPanel.SetActive(true);
                slider.gameObject.SetActive(true);
            }

            if (!nameClip)
                slider.maxValue = clip.length;
            else if (!clip)
                slider.maxValue = nameClip.length;
            else
                slider.maxValue = nameClip.length + clip.length;

            currentNameClip = nameClip;
            currentClip = clip;
        }

        void PlayVoiceClip(AudioClip INclip)
        {
            if (AudioController.Instance.voiceSource.clip == INclip)
                return;

            AudioController.Instance.voiceSource.clip = INclip;
            AudioController.Instance.PlayCurrentVoice();
        }
    }
}
