using System;
using System.Collections.Generic;
using UnityEngine;

namespace AnimalAnatomy
{
    public class InfoConstructor : MonoBehaviour
    {
        List<GameObject> children = new List<GameObject>();
        InfoConstructorHelper infoConstructorHelper;
        LODGroup LODComponent;

        void Reset()
        {
            if (!GetComponent<InfoConstructorHelper>())
                infoConstructorHelper = gameObject.AddComponent<InfoConstructorHelper>();

            gameObject.layer = infoConstructorHelper.layer;

            foreach (Transform child in transform)
                children.Add(child.gameObject);            

            if (!GetComponent<MeshCollider>())
            {
                for (int i = 0; i < children.Count; i++)
                {
                    string text = children[i].name;
                    string[] words = text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    for (int w = 0; w < words.Length; w++)
                    {
                        if (words[w] == "low")
                        {
                            gameObject.AddComponent<MeshCollider>();
                            gameObject.GetComponent<MeshCollider>().sharedMesh = children[i].GetComponent<MeshFilter>().sharedMesh;
                        }                    
                    }
                }
            }

            if (infoConstructorHelper.material)
            {
                foreach (GameObject child in children)
                {
                    if (child.GetComponent<MeshRenderer>())
                        child.GetComponent<MeshRenderer>().material = infoConstructorHelper.material;
                }
            }

            if (!GetComponent<LODGroup>())
                LODComponent = gameObject.AddComponent<LODGroup>();

            if (!GetComponent<BodyPartInfo>())
                gameObject.AddComponent<BodyPartInfo>();
        }
    }
}
