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

        void Start()
        {
            Init();
        }

        public void Init()
        {
            if (bodyPartNameText)
                bodyPartNameText.text = "";

            if (bodyPartScientificNameText)
                bodyPartScientificNameText.text = "";

            if (bodyPartDescriptionText)
                bodyPartDescriptionText.text = "";
        }

        public void ShowInfo(BodyPartInfo bodyPartInfo)
        {
            if (!bodyPartNameText || !bodyPartScientificNameText) return;

            bodyPartNameText.text = bodyPartInfo.GetFullRussianName();
            bodyPartScientificNameText.text = bodyPartInfo.GetFullScientificName();

            if (!bodyPartInfo.info) return;

            if (bodyPartDescriptionText)
                bodyPartDescriptionText.text = bodyPartInfo.info.description;

            if (audioDescriptionPlayerPanel)
                audioDescriptionPlayerPanel.SetCurrentDescriptionAudio(bodyPartInfo.info.nameClip, bodyPartInfo.info.clip);
        }

        public void ShowInfo(BodyPartGroup bodyPartGroupInfo)
        {
            if (!bodyPartNameText || !bodyPartScientificNameText) return;

            bodyPartNameText.text = bodyPartGroupInfo.GetFullRussianName();
            bodyPartScientificNameText.text = bodyPartGroupInfo.GetFullScientificName();

            if (!bodyPartGroupInfo.info)
                return;

            bodyPartDescriptionText.text = bodyPartGroupInfo.info.description;
            audioDescriptionPlayerPanel.SetCurrentDescriptionAudio(bodyPartGroupInfo.info.nameClip, bodyPartGroupInfo.info.clip);
        }

        public void CollapseBodyPartDescription(bool state)
        {
            bodyPartDescriptionText.gameObject.SetActive(!state);
        }
    }
}
