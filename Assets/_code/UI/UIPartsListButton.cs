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

        public void SelectBodyPart()
        {
            GameController.Instance.SelectBodyPart(bodyPartInfo);
            CameraController.Instance.UpdatePosition();
        }
    }
}
