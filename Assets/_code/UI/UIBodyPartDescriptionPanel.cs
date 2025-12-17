using UnityEngine;
using TMPro;

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

            if (bodyPartInfo.info.clip != null)
                audioDescriptionPlayerPanel.SetCurrentDescriptionAudio(bodyPartInfo.info.clip);
            else
                audioDescriptionPlayerPanel.SetCurrentDescriptionAudio(null);
        }

        public void ShowInfo(BodyPartGroup bodyPartGroupInfo)
        {
            bodyPartNameText.text = bodyPartGroupInfo.GetFullRussianName();
            bodyPartScientificNameText.text = bodyPartGroupInfo.GetFullScientificName();
            bodyPartDescriptionText.text = bodyPartGroupInfo.info.description;

            if (bodyPartGroupInfo.info.clip != null)
                audioDescriptionPlayerPanel.SetCurrentDescriptionAudio(bodyPartGroupInfo.info.clip);
            else
                audioDescriptionPlayerPanel.SetCurrentDescriptionAudio(null);
        }

        public void CollapseBodyPartDescription(bool state)
        {
            bodyPartDescriptionText.gameObject.SetActive(!state);
        }
    }
}
