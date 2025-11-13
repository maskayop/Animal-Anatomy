using UnityEditor;
using UnityEditor.SceneManagement;

namespace AnimalAnatomy.Editor
{
	public class EditorMenuSceneLoader : EditorWindow
	{
		static void LoadScene(string sceneName)
		{
			string path = "Assets/Scenes/";

			if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
			{
				EditorSceneManager.OpenScene(path + sceneName + ".unity", OpenSceneMode.Single);
			}
		}

		[MenuItem("Animal Anatomy/Открыть сцену/Init")]
		static void LoadSceneInit()
		{
			LoadScene("_Init");
		}

        [MenuItem("Animal Anatomy/Открыть сцену/Main Menu")]
        static void LoadSceneMainMenu()
        {
            LoadScene("_Main Menu");
        }

        [MenuItem("Animal Anatomy/Открыть сцену/Protection")]
        static void LoadSceneProtection()
        {
            LoadScene("_Protection");
        }

        [MenuItem("Animal Anatomy/Открыть сцену/Животные/Корова")]
        static void LoadSceneCow()
        {
            LoadScene("Cow");
        }

        [MenuItem("Animal Anatomy/Открыть сцену/Животные/Конь")]
        static void LoadSceneHorse()
        {
            LoadScene("Horse");
        }

        [MenuItem("Animal Anatomy/Открыть сцену/Животные/Свинья")]
        static void LoadScenePig()
        {
            LoadScene("Pig");
        }

        [MenuItem("Animal Anatomy/Открыть сцену/Тест/Андроид")]
        static void LoadSceneAndroidTest()
        {
            LoadScene("Android Test");
        }
    }
}
