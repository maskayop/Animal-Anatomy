using UnityEngine;

namespace AnimalAnatomy
{
    public class GlobalMaterialsManager : MonoBehaviour
    {
        public static GlobalMaterialsManager Instance;

        [Header("Materials")]
        public Material selectedMaterial;
        public Material selectedGroupMaterial;
        public Material transparentModeMaterial;
        public Material examModeCorrectMaterial;
        public Material examModeWrongMaterial;

        void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("Cannot create GlobalMaterialsManager");
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }
    }
}
