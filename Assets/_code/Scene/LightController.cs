using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Vopere.Common;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace AnimalAnatomy
{
    public class LightController : MonoBehaviour
    {
        public static LightController Instance;

        [Header("Main Light")]
        [SerializeField] Light mainLight;
        [SerializeField] float rotationSpeed = 5f;

        [Header("Background")]
        [SerializeField] MeshRenderer skyboxRenderer;
        [SerializeField] List<Material> skyboxMaterials = new List<Material>();

        public bool lightRotationMode = false;

        Vector2 currentMousePosition;
        Vector2 lastMousePosition;
        Vector2 lastTouchPosition;
        int backgroundColorSchemeId;

        ObjectRotator lightRotator;

#if ENABLE_INPUT_SYSTEM
        Mouse currentMouse = Mouse.current;
#endif

        void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("Cannot create LightController");
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        void Start()
        {
            EnhancedTouchSupport.Enable();
            Init();
        }

        void OnDestroy()
        {
            if (EnhancedTouchSupport.enabled)
                EnhancedTouchSupport.Disable();
        }

        public void Init()
        {
            backgroundColorSchemeId = DataSaveLoad.Instance.GetSavedInt("BackgroundColorScheme");

            if (backgroundColorSchemeId != -1)
                SetSkyboxColors(backgroundColorSchemeId);

            lightRotator = mainLight.GetComponent<ObjectRotator>();

            // Инициализируем lastMousePosition текущей позицией мыши
#if ENABLE_INPUT_SYSTEM
            if (Mouse.current != null)
            {
                lastMousePosition = Mouse.current.position.ReadValue();
            }
#endif
        }

        void Update()
        {
#if ENABLE_LEGACY_INPUT_MANAGER
            currentMousePosition = Input.mousePosition;

            if (App.Instance && !App.Instance.isXR)
            {
                if (Input.GetMouseButton(0))
                    RotateLightLegacy();
            }

            lastMousePosition = currentMousePosition;
#endif

#if ENABLE_INPUT_SYSTEM
            // --- Проверяем ПК (мышь) ---
            currentMouse = Mouse.current;
            if (currentMouse != null)
            {
                currentMousePosition = currentMouse.position.ReadValue();

                if (App.Instance && !App.Instance.isXR)
                {
                    if (currentMouse.leftButton.isPressed)
                    {
                        RotateLight(currentMousePosition, ref lastMousePosition);
                    }
                    else
                    {
                        // Обновляем lastMousePosition только когда кнопка не нажата
                        lastMousePosition = currentMousePosition;
                    }
                }
            }

            // --- Проверяем Android (тач) ---
            if (App.Instance && !App.Instance.isXR && Touch.activeTouches.Count == 1)
            {
                Touch touch = Touch.activeTouches[0];

                // Если палец двигается или стоит на месте
                if (touch.phase == UnityEngine.InputSystem.TouchPhase.Moved ||
                    touch.phase == UnityEngine.InputSystem.TouchPhase.Stationary)
                {
                    Vector2 touchPosition = touch.screenPosition;

                    // Для первого кадра касания - запоминаем позицию
                    if (touch.phase == UnityEngine.InputSystem.TouchPhase.Began)
                    {
                        lastTouchPosition = touchPosition;
                        return;
                    }

                    RotateLight(touchPosition, ref lastTouchPosition);
                }
                else
                {
                    // Обновляем lastTouchPosition когда палец не двигается
                    lastTouchPosition = touch.screenPosition;
                }
            }
#endif
        }

        // --- Вращение для новой Input System (ПК и Android) ---
        void RotateLight(Vector2 currentPosition, ref Vector2 lastPosition)
        {
            if (!lightRotationMode)
                return;

            float deltaX = currentPosition.x - lastPosition.x;

            // Если дельта слишком маленькая - не вращаем (экономия производительности)
            if (Mathf.Abs(deltaX) < 0.1f)
                return;

            float rotationDelta = -deltaX * rotationSpeed * Time.deltaTime;
            mainLight.transform.rotation = Quaternion.Euler(
                mainLight.transform.eulerAngles.x,
                mainLight.transform.eulerAngles.y + rotationDelta,
                0
            );

            // Обновляем lastPosition после вращения
            lastPosition = currentPosition;
        }

        // --- Вращение для старой Input System (Legacy) ---
        void RotateLightLegacy()
        {
            if (!lightRotationMode)
                return;

            float deltaX = currentMousePosition.x - lastMousePosition.x;

            if (Mathf.Abs(deltaX) < 0.1f)
                return;

            float rotationDelta = -deltaX * rotationSpeed * Time.deltaTime;
            mainLight.transform.rotation = Quaternion.Euler(
                mainLight.transform.eulerAngles.x,
                mainLight.transform.eulerAngles.y + rotationDelta,
                0
            );
        }

        public void SetSkyboxColors(int id)
        {
            if (skyboxRenderer && id >= 0 && id < skyboxMaterials.Count)
                skyboxRenderer.material = skyboxMaterials[id];
        }

        public void SetLightingMode(bool state)
        {
            lightRotationMode = state;
        }

        public ObjectRotator GetLightRotator()
        {
            return lightRotator;
        }
    }
}
