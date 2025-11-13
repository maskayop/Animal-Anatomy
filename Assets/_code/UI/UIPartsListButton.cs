using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static AnimalAnatomy.GameController;

namespace AnimalAnatomy
{
    public class UIPartsListButton : MonoBehaviour
    {
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI scientificNameText;
        public RectTransform containerTransform;
        public GameObject selectionImage;
        public Image systemTypeColorImage;

        BodyPartInfo bodyPartInfo;

        [HideInInspector]
        public string textToCopy;

        void Start()
        {
            SetAsSelected(false);
        }

        public void Init(BodyPartInfo info)
        {
            bodyPartInfo = info;
            nameText.text = info.GetFullRussianName();
            scientificNameText.text = info.GetFullScientificName();
            systemTypeColorImage.color = ColorsManager.Instance.GetSystemColor(info.systemType);
        }

        public void Init(SystemType systemType)
        {
            string name = Enum.GetName(typeof(SystemType), systemType);
            nameText.text = "<b>- " + name + " -</b>";
            Destroy(scientificNameText.gameObject);
        }

        public void SelectBodyPart()
        {
            if (!bodyPartInfo)
                return;
            
            GameController.Instance.SelectBodyPart(bodyPartInfo);
            CameraController.Instance.UpdatePosition();
            SetAsSelected(true);

            CopyTextsToClipboard();
        }

        public void UnSelectBodyPart()
        {
            SetAsSelected(false);
        }

        public virtual void SetAsSelected(bool state)
        {
            if (selectionImage)
                selectionImage.SetActive(state);
        }

        void CopyTextsToClipboard()
        {
            #if !PLATFORM_ANDROID

            textToCopy = "Название части тела: " + bodyPartInfo.GetFullRussianName() + "\n" + "Латинское название: " + bodyPartInfo.GetFullScientificName() +
                "\n" + "Описание: " + bodyPartInfo.info.description;

            GUIUtility.systemCopyBuffer = textToCopy;
            
            #endif
        }
    }
}
