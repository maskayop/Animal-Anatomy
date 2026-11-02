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

        static void LoadScene(string sceneName, bool schemes)
        {
            string path = "Assets/Scenes Interactive Schemes/";

            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene(path + sceneName + ".unity", OpenSceneMode.Single);
            }
        }

        // Анатомия

        [MenuItem("Animal Anatomy/Открыть сцену/Init")]
        static void LoadSceneInit()
        {
            LoadScene("_Init");
        }

        [MenuItem("Animal Anatomy/Открыть сцену/Init XR")]
        static void LoadSceneInitXR()
        {
            LoadScene("_Init XR");
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

        [MenuItem("Animal Anatomy/Открыть сцену/Животные/Пёс")]
        static void LoadSceneDog()
        {
            LoadScene("Dog");
        }

        [MenuItem("Animal Anatomy/Открыть сцену/Животные/Кот")]
        static void LoadSceneCat()
        {
            LoadScene("Cat");
        }

        [MenuItem("Animal Anatomy/Открыть сцену/Животные/Курица")]
        static void LoadSceneChicken()
        {
            LoadScene("Chicken");
        }

        // Интерактивные Схемы

        [MenuItem("Animal Anatomy/Открыть сцену - Схемы/Main Menu")]
        static void LoadSceneISMainMenu()
        {
            LoadScene("_Main Menu Interactive Schemes", true);
        }

        [MenuItem("Animal Anatomy/Открыть сцену - Схемы/Птицефабрика")]
        static void LoadSceneISPoultryFarm()
        {
            LoadScene("Poultry Farm", true);
        }

        [MenuItem("Animal Anatomy/Открыть сцену - Схемы/Производство яиц")]
        static void LoadSceneISEggProduction()
        {
            LoadScene("Egg Production", true);
        }

        [MenuItem("Animal Anatomy/Открыть сцену - Схемы/Линия переработки птицы")]
        static void LoadSceneISPoultryProcessingLine()
        {
            LoadScene("Poultry Processing Line", true);
        }

        [MenuItem("Animal Anatomy/Открыть сцену - Схемы/Технология инкубации")]
        static void LoadSceneISIncubationTechnology()
        {
            LoadScene("Incubation Technology", true);
        }

        [MenuItem("Animal Anatomy/Открыть сцену - Схемы/Теория птицеводства")]
        static void LoadSceneISTheoryPoultryFarming()
        {
            LoadScene("Theory Poultry Farming", true);
        }
    }
}
