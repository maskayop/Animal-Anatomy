using UnityEngine;

namespace AnimalAnatomy
{
    [CreateAssetMenu(fileName = "New Data Info", menuName = "Animal Anatomy/Data Info", order = 1)]
    public class Data_Info : ScriptableObject
    {
        [Header("Texts")]
        public string russianName;
        public string scientificName;
        [TextArea(1, 20)]
        public string description;

        public enum SagittalType { none, left, right };
        [Space(20)]
        public SagittalType sagittalType;
        public int sagittalTypeTermId;

        [Header("Audio")]
        public AudioClip clip;
    }
}
