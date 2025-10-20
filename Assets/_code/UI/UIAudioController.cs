using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

namespace AnimalAnatomy
{
    public class UIAudioController : MonoBehaviour
    {
        public static UIAudioController Instance;

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

        void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("Cannot create UIAudioController");
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
        }

        public void OnPrevButtonClicked()
        {

        }

        public void OnPlayButtonClicked()
        {
            playButton.SetActive(false);
            pauseButton.SetActive(true);
        }

        public void OnStopButtonClicked()
        {

        }

        public void OnPauseButtonClicked()
        {
            playButton.SetActive(true);
            pauseButton.SetActive(false);
        }

        public void OnNextButtonClicked()
        {

        }

        public void OnRepeatButtonClicked()
        {
            repeatImage.SetActive(true);
            inTurnImage.SetActive(false);
            randomImage.SetActive(false);

            repeatButton.SetActive(false);
            inTurnButton.SetActive(true);
            randomButton.SetActive(false);
        }

        public void OnInTurnButtonClicked()
        {
            repeatImage.SetActive(false);
            inTurnImage.SetActive(true);
            randomImage.SetActive(false);

            repeatButton.SetActive(false);
            inTurnButton.SetActive(false);
            randomButton.SetActive(true);
        }

        public void OnRandomButtonClicked()
        {
            repeatImage.SetActive(false);
            inTurnImage.SetActive(false);
            randomImage.SetActive(true);

            repeatButton.SetActive(true);
            inTurnButton.SetActive(false);
            randomButton.SetActive(false);
        }
    }
}
