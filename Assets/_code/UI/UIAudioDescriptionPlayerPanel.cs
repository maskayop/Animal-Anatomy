using UnityEngine;

namespace AnimalAnatomy
{
    public class UIAudioDescriptionPlayerPanel : MonoBehaviour
    {
        [SerializeField] GameObject playButton;
        [SerializeField] GameObject stopButton;
        [SerializeField] GameObject pauseButton;

        void Start()
        {
            Init();
        }

        public void Init()
        {
            playButton.SetActive(true);
            stopButton.SetActive(false);
            pauseButton.SetActive(false);
        }

        public void OnPlayButtonClicked()
        {
            playButton.SetActive(false);
            pauseButton.SetActive(true);
            stopButton.SetActive(true);

            AudioController.Instance.PlayCurrentVoice();
        }

        public void OnStopButtonClicked()
        {
            playButton.SetActive(true);
            pauseButton.SetActive(false);
            stopButton.SetActive(false);

            AudioController.Instance.StopCurrentVoice();
        }

        public void OnPauseButtonClicked()
        {
            playButton.SetActive(true);
            pauseButton.SetActive(false);

            AudioController.Instance.PauseCurrentVoice();
        }

        public void SetCurrentDescriptionAudio(AudioClip currentClip)
        {
            AudioController.Instance.voiceSource.clip = currentClip;
            OnStopButtonClicked();
        }
    }
}
