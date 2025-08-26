using UnityEngine;

namespace AnimalAnatomy
{
    public class BodyPartInfo : MonoBehaviour
    {
        public GameController.SystemType systemType;

        public string partName;
        public string partDescription;

        [HideInInspector]
        public MeshRenderer meshRenderer;

        void Start()
        {
            GameController.Instance.allBodyParts.Add(this);
            meshRenderer = GetComponent<MeshRenderer>();

            partName = gameObject.name;
            partDescription = gameObject.name + ": Description";
        }

        public void Select()
        {
            Material[] materials = new Material[2];
            materials[0] = meshRenderer.materials[0];
            materials[1] = GameController.Instance.selectedMaterial;

            meshRenderer.materials = materials;
        }

        public void UnSelect()
        {
            Material[] materials = new Material[1];
            materials[0] = meshRenderer.materials[0];

            meshRenderer.materials = materials;
        }
    }
}
