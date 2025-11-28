using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace AnimalAnatomy
{
    public class XR_Helper : MonoBehaviour
    {
        GameController gameController;
        XRGrabInteractable grabInteractable;
        BodyPartInfo bodyPartInfo;

        void Start()
        {
            gameController = GameController.Instance;
            grabInteractable = GetComponent<XRGrabInteractable>();
            grabInteractable.selectEntered.AddListener(OnSelectEntered);
            bodyPartInfo = GetComponent<BodyPartInfo>();
        }

        void OnSelectEntered(SelectEnterEventArgs args)
        {
            if (!grabInteractable || !bodyPartInfo)
                return;

            gameController.SelectBodyPart(bodyPartInfo);

            if (gameController.exclusionMode)
                gameController.HideSelectedBodyPart();
        }

        void OnDestroy()
        {
            grabInteractable.selectEntered.RemoveListener(OnSelectEntered);
        }
    }
}
