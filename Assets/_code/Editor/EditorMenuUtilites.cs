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
    }
}
