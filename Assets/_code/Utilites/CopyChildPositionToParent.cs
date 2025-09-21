using System.Collections.Generic;
using UnityEngine;

namespace AnimalAnatomy
{
    public class CopyChildPositionToParent : MonoBehaviour
    {
        List<GameObject> children = new List<GameObject>();

        void Reset()
        {
            foreach (Transform child in transform)
                children.Add(child.gameObject);

            transform.position = children[0].transform.position;

            for (int i = 0; i < children.Count; i++)
                children[i].transform.localPosition = Vector3.zero;
        }
    }
}
