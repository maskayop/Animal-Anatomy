using System.Collections.Generic;
using UnityEngine;

namespace AnimalAnatomy
{ 
    public class BodyPartGroup : MonoBehaviour
    {
        public float cameraDistanceLimitsMultiplier = 1.0f;
        public List<BodyPartGroup> bodyPartsGroups = new List<BodyPartGroup>();
        public List<BodyPartInfo> bodyParts = new List<BodyPartInfo>();

        [Header("Texts")]
        public string groupName;
        public string groupScientificName;
        [TextArea(1, 20)]
        public string description;

        [HideInInspector]
        public UIPartsListButton partListButton;

        [HideInInspector]
        public BodyPartGroup parentBodyPartGroup;

        [HideInInspector]
        public BodyPartInfo allChildrenBodyParts;

        void Start()
        {
            Init();
        }

        public void Init()
        {
            foreach (Transform t in transform)
            {
                if (t.GetComponent<BodyPartInfo>())
                {
                    bodyParts.Add(t.GetComponent<BodyPartInfo>());
                    t.GetComponent<BodyPartInfo>().bodyPartGroup = this;
                }
                
                if (t.GetComponent<BodyPartGroup>())
                {
                    t.GetComponent<BodyPartGroup>().parentBodyPartGroup = this;
                    bodyPartsGroups.Add(t.GetComponent<BodyPartGroup>());
                }
            }

            GameController.Instance.allBodyPartsGroups.Add(this);


        }

        public void Select()
        {
            for (int g = 0; g < bodyPartsGroups.Count; g++)
            {
                bodyPartsGroups[g].Select();
            }

            for (int p = 0; p < bodyParts.Count; p++)
            {
                bodyParts[p].Select();
            }
        }

        public void UnSelect()
        {
            for (int g = 0; g < bodyPartsGroups.Count; g++)
            {
                bodyPartsGroups[g].UnSelect();
            }

            for (int p = 0; p < bodyParts.Count; p++)
            {
                bodyParts[p].UnSelect();
            }
        }

        public void SetAsTransparent(bool state)
        {
            for (int g = 0; g < bodyPartsGroups.Count; g++)
            {
                bodyPartsGroups[g].SetAsTransparent(state);
            }

            for (int p = 0; p < bodyParts.Count; p++)
            {
                bodyParts[p].SetAsTransparent(state);
            }
        }    
        
        public Vector3 GetCenterOfGroup()
        {
            if (bodyParts.Count == 0)
                return Vector3.zero;

            Vector3 position = Vector3.zero;

            for (int g = 0; g < bodyParts.Count; g++)
            {
                position += bodyParts[g].GetCenterOfObject();
            }

            position /= bodyParts.Count;

            return position;
        }
    }
}
