using System.Collections;
using UnityEngine;
using UnityEngine.Video;

namespace AnimalAnatomy
{
    public class Initialization : MonoBehaviour
    {
        public static Initialization Instance;

        [SerializeField] string sceneToLoadFirst;

        [Header("Start Video")]
        [SerializeField] VideoPlayer videoPlayer;
        [SerializeField] Animator videoPlayerAnimator;
        [SerializeField] string videoFadeOutState;
        [SerializeField] float fadeOutAnimationLenght = 1.0f;

        float videoLength;

        void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("Cannot create Initialization");
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
            videoLength = (float)videoPlayer.clip.length;
            StartCoroutine(StartAnimationAfterVideo());
        }

        public void LoadGameScene()
        {
            ScenesManager.Instance.LoadSceneByName(sceneToLoadFirst);
        }

        IEnumerator StartAnimationAfterVideo()
        {
            yield return new WaitForSeconds(videoLength - fadeOutAnimationLenght);

            videoPlayerAnimator.Play(videoFadeOutState);

            StartCoroutine(StartGameAfterAnimation());
        }

        IEnumerator StartGameAfterAnimation()
        {
            yield return new WaitForSeconds(fadeOutAnimationLenght * 2);

            LoadGameScene();
        }
    }
}
