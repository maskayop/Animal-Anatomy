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

        AudioController audioController;

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
            audioController = AudioController.Instance;

            playButton.SetActive(true);
            stopButton.SetActive(false);
            pauseButton.SetActive(false);

            OnStopButtonClicked();
        }

        public void OnPlayButtonClicked()
        {
            if (!audioController) return;

            playButton.SetActive(false);
            pauseButton.SetActive(true);
            stopButton.SetActive(true);

            isPlaying = true;

            if (currentTime > 0)
            {
                audioController.PlayCurrentVoice();
                return;
            }

            if (currentNameClip)
                PlayVoiceClip(currentNameClip);
            else if (currentClip)
                PlayVoiceClip(currentClip);
        }

        public void OnStopButtonClicked()
        {
            if (!audioController) return;

            if (playButton) playButton.SetActive(true);
            if (pauseButton) pauseButton.SetActive(false);
            if (stopButton) stopButton.SetActive(false);

            audioController.StopCurrentVoice();

            if (slider)
            {
                slider.minValue = 0;
                slider.value = 0;
            }

            currentTime = 0;
            isPlaying = false;
        }

        public void OnPauseButtonClicked()
        {
            if (!audioController) return;

            playButton.SetActive(true);
            pauseButton.SetActive(false);

            audioController.PauseCurrentVoice();

            isPlaying = false;
        }

        public void SetCurrentDescriptionAudio(AudioClip nameClip, AudioClip clip)
        {
            if (!nameClip && !clip)
            {
                if (audioPlayerPanel)
                    audioPlayerPanel.SetActive(false);

                if (slider)
                    slider.gameObject.SetActive(false);

                return;
            }

            OnStopButtonClicked();

            if (audioPlayerPanel)
                audioPlayerPanel.SetActive(true);

            if (!slider) return;

            slider.gameObject.SetActive(true);

            if (!nameClip && clip)
                slider.maxValue = clip.length;

            else if (nameClip && !clip)
                slider.maxValue = nameClip.length;
            else if (nameClip && clip)
                slider.maxValue = nameClip.length + clip.length;
            else
                slider.maxValue = 0;

            currentNameClip = nameClip;
            currentClip = clip;
        }

        void PlayVoiceClip(AudioClip INclip)
        {
            if (!audioController) return;

            if (audioController.voiceSource.clip == INclip)
                return;

            audioController.voiceSource.clip = INclip;
            audioController.PlayCurrentVoice();
        }
    }
}
