using UnityEditor;
using UnityEditor.SceneManagement;

namespace Vopere.Editor
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

        [MenuItem("Animal Anatomy/Открыть сцену/Cow")]
        static void LoadSceneCow()
        {
            LoadScene("Cow");
        }
    }
}
