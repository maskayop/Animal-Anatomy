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

        AudioClip currentClip;

        void Start()
        {
            Init();
        }

        void Update()
        {
            if (AudioController.Instance.voiceSource.isPlaying)
            {
                slider.value = AudioController.Instance.voiceSource.time;
                currentTime = slider.value;
            }

            if (isPlaying)
            {
                currentTime += Time.deltaTime;

                if (currentTime >= currentClip.length)
                    OnStopButtonClicked();
            }
        }

        public void Init()
        {
            playButton.SetActive(true);
            stopButton.SetActive(false);
            pauseButton.SetActive(false);

            OnStopButtonClicked();
            slider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        public void OnPlayButtonClicked()
        {
            playButton.SetActive(false);
            pauseButton.SetActive(true);
            stopButton.SetActive(true);

            AudioController.Instance.PlayCurrentVoice();

            isPlaying = true;
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

        public void SetCurrentDescriptionAudio(AudioClip clip)
        {
            if (clip == null)
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
            
            AudioController.Instance.voiceSource.clip = clip;            
            slider.maxValue = clip.length;
            currentClip = clip;
        }

        public void OnSliderValueChanged(float value)
        {

        }
    }
}
