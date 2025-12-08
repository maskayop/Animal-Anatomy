using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using Vopere.Common;

namespace AnimalAnatomy
{
    public class XRInitialization : MonoBehaviour
    {
        public static XRInitialization Instance;

        [SerializeField] string sceneToLoadFirst;

        [Header("Start Video")]
        [SerializeField] VideoPlayer videoPlayer;
        [SerializeField] Animator videoPlayerAnimator;
        [SerializeField] string videoFadeOutState;
        [SerializeField] float fadeOutAnimationLenght = 1.0f;
        [SerializeField] float editorVideoSpeed = 1.0f;

        float videoLength;

        void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("Cannot create XRInitialization");
                Destroy(gameObject);
                return;
            }

            Instance = this;

            Debug.Log("\n" + "--- Инициализация ---");
        }

        void Start()
        {
            Init();
        }

        public void Init()
        {
            if (App.Instance && !App.Instance.isXR)
                return;

            videoLength = (float)videoPlayer.clip.length;

#if UNITY_EDITOR
            videoPlayer.playbackSpeed = editorVideoSpeed;
            videoLength /= editorVideoSpeed;
#else
            videoPlayer.playbackSpeed = 1.0f;
#endif

            videoPlayer.Play();
            StartCoroutine(StartAnimationAfterVideo());

            Debug.Log("Запуск программы");
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
