using System.IO;
using UnityEngine;

namespace AnimalAnatomy
{
    public class BodyPartsDescriptionsSaver : MonoBehaviour
    {
#if UNITY_EDITOR
        public static BodyPartsDescriptionsSaver Instance;

        [SerializeField] string filePath;

        [Header("Info")]
        public BodyPartGroup[] allBodyPartsGroups;
        public BodyPartInfo[] allBodyPartsInfos;

        void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("Cannot create BodyPartsDescriptionsSaver");
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
            allBodyPartsGroups = FindObjectsByType<BodyPartGroup>(FindObjectsSortMode.None);
            allBodyPartsInfos = FindObjectsByType<BodyPartInfo>(FindObjectsSortMode.None);
        }

        [ContextMenu("Save To File")]
        public void SaveToFile()
        {
            try
            {
                File.AppendAllText(filePath, "------------------------------------" + "\n");
                File.AppendAllText(filePath, "Группы" + "\n");
                File.AppendAllText(filePath, "------------------------------------" + "\n");

                for (int i = 0; i < allBodyPartsGroups.Length; i++)
                {
                    File.AppendAllText(filePath, i + 1 + "\n");
                    File.AppendAllText(filePath, allBodyPartsGroups[i].info.russianName + "\n");
                    File.AppendAllText(filePath, allBodyPartsGroups[i].info.scientificName + "\n");
                    File.AppendAllText(filePath, allBodyPartsGroups[i].info.description + "\n");
                    File.AppendAllText(filePath, "------------------------------------" + "\n");
                }

                File.AppendAllText(filePath, "------------------------------------" + "\n");
                File.AppendAllText(filePath, "Части тела" + "\n");
                File.AppendAllText(filePath, "------------------------------------" + "\n");

                for (int i = 0; i < allBodyPartsInfos.Length; i++)
                {
                    File.AppendAllText(filePath, i + 1 + "\n");
                    File.AppendAllText(filePath, allBodyPartsInfos[i].info.russianName + "\n");
                    File.AppendAllText(filePath, allBodyPartsInfos[i].info.scientificName + "\n");
                    File.AppendAllText(filePath, allBodyPartsInfos[i].info.description + "\n");
                    File.AppendAllText(filePath, "------------------------------------" + "\n");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Не удалось записать данные: {e.Message}");
            }
        }
#endif
    }
}
