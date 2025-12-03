using System.Collections.Generic;
using UnityEngine;

namespace AnimalAnatomy
{
    public class UIBodyPartsListPanel : MonoBehaviour
    {
        [SerializeField] Transform bodyPartsListContainer;
        [SerializeField] GameObject partsListButtonPrefab;
        [SerializeField] GameObject partsGroupListButtonPrefab;
        [SerializeField] int listButtonsContainerOffset = 5;

        List<UIPartsListButton> partsListButtons = new List<UIPartsListButton>();

        GameController gameController;
        UIMainCanvas mainCanvas;

        public void Init()
        {
            gameController = GameController.Instance;
            mainCanvas = UIMainCanvas.Instance;

            CreateBodyPartsGroupsListButtons();
        }

        public void Open()
        {
            if (mainCanvas)
                mainCanvas.OpenPartsListPanel();
        }

        public void Close()
        {
            if (mainCanvas)
                mainCanvas.ClosePartsListPanel();
        }

        public void FreezeCamera(bool state)
        {
#if PLATFORM_ANDROID
            return;
#else
            if (CameraController.Instance)
                CameraController.Instance.Freeze(state);
#endif
        }

        void CreateBodyPartsGroupsListButtons()
        {
            partsListButtons.Clear();

            foreach (Transform t in bodyPartsListContainer)
                Destroy(t.gameObject);

            if (!gameController.baseBodyPartGroup)
                return;

            CreateBodyPartsSingleGroupListButtons(gameController.baseBodyPartGroup);
            CreateBodyPartsListButtons(gameController.baseBodyPartGroup);
        }

        void CreateBodyPartsSingleGroupListButtons(BodyPartGroup group)
        {
            GameObject newGroupGO = Instantiate(partsGroupListButtonPrefab, bodyPartsListContainer);
            UIPartsGroupListButton groupListButton = newGroupGO.GetComponent<UIPartsGroupListButton>();
            groupListButton.Init(group);
            group.partGroupListButton = groupListButton;

            if (group.parentBodyPartGroup)
                groupListButton.containerTransform.offsetMin =
                    new Vector2(group.parentBodyPartGroup.partGroupListButton.containerTransform.offsetMin.x + listButtonsContainerOffset, 0);

            CreateBodyPartsGroupsListButtons(group.bodyPartsGroups);
        }

        void CreateBodyPartsGroupsListButtons(List<BodyPartGroup> groups)
        {
            for (int i = 0; i < groups.Count; i++)
            {
                CreateBodyPartsSingleGroupListButtons(groups[i]);
                CreateBodyPartsListButtons(groups[i]);
            }
        }

        void CreateBodyPartsListButtons(BodyPartGroup group)
        {
            if (group.bodyParts.Count != 0)
            {
                for (int b = 0; b < group.bodyParts.Count; b++)
                {
                    GameObject newGO = Instantiate(partsListButtonPrefab, bodyPartsListContainer);
                    UIPartsListButton listButton = newGO.GetComponent<UIPartsListButton>();
                    listButton.Init(group.bodyParts[b]);
                    group.bodyParts[b].partListButton = listButton;
                    listButton.containerTransform.offsetMin =
                        new Vector2(group.parentBodyPartGroup.partGroupListButton.containerTransform.offsetMin.x + listButtonsContainerOffset, 0);
                }
            }
        }
    }
}
