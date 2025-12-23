using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

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

                if (GetComponent<MeshRenderer>())
                    gameObject.AddComponent<MeshCollider>();
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
            {
                LODComponent = gameObject.AddComponent<LODGroup>();

                LOD[] lods = new LOD[children.Count];

                if (children.Count == 1)
                {
                    Renderer[] newRenderers = new Renderer[1];
                    newRenderers[0] = children[0].GetComponent<Renderer>();
                    lods[0] = new LOD(0.001f, newRenderers);
                }
                else if (children.Count == 2)
                {
                    Renderer[] newRenderers1 = new Renderer[1];
                    newRenderers1[0] = children[0].GetComponent<Renderer>();
                    lods[0] = new LOD(0.5f, newRenderers1);
                    Renderer[] newRenderers2 = new Renderer[1];
                    newRenderers2[0] = children[1].GetComponent<Renderer>();
                    lods[1] = new LOD(0.001f, newRenderers2);
                }
                else if (children.Count == 3)
                {
                    Renderer[] newRenderers1 = new Renderer[1];
                    newRenderers1[0] = children[1].GetComponent<Renderer>();
                    lods[0] = new LOD(0.6f, newRenderers1);
                    Renderer[] newRenderers2 = new Renderer[1];
                    newRenderers2[0] = children[0].GetComponent<Renderer>();
                    lods[1] = new LOD(0.3f, newRenderers2);
                    Renderer[] newRenderers3 = new Renderer[1];
                    newRenderers3[0] = children[2].GetComponent<Renderer>();
                    lods[2] = new LOD(0.001f, newRenderers3);
                }
                else
                {
                    for (int i = 0; i < children.Count; i++)
                    {
                        Renderer[] newRenderers = new Renderer[1];
                        newRenderers[0] = children[i].GetComponent<Renderer>();

                        if (i == children.Count - 1)
                            lods[i] = new LOD(0.001f, newRenderers);
                        else
                            lods[i] = new LOD((1 - (float)i / (float)children.Count) * 0.5f, newRenderers);
                    }
                }

                if (GetComponent<MeshRenderer>())
                {
                    Renderer[] newRenderers = new Renderer[1];
                    newRenderers[0] = GetComponent<Renderer>();
                    lods = new LOD[1];
                    lods[0] = new LOD(0.001f, newRenderers);
                }
                
                LODComponent.SetLODs(lods);
            }

            if (!GetComponent<BodyPartInfo>())
                gameObject.AddComponent<BodyPartInfo>();

            if (!GetComponent<Rigidbody>())
            {
                Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
                rigidbody.isKinematic = true;
                rigidbody.useGravity = false;
            }

            if (!GetComponent<XRGrabInteractable>())
            {
                XRGrabInteractable grabInteractable = gameObject.AddComponent<XRGrabInteractable>();
                grabInteractable.trackPosition = false;
                grabInteractable.trackRotation = false;
                grabInteractable.trackScale = false;
                grabInteractable.throwOnDetach = false;
            }

            if (!GetComponent<XR_Helper>())
                gameObject.AddComponent<XR_Helper>();
        }
    }
}
