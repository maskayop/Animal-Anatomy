using UnityEngine;
using TMPro;

namespace AnimalAnatomy
{
    public class UIBodyPartDescriptionPanel : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI bodyPartNameText;
        [SerializeField] TextMeshProUGUI bodyPartScientificNameText;
        [SerializeField] TextMeshProUGUI bodyPartDescriptionText;

        public void ShowInfo(BodyPartInfo bodyPartInfo)
        {
            bodyPartNameText.text = bodyPartInfo.GetFullRussianName();
            bodyPartScientificNameText.text = bodyPartInfo.GetFullScientificName();
            bodyPartDescriptionText.text = bodyPartInfo.info.description;
        }

        public void ShowInfo(BodyPartGroup bodyPartGroupInfo)
        {
            bodyPartNameText.text = bodyPartGroupInfo.GetFullRussianName();
            bodyPartScientificNameText.text = bodyPartGroupInfo.GetFullScientificName();
            bodyPartDescriptionText.text = bodyPartGroupInfo.info.description;
        }

        public void CollapseBodyPartDescription(bool state)
        {
            bodyPartDescriptionText.gameObject.SetActive(!state);
        }
    }
}
