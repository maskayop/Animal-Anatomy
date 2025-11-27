using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

namespace Vopere.Common
{
	public class ScenesManager : MonoBehaviour
	{
		public static ScenesManager Instance { get; private set; }

        [SerializeField] List<string> scenes = new List<string>();

		[Header("Info")]
		public string[] scenesInBuild;

        string currentLoadedScene;

		void Awake()
		{
			if (Instance != null)
			{
				Debug.LogWarning("Cannot create ScenesManager");
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
			GetAllScenesInBuild();
        }

        public string[] GetAllScenesInBuild()
        {
            int sceneCount = SceneManager.sceneCountInBuildSettings;
            scenesInBuild = new string[sceneCount];

            for (int i = 0; i < sceneCount; i++)
            {
                string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
                scenesInBuild[i] = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            }

            return scenesInBuild;
        }

        public bool IsSceneAddedToBuild(string sceneName)
		{
			bool value = false;

			for (int i = 0; i < scenesInBuild.Length; i++)
				if (scenesInBuild[i] == sceneName)
				{
					value = true;
					break;
				}

			return value;
		}

		public void LoadScene(string sceneName)
		{
			SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
		}

		public void LoadSceneAdditive(string sceneName)
		{
			SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
			currentLoadedScene = sceneName;
		}

		public void UnloadScene(string sceneName)
		{
			SceneManager.UnloadSceneAsync(sceneName);
		}

		public void UnloadCurrentLoadedScene()
		{
			UnloadScene(currentLoadedScene);
		}

		public string GetCurrentLoadedSceneName()
		{
			return currentLoadedScene;
		}

        public void LoadSceneByName(string sceneName)
        {
			for (int i = 0;  i < scenes.Count; i++)
				if (scenes[i] ==  sceneName)
					LoadScene(scenes[i]);
        }
    }
}
