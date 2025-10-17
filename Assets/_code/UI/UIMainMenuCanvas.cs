using System;
using System.Collections.Generic;
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

        [Space(20)]
        [SerializeField] List<UIGameSelection> games = new List<UIGameSelection>();

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
            Init();
        }

        public void Init()
        {
            SelectGame(-1);
        }

        public void SelectGame(int id)
        {
            currentGame = id;

            if (currentGame < 0)
            {
                startButtonPanel.SetActive(false);
            }
            else
            {
                startButtonPanel.SetActive(true);
                MainMenuAnimalSelection.Instance.SelectAnimal(id);
            }
        }

        public void StartGame()
        {
            ScenesManager.Instance.LoadSceneByName(games[currentGame].sceneName);
        }

        public void ExitGame()
        {
            App.Instance.ExitGame();
        }
    }
}
