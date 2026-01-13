using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Vopere.Common;

namespace AnimalAnatomy
{
    [Serializable]
    public class UIGameSelection
    {
        public string sceneName;
    }

    public class UIMainMenuCanvas : MonoBehaviour
    {
        public static UIMainMenuCanvas Instance;

        [SerializeField] GameObject startButtonPanel;

        [Header("Loading Screen")]
        [SerializeField] GameObject loadingScreen;
        [SerializeField] float loadingTime = 1.0f;

        [Header("Version")]
        [SerializeField] TextMeshProUGUI versionText;

        [Header("Debug")]
        [SerializeField] TextMeshProUGUI debugText;

        [Space(20)]
        [SerializeField] List<UIGameSelection> games = new List<UIGameSelection>();

        [HideInInspector]
        public bool isLoading = false;

        int currentGame = 0;

        void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("Cannot create UIMainMenuCanvas");
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        void Start()
        {
            loadingScreen.SetActive(true);
            isLoading = true;

            Init();

            StartCoroutine(DisableLoadingScreen());
        }

        void Update()
        {
            if (debugText.gameObject.activeInHierarchy)
                debugText.text = DataSaveLoad.Instance.GetSavedInt("DefaultScreenResolutionWidth") +
                    " x " + DataSaveLoad.Instance.GetSavedInt("DefaultScreenResolutionHeight");

        }

        public void Init()
        {
            SelectGame(-1);

            if (versionText != null)
                versionText.text = Application.version;
        }

        IEnumerator DisableLoadingScreen()
        {
#if UNITY_EDITOR
            loadingTime /= 2;
#endif

            yield return new WaitForSeconds(loadingTime);

            loadingScreen.SetActive(false);
            isLoading = false;
        }

        public void SelectGame(int id)
        {
            currentGame = id;

            startButtonPanel.SetActive(true);

            if (currentGame < 0)
            {
                startButtonPanel.SetActive(false);
                return;
            }
            else
                MainMenuAnimalSelection.Instance.SelectAnimal(id);

            if (!IsGameSceneAddedToBuild(id))
                startButtonPanel.SetActive(false);
        }

        public void StartGame()
        {
            ScenesManager.Instance.LoadSceneByName(games[currentGame].sceneName);
        }

        public void StartGameByName(string gameName)
        {
            ScenesManager.Instance.LoadSceneByName(gameName);
        }

        public void ExitGame()
        {
            App.Instance.ExitGame();
        }

        bool IsGameSceneAddedToBuild(int sceneId)
        {
            return ScenesManager.Instance.IsSceneAddedToBuild(games[sceneId].sceneName);
        }
    }
}
