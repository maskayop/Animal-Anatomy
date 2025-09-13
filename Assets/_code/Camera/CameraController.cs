using UnityEngine;

namespace AnimalAnatomy
{
    public class CameraController : MonoBehaviour
    {
        public static CameraController Instance;

        public Camera mainCamera;

        [SerializeField] float rotationSpeed = 5f;

        [Header("Camera Zoom")]
        //ћинимальное, базовое, максимальное рассто€ние камеры
        public Vector3 cameraDistanceLimits = new Vector3(1f, 5f, 10f);
        public float scrollSpeed = 1.0f;
        public float distanceLimitsMultiplier = 1.0f;

        Vector2 lastMousePosition;
        float xRotation = 0f;

        float currentZoom;
        Vector3 defaultPosition;

        bool isFreezed = false;

        void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("Cannot create CameraController");
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        void Start()
        {
            Init();
        }

        void Update()
        {
            if (UIMainCanvas.Instance.isLoading)
                return;

            if (isFreezed)
                return;

            UpdateViewRotation();
            UpdateViewZoom();
        }

        public void Init()
        {
            currentZoom = cameraDistanceLimits.y;
            mainCamera.transform.localPosition = new Vector3(0, 0, cameraDistanceLimits.y);
            defaultPosition = transform.position;
        }

        void UpdateViewRotation()
        {
            if (Input.GetMouseButton(0))
            {
                Vector3 currentMousePosition = Input.mousePosition;

                if (Input.GetMouseButtonDown(0))
                {
                    lastMousePosition = currentMousePosition;
                    return;
                }

                // ¬ычисл€ем разницу движени€ мыши по ос€м X и Y
                float mouseDeltaX = - (currentMousePosition.x - lastMousePosition.x) * rotationSpeed * Time.deltaTime;
                float mouseDeltaY = (currentMousePosition.y - lastMousePosition.y) * rotationSpeed * Time.deltaTime;

                // ќбновл€ем угол поворота по оси X (вертикальное вращение)
                xRotation -= mouseDeltaY; // ћинус дл€ интуитивного направлени€
                xRotation = Mathf.Clamp(xRotation, -89f, 89f); // ќграничиваем угол от -89 до 89 градусов

                // ѕримен€ем вращение: по оси Y Ч горизонтально, по оси X Ч вертикально
                transform.rotation = Quaternion.Euler(xRotation, transform.eulerAngles.y - mouseDeltaX, 0);

                lastMousePosition = currentMousePosition;
            }
        }

        void UpdateViewZoom()
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
                currentZoom -= scrollSpeed;
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
                currentZoom += scrollSpeed;

            currentZoom = Mathf.Clamp(currentZoom, cameraDistanceLimits.x * distanceLimitsMultiplier, cameraDistanceLimits.z * distanceLimitsMultiplier);
            
            mainCamera.transform.localPosition = new Vector3(0, 0, -currentZoom);            
        }

        public void UpdatePosition()
        {
            if (GameController.Instance.selectedBodyPart != null)
                UpdatePositionOnBodyPart(GameController.Instance.selectedBodyPart);
            else if (GameController.Instance.selectedBodyPartsGroup != null)
                UpdatePositionOnBodyGroup(GameController.Instance.selectedBodyPartsGroup);
            else
                transform.position = defaultPosition;
        }

        public void UpdatePositionOnBodyPart(BodyPartInfo info)
        {
            transform.position = info.GetCenterOfObject();
        }

        public void UpdatePositionOnBodyGroup(BodyPartGroup info)
        {
            transform.position = GameController.Instance.selectedBodyPartsGroup.GetCenterOfGroup();
            
            if (transform.position == Vector3.zero)
                transform.position = defaultPosition;
        }

        public void Freeze(bool state)
        {
            isFreezed = state;
        }
    }
}
