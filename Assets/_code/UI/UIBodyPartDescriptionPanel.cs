using TMPro;
using UnityEngine;

namespace AnimalAnatomy
{
    public class UIBodyPartDescriptionPanel : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI bodyPartNameText;
        [SerializeField] TextMeshProUGUI bodyPartScientificNameText;
        [SerializeField] TextMeshProUGUI bodyPartDescriptionText;

        [Header("Audio")]
        [SerializeField] UIAudioDescriptionPlayerPanel audioDescriptionPlayerPanel;

        public void ShowInfo(BodyPartInfo bodyPartInfo)
        {
            bodyPartNameText.text = bodyPartInfo.GetFullRussianName();
            bodyPartScientificNameText.text = bodyPartInfo.GetFullScientificName();
            bodyPartDescriptionText.text = bodyPartInfo.info.description;

            audioDescriptionPlayerPanel.SetCurrentDescriptionAudio(bodyPartInfo.info.nameClip, bodyPartInfo.info.clip);
        }

        public void ShowInfo(BodyPartGroup bodyPartGroupInfo)
        {
            bodyPartNameText.text = bodyPartGroupInfo.GetFullRussianName();
            bodyPartScientificNameText.text = bodyPartGroupInfo.GetFullScientificName();
            bodyPartDescriptionText.text = bodyPartGroupInfo.info.description;

            audioDescriptionPlayerPanel.SetCurrentDescriptionAudio(bodyPartGroupInfo.info.nameClip, bodyPartGroupInfo.info.clip);
        }

        public void CollapseBodyPartDescription(bool state)
        {
            bodyPartDescriptionText.gameObject.SetActive(!state);
        }
    }
}
