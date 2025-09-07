using System.Collections.Generic;
using UnityEngine;

namespace AnimalAnatomy
{
    public class BodyPartInfo : MonoBehaviour
    {
        public GameController.SystemType systemType;

        [SerializeField] List<MeshRenderer> meshRenderers = new List<MeshRenderer>();

        public float cameraDistanceLimitsMultiplier = 1.0f;

        [Header("Texts")]
        public string partName;
        public string partScientificName;
        [TextArea(1, 10)]
        public string partDescription;

        bool isHidden = false;
        public bool IsHidden {  get { return isHidden; } }

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

            if (partName == "")
                partName = gameObject.name;
            
            if (partDescription == "")
                partDescription = partName + ": Description";

            if (meshRenderers.Count == 0)
            {
                if (GetComponent<MeshRenderer>())
                    meshRenderers.Add(GetComponent<MeshRenderer>());
            }
            
            if (meshRenderers.Count != 0)
                defaultMaterial = meshRenderers[0].material;
        }

        public void Select()
        {
            for (int i = 0; i < meshRenderers.Count; i++)
            {
                Material[] materials = new Material[2];
                materials[0] = meshRenderers[i].materials[0];
                materials[1] = GameController.Instance.selectedMaterial;

                meshRenderers[i].materials = materials;
            }
        }

        public void UnSelect()
        {
            for (int i = 0; i < meshRenderers.Count; i++)
            {
                Material[] materials = new Material[1];
                materials[0] = meshRenderers[i].materials[0];

                meshRenderers[i].materials = materials;
            }
        }

        public void Hide(bool state)
        {
            isHidden = state;
        }

        public void SetAsTransparent(bool state)
        {
            for (int i = 0; i < meshRenderers.Count; i++)
            {
                Material[] materials = new Material[1];

                if (state)
                    materials[0] = GameController.Instance.transparentModeMaterial;
                else
                    materials[0] = defaultMaterial;
                
                meshRenderers[i].materials = materials;
            }
        }

        public Vector3 GetCenterOfObject()
        {
            return meshRenderers[0].bounds.center;
        }
    }
}
