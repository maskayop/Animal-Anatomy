using UnityEngine;

namespace AnimalAnatomy
{
    public class BodyPartInfo : MonoBehaviour
    {
        public GameController.SystemType systemType;

        void Start()
        {
            GameController.Instance.allBodyParts.Add(this);
        }
    }
}
