using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using Vopere.Common;

namespace AnimalAnatomy
{
    public class XR_Helper : MonoBehaviour
    {
        GameController gameController;
        XRGrabInteractable grabInteractable;
        BodyPartInfo bodyPartInfo;

        void Start()
        {
            if (!App.Instance.isXR)
            {
                Destroy(this);
                return;
            }
        }

        public void Init()
        {
            gameController = GameController.Instance;

            grabInteractable = GetComponent<XRGrabInteractable>();

            if (grabInteractable)
                grabInteractable.selectEntered.AddListener(OnSelectEntered);

            if (grabInteractable)
                grabInteractable.selectExited.AddListener(OnSelectExited);

            bodyPartInfo = GetComponent<BodyPartInfo>();

            if (bodyPartInfo && grabInteractable)
                grabInteractable.attachTransform = bodyPartInfo.GetCenterTransform();
        }

        void OnSelectEntered(SelectEnterEventArgs args)
        {
            if (!grabInteractable || !bodyPartInfo)
                return;

            if (ExaminationController.Instance.isExamination)
                return;

            gameController.SelectBodyPart(bodyPartInfo);
        }

        void OnSelectExited(SelectExitEventArgs args)
        {
            if (!grabInteractable || !bodyPartInfo)
                return;

            if (ExaminationController.Instance.isExamination)
                return;

            if (gameController.exclusionMode)
                gameController.HideSelectedBodyPart();
        }

        void OnDestroy()
        {
            if (grabInteractable)
                grabInteractable.selectEntered.RemoveListener(OnSelectEntered);
        }
    }
}
