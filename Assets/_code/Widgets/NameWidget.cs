using TMPro;
using UnityEngine;

namespace AnimalAnatomy
{
    public class NameWidget : MonoBehaviour
    {
        [SerializeField] GameObject container;
        [SerializeField] TextMeshPro text;

        [Header("Transform")]
        [SerializeField] float minScale = 0;
        [SerializeField] bool useLookAt = false;

        [Header("Inverse")]
        [SerializeField] bool inverseScaling = false;
        [SerializeField] float scaleForDisabling = 1;

        [Header("Body Part")]
        [SerializeField] BodyPartInfo bodyPartInfo;
        [SerializeField] Color selectedTextColor = Color.white;
        [SerializeField] Color defaultTextColor = Color.white;

        CameraController cameraController;
        GameController gameController;

        void Start()
        {
            cameraController = CameraController.Instance;
            gameController = GameController.Instance;
        }

        void Update()
        {
            if (!cameraController)
                return;

            if (ExaminationController.Instance)
            {
                if (ExaminationController.Instance.isExamination)
                {
                    container?.SetActive(false);
                    return;
                }
                else
                    container?.SetActive(true);
            }

            if (transform.localScale.x < scaleForDisabling)
                container.SetActive(false);
            else
                container.SetActive(true);

            if (useLookAt)
                transform.LookAt(cameraController.mainCamera.transform.position);
            else
            {
                transform.rotation = cameraController.mainCamera.transform.rotation;
                transform.Rotate(180, 0, 180);
            }

            if (!inverseScaling)
                transform.localScale = Vector3.one * Mathf.Clamp(cameraController.GetNormalizedCameraZoom(), minScale, 1);
            else
                transform.localScale = Vector3.one * Mathf.Clamp(1 - cameraController.GetNormalizedCameraZoom(), 0, minScale);

            UpdateTextColor();
        }

        void UpdateTextColor()
        {
            if (!gameController)
                return;

            if (!bodyPartInfo)
                return;

            if (gameController.selectedBodyPart == bodyPartInfo)
                text.color = selectedTextColor;
            else
                text.color = defaultTextColor;
        }
    }
}
