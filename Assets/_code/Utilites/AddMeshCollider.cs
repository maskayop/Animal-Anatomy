using System;
using System.Collections.Generic;
using UnityEngine;

namespace AnimalAnatomy
{
    public class AddMeshCollider : MonoBehaviour
    {
        List<GameObject> children = new List<GameObject>();

        void Reset()
        {
            if (gameObject.GetComponent<MeshCollider>())
                return;

            foreach (Transform child in transform)
                children.Add(child.gameObject);

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
    }
}
