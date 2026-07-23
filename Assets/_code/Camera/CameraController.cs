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
        public Vector3 cameraDistanceLimits = new Vector3(1f, 5f, 10f);
        public float scrollSpeed = 1.0f;
        public float doubleTouchZoomSpeed = 1.0f;
        public bool useDistanceLimitsMultiplier = true;
        public float distanceLimitsMultiplier = 1.0f;

        // --- Приватные переменные ---
        Vector2 lastMousePosition;
        Vector2 lastTouchPosition;
        float xRotation = 0f;
        public float currentZoom;
        Vector3 defaultPosition;
        bool isFreezed = false;

        // --- Кешируем устройства ввода ---
        Mouse currentMouse;

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
            // Включаем поддержку EnhancedTouch
            EnhancedTouchSupport.Enable();

            // Кешируем устройства ввода
            currentMouse = Mouse.current;

            // Нормализуем начальный угол
            Vector3 currentRotation = transform.localEulerAngles;
            xRotation = currentRotation.x;
            if (xRotation > 180)
                xRotation -= 360;

            Init();
        }

        void OnDestroy()
        {
            // Отключаем EnhancedTouch при выходе
            if (EnhancedTouchSupport.enabled)
                EnhancedTouchSupport.Disable();
        }

        void Update()
        {
            if (UIMainCanvas.Instance && UIMainCanvas.Instance.isLoading)
                return;

            if (isFreezed)
                return;

            UpdateViewRotation();
            UpdateViewZoom();
            HandleTouchZoom();
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

        // ==============================================
        //  ВРАЩЕНИЕ (ПК + Android)
        // ==============================================
        void UpdateViewRotation()
        {
            if (LightController.Instance.lightRotationMode)
                return;

            bool isDragging = false;
            Vector2 currentPosition = Vector2.zero;
            bool isTouch = false;

            // --- Проверяем ввод с мыши (ПК) ---
            if (currentMouse != null && currentMouse.leftButton.isPressed)
            {
                isDragging = true;
                currentPosition = currentMouse.position.ReadValue();
                isTouch = false;

                if (currentMouse.leftButton.wasPressedThisFrame)
                {
                    lastMousePosition = currentPosition;
                    return;
                }
            }

            // --- Проверяем ввод с тача (Android) ---
            if (Touch.activeTouches.Count == 1 && !isDragging)
            {
                Touch touch = Touch.activeTouches[0];

                // Игнорируем, если палец только начал касание
                if (touch.phase == UnityEngine.InputSystem.TouchPhase.Began)
                {
                    lastTouchPosition = touch.screenPosition;
                    return;
                }

                // Если палец двигается или стоит на месте
                if (touch.phase == UnityEngine.InputSystem.TouchPhase.Moved ||
                    touch.phase == UnityEngine.InputSystem.TouchPhase.Stationary)
                {
                    isDragging = true;
                    currentPosition = touch.screenPosition;
                    isTouch = true;

                    // Для тача используем lastTouchPosition
                    if (touch.phase == UnityEngine.InputSystem.TouchPhase.Began)
                    {
                        lastTouchPosition = currentPosition;
                        return;
                    }
                }
            }

            // --- Если ничего не нажато - выходим ---
            if (!isDragging)
                return;

            // --- Вычисляем дельту ---
            Vector2 delta;
            if (isTouch)
            {
                delta = currentPosition - lastTouchPosition;
                lastTouchPosition = currentPosition;
            }
            else
            {
                delta = currentPosition - lastMousePosition;
                lastMousePosition = currentPosition;
            }

            float mouseDeltaX = -delta.x * rotationSpeed * Time.deltaTime;
            float mouseDeltaY = delta.y * rotationSpeed * Time.deltaTime;

            xRotation -= mouseDeltaY;
            xRotation = Mathf.Clamp(xRotation, verticalRotationLimits.x, verticalRotationLimits.y);

            transform.rotation = Quaternion.Euler(xRotation, transform.eulerAngles.y - mouseDeltaX, 0);
        }

        // ==============================================
        //  ЗУМ (ПК + Android)
        // ==============================================
        void UpdateViewZoom()
        {
            // --- Зум колесиком мыши (ПК) ---
            if (currentMouse != null)
            {
                float scrollDelta = currentMouse.scroll.ReadValue().y;
                if (scrollDelta > 0f)
                    currentZoom -= scrollSpeed;
                else if (scrollDelta < 0f)
                    currentZoom += scrollSpeed;
            }

            // --- Зум двумя пальцами (Android) ---
            // Вынесен в отдельный метод HandleTouchZoom()

            // Ограничиваем зум
            currentZoom = Mathf.Clamp(
                currentZoom,
                cameraDistanceLimits.x * distanceLimitsMultiplier,
                cameraDistanceLimits.z * distanceLimitsMultiplier
            );

            mainCamera.transform.localPosition = new Vector3(0, 0, -currentZoom);
        }

        // ==============================================
        //  ЗУМ ДВУМЯ ПАЛЬЦАМИ (Android)
        // ==============================================
        void HandleTouchZoom()
        {
            if (Touch.activeTouches.Count != 2)
                return;

            Touch touch1 = Touch.activeTouches[0];
            Touch touch2 = Touch.activeTouches[1];

            // Предыдущие позиции
            Vector2 touch1PrevPos = touch1.screenPosition - touch1.delta;
            Vector2 touch2PrevPos = touch2.screenPosition - touch2.delta;

            // Расстояния
            float prevDistance = Vector2.Distance(touch1PrevPos, touch2PrevPos);
            float currentDistance = Vector2.Distance(touch1.screenPosition, touch2.screenPosition);

            // Разница
            float difference = currentDistance - prevDistance;
            currentZoom -= difference * doubleTouchZoomSpeed * scrollSpeed;
        }

        // ==============================================
        //  ПЕРЕМЕЩЕНИЕ К КОНКРЕТНОЙ ЧАСТИ
        // ==============================================
        public void UpdatePosition()
        {
            if (ExaminationController.Instance && ExaminationController.Instance.isExamination)
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

        // ==============================================
        //  УПРАВЛЕНИЕ СОСТОЯНИЕМ
        // ==============================================
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
            float divider = cameraDistanceLimits.y - cameraDistanceLimits.x;
            if (divider == 0)
                divider = 1;

            return (currentZoom - cameraDistanceLimits.x) / divider;
        }

        public void SetCameraDistanceLimitsMultiplier(float value)
        {
            distanceLimitsMultiplier = useDistanceLimitsMultiplier ? value : 1;
        }
    }
}
