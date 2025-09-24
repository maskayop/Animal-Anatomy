using UnityEngine;

namespace AnimalAnatomy
{
    public class UIPartsGroupListButton : UIPartsListButton
    {
        [SerializeField] GameObject expandIcon;
        [SerializeField] GameObject collapseIcon;

        public bool isCollapsed = true;

        BodyPartGroup bodyPartGroup;

        void Start()
        {
            SetAsCollapsed(true);
        }

        public void Init(BodyPartGroup info)
        {
            bodyPartGroup = info;
            nameText.text = info.GetFullRussianName();
            scientificNameText.text = info.GetFullScientificName();
            systemTypeColorImage.color = ColorsManager.Instance.GetSystemColor(info.systemType);
        }

        public void SelectBodyPartGroup()
        {
            if (!bodyPartGroup)
                return;

            GameController.Instance.SelectBodyPartGroup(bodyPartGroup);
            CameraController.Instance.UpdatePosition();
            SetAsSelected(true);

            CopyTextsToClipboard();
        }

        public void UnSelectBodyPartGroup()
        {
            SetAsSelected(false);
        }

        public void SwitchCollapseState()
        {
            SetAsCollapsed(!isCollapsed);
        }

        public void SetAsCollapsed(bool state)
        {
            if (!expandIcon || !collapseIcon)
                return;

            expandIcon.SetActive(state);
            collapseIcon.SetActive(!state);

            if (state)
                Collapse();
            else
                Expand();
        }

        void Collapse()
        {
            isCollapsed = true;
            HideContent();
        }

        void Expand()
        {
            isCollapsed = false;
            ShowContent();
        }

        public void HideContent()
        {
            for (int i = 0; i < bodyPartGroup.bodyPartsGroups.Count; i++)
            {
                bodyPartGroup.bodyPartsGroups[i].partGroupListButton.gameObject.SetActive(false);
                bodyPartGroup.bodyPartsGroups[i].partGroupListButton.HideContent();
            }

            for (int i = 0; i < bodyPartGroup.bodyParts.Count;i++)
            {
                bodyPartGroup.bodyParts[i].partListButton.gameObject.SetActive(false);
            }
        }

        public void ShowContent()
        {
            for (int i = 0; i < bodyPartGroup.bodyPartsGroups.Count; i++)
            {
                bodyPartGroup.bodyPartsGroups[i].partGroupListButton.gameObject.SetActive(true);

                if (bodyPartGroup.bodyPartsGroups[i].partGroupListButton.isCollapsed)
                    bodyPartGroup.bodyPartsGroups[i].partGroupListButton.HideContent();
                else
                    bodyPartGroup.bodyPartsGroups[i].partGroupListButton.ShowContent();

                for (int p = 0; p < bodyPartGroup.bodyPartsGroups[i].bodyParts.Count; p++)
                {
                    if (bodyPartGroup.bodyPartsGroups[i].partGroupListButton.isCollapsed)
                        bodyPartGroup.bodyPartsGroups[i].bodyParts[p].partListButton.gameObject.SetActive(false);
                    else
                        bodyPartGroup.bodyPartsGroups[i].bodyParts[p].partListButton.gameObject.SetActive(true);
                }
            }

            for (int i = 0; i < bodyPartGroup.bodyParts.Count; i++)
            {
                bodyPartGroup.bodyParts[i].partListButton.gameObject.SetActive(true);
            }
        }

        void CopyTextsToClipboard()
        {
            textToCopy = "Название группы: " + bodyPartGroup.GetFullRussianName() + "\n" + "Латинское название: " + bodyPartGroup.GetFullScientificName() +
                "\n" + "Описание: " + bodyPartGroup.info.description;

            GUIUtility.systemCopyBuffer = textToCopy;
        }
    }
}
