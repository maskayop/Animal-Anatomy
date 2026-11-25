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

        [Header("Image")]
        [SerializeField] GameObject imageButton;

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

            if (imageButton)
                imageButton.SetActive(false);
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

            if (bodyPartInfo.info.images.Count != 0)
            {
                imageButton.SetActive(true);
                UIMainCanvas.Instance?.InitBodyPartImagesPanel(bodyPartInfo.info.images[0]);
            }
            else
            {
                imageButton.SetActive(false);
                UIMainCanvas.Instance?.CloseBodyPartImagesPanel();
            }
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

            if (bodyPartGroupInfo.info.images.Count != 0)
            {
                imageButton.SetActive(true);
                UIMainCanvas.Instance?.InitBodyPartImagesPanel(bodyPartGroupInfo.info.images[0]);
            }
            else
            {
                imageButton.SetActive(false);
                UIMainCanvas.Instance?.CloseBodyPartImagesPanel();
            }
        }

        public void CollapseBodyPartDescription(bool state)
        {
            bodyPartDescriptionText.gameObject.SetActive(!state);
        }

        public void OpenBodyPartImagesPanel()
        {
            UIMainCanvas.Instance?.OpenBodyPartImagesPanel();
        }
    }
}
