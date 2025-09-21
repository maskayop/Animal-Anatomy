using System.Collections.Generic;
using UnityEngine;

namespace AnimalAnatomy
{
    public class BodyPartInfo : MonoBehaviour
    {
        public GameController.SystemType systemType;

        [SerializeField] List<MeshRenderer> meshRenderers = new List<MeshRenderer>();

        public float cameraDistanceLimitsMultiplier = 1.0f;

        [Header("Info")]
        public Data_Info info;
        public Data_Info suffixSaggital;
        public Data_Info suffix;

        [HideInInspector]
        public UIPartsListButton partListButton;

        [HideInInspector]
        public BodyPartGroup bodyPartGroup;

        Material defaultMaterial;

        void Reset()
        {
            foreach (Transform child in transform)
            {
                if (child.GetComponent<MeshRenderer>())
                    meshRenderers.Add(child.GetComponent<MeshRenderer>());
            }

            if (!GetComponent<LODGroup>())
                transform.gameObject.AddComponent<LODGroup>();
        }

        void Start()
        {
            Init();
        }

        public void Init()
        {
            GameController.Instance.allBodyParts.Add(this);

            if (!info)
                Debug.LogWarning("Нет data_info для " + name);

            if (meshRenderers.Count == 0)
            {
                Debug.LogWarning("Нет meshRenderers для " + name);

                if (GetComponent<MeshRenderer>())
                    meshRenderers.Add(GetComponent<MeshRenderer>());
            }
            
            if (meshRenderers.Count != 0)
                defaultMaterial = meshRenderers[0].material;

            CalculateCameraDistanceLimitsMultiplier();
        }

        public void Select(bool isGroupSelection)
        {
            for (int i = 0; i < meshRenderers.Count; i++)
            {
                Material[] materials = new Material[2];
                materials[0] = meshRenderers[i].materials[0];

                if (isGroupSelection)
                    materials[1] = GlobalMaterialsManager.Instance.selectedGroupMaterial;
                else
                    materials[1] = GlobalMaterialsManager.Instance.selectedMaterial;
                
                meshRenderers[i].materials = materials;
            }
            
            partListButton.SetAsSelected(true);
        }

        public void UnSelect()
        {
            for (int i = 0; i < meshRenderers.Count; i++)
            {
                Material[] materials = new Material[1];
                materials[0] = meshRenderers[i].materials[0];

                meshRenderers[i].materials = materials;
            }

            partListButton.SetAsSelected(false);
        }

        public void SetAsTransparent(bool state)
        {
            for (int i = 0; i < meshRenderers.Count; i++)
            {
                Material[] materials = new Material[1];

                if (state)
                    materials[0] = GlobalMaterialsManager.Instance.transparentModeMaterial;
                else
                    materials[0] = defaultMaterial;
                
                meshRenderers[i].materials = materials;
            }
        }

        public Vector3 GetCenterOfObject()
        {
            return meshRenderers[0].bounds.center;
        }

        public void SelectCorrect(bool isCorrect)
        {
            for (int i = 0; i < meshRenderers.Count; i++)
            {
                Material[] materials = new Material[2];
                materials[0] = meshRenderers[i].materials[0];

                if (isCorrect)
                    materials[1] = GlobalMaterialsManager.Instance.examModeCorrectMaterial;
                else
                    materials[1] = GlobalMaterialsManager.Instance.examModeWrongMaterial;

                meshRenderers[i].materials = materials;
            }
        }

        public string GetFullRussianName()
        {
            string saggital = "";

            if (suffixSaggital)
                saggital = " (" + suffixSaggital.russianName + ")";

            string suffixAdditional = "";

            if (suffix)
                suffixAdditional = " (" + suffix.russianName + ")";

            return info.russianName + suffixAdditional + saggital;
        }

        public string GetFullScientificName()
        {
            string saggital = "";

            if (suffixSaggital)
                saggital = " (" + suffixSaggital.scientificName + ")";

            string suffixAdditional = "";

            if (suffix)
                suffixAdditional = " (" + suffix.scientificName + ")";

            return info.scientificName + suffixAdditional + saggital;
        }

        void CalculateCameraDistanceLimitsMultiplier()
        {
            float[] floats = new float[3];
            floats[0] = meshRenderers[0].bounds.size.x;
            floats[1] = meshRenderers[0].bounds.size.y;
            floats[2] = meshRenderers[0].bounds.size.z;

            cameraDistanceLimitsMultiplier = Mathf.Clamp(Mathf.Pow(Mathf.Max(floats), 0.6f), 0.25f, 1.0f);
        }
    }
}
