using System.Collections.Generic;
using UnityEngine;
using Vopere.Common;

namespace AnimalAnatomy
{
    public class XR_MainMenu : MonoBehaviour
    {
        public static XR_MainMenu Instance;

        [SerializeField] List<GameObject> startButtonsPanels = new List<GameObject>();
        [SerializeField] List<UIGameSelection> games = new List<UIGameSelection>();

        public int currentGame = 0;

        void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("Cannot create XR_MainMenu");
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
                for (int i = 0; i < startButtonsPanels.Count; i++)
                    startButtonsPanels[i].SetActive(false);
            }
            else
            {
                for (int i = 0; i < startButtonsPanels.Count; i++)
                {
                    if (i == id)
                        startButtonsPanels[i].SetActive(true);
                    else
                        startButtonsPanels[i].SetActive(false);
                }

                MainMenuAnimalSelection.Instance.SelectAnimal(id);
            }
        }

        public void StartGame()
        {
            ScenesManager.Instance.LoadSceneByName(games[currentGame].sceneName);
        }
    }
}
