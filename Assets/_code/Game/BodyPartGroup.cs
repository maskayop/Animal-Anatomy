using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimalAnatomy
{ 
    public class BodyPartGroup : MonoBehaviour
    {
        public GameController.SystemType systemType;

        public float cameraDistanceLimitsMultiplier = 1.0f;
        public List<BodyPartGroup> bodyPartsGroups = new List<BodyPartGroup>();
        public List<BodyPartInfo> bodyParts = new List<BodyPartInfo>();
        public List<BodyPartInfo> allChildrenBodyParts = new List<BodyPartInfo>();

        [Header("Info")]
        public Data_Info info;
        public Data_Info suffix;

        [HideInInspector]
        public UIPartsGroupListButton partGroupListButton;

        [HideInInspector]
        public BodyPartGroup parentBodyPartGroup;

        void Start()
        {
            Init();
        }

        public void Init()
        {
            if (!info)
                Debug.LogWarning("Нет data_info для " + name);

            if (info && !info.clip)
                Debug.LogWarning("Нет audio clip для " + name);

            foreach (Transform t in transform)
            {
                BodyPartInfo info = t.GetComponent<BodyPartInfo>();
                BodyPartGroup groupInfo = t.GetComponent<BodyPartGroup>();

                if (info)
                {
                    bodyParts.Add(info);
                    info.bodyPartGroup = this;
                }
                
                if (groupInfo)
                {
                    groupInfo.parentBodyPartGroup = this;
                    bodyPartsGroups.Add(groupInfo);
                }
            }

            GameController.Instance.allBodyPartsGroups.Add(this);

            StartCoroutine(InitializeDelayed());
        }

        IEnumerator InitializeDelayed()
        {
            yield return new WaitForSeconds(1.0f);

            SetAllChildrenPartInfos(allChildrenBodyParts);
        }

        public void SetAllChildrenPartInfos(List<BodyPartInfo> list)
        {
            for (int g = 0; g < bodyPartsGroups.Count; g++)
            {
                bodyPartsGroups[g].SetAllChildrenPartInfos(list);
            }

            for (int p = 0; p < bodyParts.Count; p++)
            {
                list.Add(bodyParts[p]);
            }
        }

        public void Select()
        {
            for (int g = 0; g < bodyPartsGroups.Count; g++)
                bodyPartsGroups[g].Select();

            for (int p = 0; p < bodyParts.Count; p++)
                bodyParts[p].Select(true);

            if (partGroupListButton)
                partGroupListButton.SetAsSelected(true);
        }

        public void UnSelect()
        {
            for (int g = 0; g < bodyPartsGroups.Count; g++)
                bodyPartsGroups[g].UnSelect();

            for (int p = 0; p < bodyParts.Count; p++)
                bodyParts[p].UnSelect();

            if (partGroupListButton)
                partGroupListButton.SetAsSelected(false);

            if (AudioController.Instance)
                AudioController.Instance.OnUnSelectBodyPart();
        }

        public Vector3 GetCenterOfGroup()
        {
            if (allChildrenBodyParts.Count == 0)
                return Vector3.zero;

            Vector3 position = Vector3.zero;

            for (int i = 0; i < allChildrenBodyParts.Count; i++)
                position += allChildrenBodyParts[i].GetCenterOfObject();

            position /= allChildrenBodyParts.Count;

            return position;
        }

        public string GetFullRussianName()
        {
            string suffixAdditional = "";

            if (suffix)
                suffixAdditional = " (" + suffix.russianName + ")";

            return info.russianName + suffixAdditional;
        }

        public string GetFullScientificName()
        {
            string suffixAdditional = "";

            if (suffix)
                suffixAdditional = " (" + suffix.scientificName + ")";

            return info.scientificName + suffixAdditional;
        }
    }
}
