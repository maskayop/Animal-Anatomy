using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Vopere.Common;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace AnimalAnatomy
{
    public class CameraController : MonoBehaviour
    {
        public static CameraController Instance;

        public Camera mainCamera;

        [Header("Rotation")]
        [SerializeField] float rotationSpeed = 5f;
        [SerializeField] float defaultFOV = 60;
        [SerializeField] Vector2 verticalRotationLimits = new Vector2(-89f, 89f);

        [Header("Zoom")]
        //Минимальное, базовое, максимальное расстояние камеры
        public Vector3 cameraDistanceLimits = new Vector3(1f, 5f, 10f);
        public float scrollSpeed = 1.0f;
        public float doubleTouchZoomSpeed = 1.0f;
        public bool useDistanceLimitsMultiplier = true;
        public float distanceLimitsMultiplier = 1.0f;

        Vector2 lastMousePosition;
        float xRotation = 0f;

        public float currentZoom;
        Vector3 defaultPosition;

        bool isFreezed = false;

#if ENABLE_INPUT_SYSTEM
        Mouse currentMouse = Mouse.current;
#endif

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
            EnhancedTouchSupport.Enable();

            Vector3 currentRotation = transform.localEulerAngles;

            // Нормализуем угол в диапазон [-180, 180]
            xRotation = currentRotation.x;

            if (xRotation > 180)
                xRotation -= 360;
        }

        void Update()
        {
            if (UIMainCanvas.Instance && UIMainCanvas.Instance.isLoading)
                return;

            if (isFreezed)
                return;

#if ENABLE_INPUT_SYSTEM
            currentMouse = Mouse.current;
#endif

            UpdateViewRotation();
            UpdateViewZoom();

#if PLATFORM_ANDROID
            HandleZoom();
#endif
        }

        public void Init()
        {
            currentZoom = cameraDistanceLimits.y;
            mainCamera.transform.localPosition = new Vector3(0, 0, cameraDistanceLimits.y);
            defaultPosition = transform.position;

            float rotationSensitivity = DataSaveLoad.Instance.GetSavedFloat("RotationSensitivity");

            if (rotationSensitivity != -1)
                ChangeRotationSensitivity(rotationSensitivity);

            float zoomSensitivity = DataSaveLoad.Instance.GetSavedFloat("ZoomSensitivity");

            if (zoomSensitivity != -1)
                ChangeZoomSensitivity(zoomSensitivity);

            SetDefaultCameraFOV();
            EnableAudioListener(true);
        }

        void UpdateViewRotation()
        {
            if (LightController.Instance.lightRotationMode)
                return;

#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetMouseButton(0))
            {
                Vector3 currentMousePosition = Input.mousePosition;

                if (Input.GetMouseButtonDown(0))
                {
                    lastMousePosition = currentMousePosition;
                    return;
                }
#endif

#if ENABLE_INPUT_SYSTEM
            if (currentMouse == null)
                return;

            if (currentMouse.leftButton.isPressed)
            {
                Vector3 currentMousePosition = currentMouse.position.ReadValue();

                if (currentMouse.leftButton.wasPressedThisFrame)
                {
                    lastMousePosition = currentMousePosition;
                    return;
                }
#endif

                // Вычисляем разницу движения мыши по осям X и Y
                float mouseDeltaX = -(currentMousePosition.x - lastMousePosition.x) * rotationSpeed * Time.deltaTime;
                float mouseDeltaY = (currentMousePosition.y - lastMousePosition.y) * rotationSpeed * Time.deltaTime;

                // Обновляем угол поворота по оси X (вертикальное вращение)
                xRotation -= mouseDeltaY; // Минус для интуитивного направления
                xRotation = Mathf.Clamp(xRotation, verticalRotationLimits.x, verticalRotationLimits.y);

                // Применяем вращение: по оси Y — горизонтально, по оси X — вертикально
                transform.rotation = Quaternion.Euler(xRotation, transform.eulerAngles.y - mouseDeltaX, 0);

                lastMousePosition = currentMousePosition;
            }

#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetMouseButton(2))
                UpdatePosition();
#endif

#if ENABLE_INPUT_SYSTEM
            if (currentMouse.middleButton.isPressed)
                UpdatePosition();
#endif
        }

        void UpdateViewZoom()
        {
#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
                currentZoom -= scrollSpeed;
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
                currentZoom += scrollSpeed;
#endif

#if ENABLE_INPUT_SYSTEM
            if (currentMouse == null)
                return;

            float scrollDelta = currentMouse.scroll.ReadValue().y;

            if (scrollDelta > 0f)
                currentZoom -= scrollSpeed;
            else if (scrollDelta < 0f)
                currentZoom += scrollSpeed;
#endif

            currentZoom = Mathf.Clamp(currentZoom, cameraDistanceLimits.x * distanceLimitsMultiplier, cameraDistanceLimits.z * distanceLimitsMultiplier);

            mainCamera.transform.localPosition = new Vector3(0, 0, -currentZoom);
        }

        void HandleZoom()
        {
#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.touchCount == 2)
            {
                Touch touch1 = Input.GetTouch(0);
                Touch touch2 = Input.GetTouch(1);

                Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;
                Vector2 touch2PrevPos = touch2.position - touch2.deltaPosition;

                // Расстояние между пальцами в прошлом кадре
                float prevDistance = Vector2.Distance(touch1PrevPos, touch2PrevPos);
                // Расстояние между пальцами в текущем кадре
                float currentDistance = Vector2.Distance(touch1.position, touch2.position);
#endif

#if ENABLE_INPUT_SYSTEM
            if (Touch.activeTouches.Count == 2)
            {
                Touch touch1 = Touch.activeTouches[0];
                Touch touch2 = Touch.activeTouches[1];

                Vector2 touch1PrevPos = touch1.screenPosition - touch1.delta;
                Vector2 touch2PrevPos = touch2.screenPosition - touch2.delta;

                // Расстояние между пальцами в прошлом кадре
                float prevDistance = Vector2.Distance(touch1PrevPos, touch2PrevPos);
                // Расстояние между пальцами в текущем кадре
                float currentDistance = Vector2.Distance(touch1.screenPosition, touch2.screenPosition);
#endif

                float difference = currentDistance - prevDistance;
                currentZoom -= difference * doubleTouchZoomSpeed * scrollSpeed;
            }
        }

        public void UpdatePosition()
        {
            if (ExaminationController.Instance)
                if (ExaminationController.Instance.isExamination)
                    return;

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
            if (GameController.Instance)
                transform.position = GameController.Instance.selectedBodyPartsGroup.GetCenterOfGroup();

            if (transform.position == Vector3.zero)
                transform.position = defaultPosition;
        }

        public void Freeze(bool state)
        {
            isFreezed = state;
        }

        public void ChangeRotationSensitivity(float INvalue)
        {
            rotationSpeed = INvalue;
            DataSaveLoad.Instance.Save("RotationSensitivity", INvalue);
        }

        public void ChangeZoomSensitivity(float INvalue)
        {
            scrollSpeed = INvalue / 10;
            DataSaveLoad.Instance.Save("ZoomSensitivity", INvalue);
        }

        public void SetDefaultCameraFOV()
        {
            mainCamera.fieldOfView = defaultFOV;
        }

        void EnableAudioListener(bool state)
        {
            if (mainCamera.GetComponent<AudioListener>())
                mainCamera.GetComponent<AudioListener>().enabled = state;
        }
        public float GetCameraZoom()
        {
            return currentZoom;
        }

        public float GetNormalizedCameraZoom()
        {
            float divivder = cameraDistanceLimits.y - cameraDistanceLimits.x;

            if (divivder == 0)
                divivder = 1;

            float value = (currentZoom - cameraDistanceLimits.x) / divivder;

            return value;
        }

        public void SetCameraDistanceLimitsMultiplier(float value)
        {
            if (useDistanceLimitsMultiplier)
                distanceLimitsMultiplier = value;
            else
                distanceLimitsMultiplier = 1;
        }
    }
}
