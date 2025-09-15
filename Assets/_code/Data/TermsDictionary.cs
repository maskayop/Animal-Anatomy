using System;
using System.Collections.Generic;
using UnityEngine;

namespace AnimalAnatomy
{
    [Serializable]
    public class TermVariation
    {
        public string russianName;
        public string scientificName;
    }

    [Serializable]
    public class Term
    {
        public string name;
        public List<TermVariation> termVariations = new List<TermVariation>();
    }

    public class TermsDictionary : MonoBehaviour
    {
        public static TermsDictionary Instance;

        public List<Term> terms = new List<Term>();

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
