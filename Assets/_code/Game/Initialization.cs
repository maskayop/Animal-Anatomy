using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using Vopere.Common;
using Vopere.Protection;

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
        [SerializeField] float editorVideoSpeed = 1.0f;

        float videoLength;
        Protection protection;

        Vector2Int defaultScreenResolution = Vector2Int.zero;

        void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("Cannot create Initialization");
                Destroy(gameObject);
                return;
            }

            Instance = this;

            GetDefaultScreenResolution();

            Debug.Log("\n" + "--- Инициализация ---");
        }

        void Start()
        {
            protection = FindFirstObjectByType<Protection>();

            if (!protection)
                Init();
        }

        public void Init()
        {
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

        void GetDefaultScreenResolution()
        {
            defaultScreenResolution.x = DataSaveLoad.Instance.GetSavedInt("DefaultScreenResolutionWidth");

            if (defaultScreenResolution.x == -1)
            {
                defaultScreenResolution.x = Screen.width;
                DataSaveLoad.Instance.Save("DefaultScreenResolutionWidth", defaultScreenResolution.x);
            }

            defaultScreenResolution.y = DataSaveLoad.Instance.GetSavedInt("DefaultScreenResolutionHeight");

            if (defaultScreenResolution.y == -1)
            {
                defaultScreenResolution.y = Screen.height;
                DataSaveLoad.Instance.Save("DefaultScreenResolutionHeight", defaultScreenResolution.y);
            }
        }
    }
}
