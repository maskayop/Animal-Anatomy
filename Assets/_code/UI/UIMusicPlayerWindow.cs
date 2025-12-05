using TMPro;
using UnityEngine;

namespace AnimalAnatomy
{
    public class UIMusicPlayerWindow : MonoBehaviour
    {
        [Header("Panels")]
        [SerializeField] GameObject bigPanel;
        [SerializeField] GameObject smallPanel;        

        [Header("Indicators")]
        [SerializeField] TextMeshProUGUI musicNameText;
        [SerializeField] GameObject repeatImage;
        [SerializeField] GameObject inTurnImage;
        [SerializeField] GameObject randomImage;

        [Header("Buttons")]
        [SerializeField] GameObject prevButton;
        [SerializeField] GameObject playButton;
        [SerializeField] GameObject stopButton;
        [SerializeField] GameObject pauseButton;
        [SerializeField] GameObject nextButton;
        [SerializeField] GameObject repeatButton;
        [SerializeField] GameObject inTurnButton;
        [SerializeField] GameObject randomButton;

        AudioController controller;

        void Start()
        {
            Init();
        }

        void Update()
        {
            SetMusicName();
        }

        public void Init()
        {
            controller = AudioController.Instance;

            bigPanel.SetActive(false);
            smallPanel.SetActive(true);

            repeatImage.SetActive(false);
            inTurnImage.SetActive(true);
            randomImage.SetActive(false);

            prevButton.SetActive(true);
            playButton.SetActive(false);
            stopButton.SetActive(false);
            pauseButton.SetActive(true);
            nextButton.SetActive(true);
            repeatButton.SetActive(false);
            inTurnButton.SetActive(false);
            randomButton.SetActive(true);

            OnRandomButtonClicked();
        }

        public void OnPrevButtonClicked()
        {
            playButton.SetActive(false);
            pauseButton.SetActive(true);

            controller.PlayPrevMusicClip();
        }

        public void OnPlayButtonClicked()
        {
            playButton.SetActive(false);
            pauseButton.SetActive(true);

            controller.PlayCurrentMusic();
        }

        public void OnStopButtonClicked()
        {

        }

        public void OnPauseButtonClicked()
        {
            playButton.SetActive(true);
            pauseButton.SetActive(false);

            controller.PauseCurrentMusic();
        }

        public void OnNextButtonClicked()
        {
            playButton.SetActive(false);
            pauseButton.SetActive(true);

            controller.PlayNextMusicClip();
        }

        public void OnRepeatButtonClicked()
        {
            repeatImage.SetActive(true);
            inTurnImage.SetActive(false);
            randomImage.SetActive(false);

            repeatButton.SetActive(false);
            inTurnButton.SetActive(true);
            randomButton.SetActive(false);

            controller.SetMusicLoopPlaying(true);
        }

        public void OnInTurnButtonClicked()
        {
            repeatImage.SetActive(false);
            inTurnImage.SetActive(true);
            randomImage.SetActive(false);

            repeatButton.SetActive(false);
            inTurnButton.SetActive(false);
            randomButton.SetActive(true);

            controller.SetMusicLoopPlaying(false);
            controller.SetRandomPlaying(false);
        }

        public void OnRandomButtonClicked()
        {
            repeatImage.SetActive(false);
            inTurnImage.SetActive(false);
            randomImage.SetActive(true);

            repeatButton.SetActive(true);
            inTurnButton.SetActive(false);
            randomButton.SetActive(false);

            controller.SetMusicLoopPlaying(false);
            controller.SetRandomPlaying(true);
        }

        void SetMusicName()
        {
            if (!controller)
                return;

            musicNameText.text = controller.musicSamples[controller.GetCurrentMusicId()].name;
        }
    }
}
