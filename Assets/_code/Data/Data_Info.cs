using UnityEngine;

namespace AnimalAnatomy
{
    [CreateAssetMenu(fileName = "New Data Info", menuName = "Animal Anatomy/Data Info", order = 1)]
    public class Data_Info : ScriptableObject
    {
        [Header("Texts")]
        public string russianName;
        public string scientificName;
        [TextArea(1, 50)]
        public string description;

        [Space(20)]
        public int sagittalTypeTermId;

        [Header("Audio")]
        public AudioClip clip;
    }
}
