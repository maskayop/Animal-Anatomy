using System;
using TMPro;
using UnityEngine;
using static AnimalAnatomy.GameController;

namespace AnimalAnatomy
{
    public class UIPartsListButton : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI nameText;
        [SerializeField] TextMeshProUGUI scientificNameText;

        BodyPartInfo bodyPartInfo;
        BodyPartGroup bodyPartGroup;

        public void Init(BodyPartInfo info)
        {
            bodyPartInfo = info;
            nameText.text = info.partName;
            scientificNameText.text = info.partScientificName;
        }

        public void Init(SystemType systemType)
        {
            string name = Enum.GetName(typeof(SystemType), systemType);
            nameText.text = "<b>- " + name + " -</b>";
            Destroy(scientificNameText.gameObject);
        }

        public void Init(BodyPartGroup info)
        {
            bodyPartGroup = info;
            nameText.text = info.groupName;
            scientificNameText.text = info.groupScientificName;
        }

        public void SelectBodyPart()
        {
            if (!bodyPartInfo)
            {
                SelectBodyPartGroup();
                return;
            }
            
            GameController.Instance.SelectBodyPart(bodyPartInfo);
            CameraController.Instance.UpdatePosition();
        }

        public void SelectBodyPartGroup()
        {
            if (!bodyPartGroup)
                return;

            GameController.Instance.SelectBodyPartGroup(bodyPartGroup);
            CameraController.Instance.UpdatePosition();
        }
    }
}
