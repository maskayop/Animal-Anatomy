using AnimalAnatomy;
using UnityEditor;
using UnityEngine;

namespace Vopere.Editor
{
    public class EditorMenuUtilites : EditorWindow
    {
        static void DeletePlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
        }

        [MenuItem("Utilites/Удалить Player Prefs")]
        static void DeletePlayerPrefsButton()
        {
            DeletePlayerPrefs();
        }

        [MenuItem("Utilites/Добавить конструктор части тела")]
        static void AddInfoConstructorButton()
        {
            foreach (GameObject selectedObject in Selection.gameObjects)
            {
                if (selectedObject.GetComponent<InfoConstructor>() == null)
                    selectedObject.AddComponent<InfoConstructor>();
            }
        }
    }
}
