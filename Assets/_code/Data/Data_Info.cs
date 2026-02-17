using UnityEngine;

namespace AnimalAnatomy
{
    [CreateAssetMenu(fileName = "New Data Info", menuName = "Animal Anatomy/Data Info", order = 1)]
    public class Data_Info : ScriptableObject
    {
        [Header("Name")]
        public string russianName;
        public string scientificName;

        [Space(10)]
        public AudioClip nameClip;

        [Header("Description")]
        [TextArea(1, 50)]
        public string description;

        [Space(10)]
        public AudioClip clip;
    }
}
