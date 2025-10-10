using System.Collections.Generic;
using UnityEngine;

namespace AnimalAnatomy
{
    [RequireComponent(typeof(MaterialToReplace))]
    public class AddMaterials : MonoBehaviour
    {
        public MaterialToReplace materialToReplace;

        List<MeshRenderer> children = new List<MeshRenderer>();

        void Reset()
        {
            materialToReplace = GetComponent<MaterialToReplace>();

            if (!materialToReplace)
                return;

            foreach (Transform child in transform)
            {
                if(child.GetComponent<MeshRenderer>())
                    children.Add(child.GetComponent<MeshRenderer>());
            }

            for (int i = 0; i < children.Count; i++)
            {
                children[i].material = materialToReplace.material;
            }                
        }
    }
}
