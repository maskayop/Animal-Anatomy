using UnityEngine;

namespace AnimalAnatomy
{
    public class GameplayAudioPlayer : MonoBehaviour
    {
        public static GameplayAudioPlayer Instance;

        [SerializeField] AudioClip bodyPartSelectionClip;
        [SerializeField] AudioClip bodyPartUnSelectionClip;

        AudioController audioController;

        void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("Cannot create GameplayAudioPlayer");
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
            audioController = AudioController.Instance;
        }

        public void PlayBodyPartSelectionAudio()
        {
            audioController.PlayUIAudioClip(bodyPartSelectionClip);
        }

        public void PlayBodyPartUnSelectionAudio()
        {
            audioController.PlayUIAudioClip(bodyPartUnSelectionClip);
        }
    }
}
