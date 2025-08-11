using UnityEngine;
using UnityEngine.SceneManagement;

namespace AnimalAnatomy
{
	public class ScenesManager : MonoBehaviour
	{
		public static ScenesManager Instance { get; private set; }

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

		public void LoadScene(string name)
		{
			SceneManager.LoadScene(name, LoadSceneMode.Single);
		}

		public void LoadSceneAdditive(string name)
		{
			SceneManager.LoadScene(name, LoadSceneMode.Additive);
			currentLoadedScene = name;
		}

		public void UnloadScene(string name)
		{
			SceneManager.UnloadSceneAsync(name);
		}

		public void UnloadCurrentLoadedScene()
		{
			UnloadScene(currentLoadedScene);
		}

		public string GetCurrentLoadedSceneName()
		{
			return currentLoadedScene;
		}
	}
}
