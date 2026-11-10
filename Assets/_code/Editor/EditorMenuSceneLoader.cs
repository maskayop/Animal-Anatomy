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
            string path = "Assets/Scenes Aviculture/";

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

        // Птицеводство

        [MenuItem("Animal Anatomy/Открыть сцену - Птицеводство/Init Aviculture")]
        static void LoadSceneAvicultureInit()
        {
            LoadScene("_Init Aviculture", true);
        }

        [MenuItem("Animal Anatomy/Открыть сцену - Птицеводство/Main Menu Aviculture")]
        static void LoadSceneAvicultureMainMenu()
        {
            LoadScene("_Main Menu Aviculture", true);
        }

        [MenuItem("Animal Anatomy/Открыть сцену - Птицеводство/Устройство птицефермы")]
        static void LoadSceneAviculturePoultryFarm()
        {
            LoadScene("Poultry Farm", true);
        }

        [MenuItem("Animal Anatomy/Открыть сцену - Птицеводство/Производство яиц")]
        static void LoadSceneAvicultureEggProduction()
        {
            LoadScene("Egg Production", true);
        }

        [MenuItem("Animal Anatomy/Открыть сцену - Птицеводство/Линия переработки птицы")]
        static void LoadSceneAviculturePoultryProcessingLine()
        {
            LoadScene("Poultry Processing Line", true);
        }

        [MenuItem("Animal Anatomy/Открыть сцену - Птицеводство/Технология инкубации")]
        static void LoadSceneAvicultureIncubationTechnology()
        {
            LoadScene("Incubation Technology", true);
        }

        [MenuItem("Animal Anatomy/Открыть сцену - Птицеводство/Теория птицеводства")]
        static void LoadSceneAvicultureTheoryPoultryFarming()
        {
            LoadScene("Theory Poultry Farming", true);
        }
    }
}
