using System.Collections.Generic;
using UnityEngine;

namespace AnimalAnatomy
{
    public class TermsDictionary : MonoBehaviour
    {
        public static TermsDictionary Instance;

        public List<Data_Info> data = new List<Data_Info>();

        void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("Cannot create TermsDictionary");
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }
    }
}
