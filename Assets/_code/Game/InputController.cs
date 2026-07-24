using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace AnimalAnatomy
{
    public class InputController : MonoBehaviour
    {
        public static InputController Instance;

        public bool isAlternativeInput = false;

        [Header("Android")]
        public float selectionTimeout = 1.0f;

#if PLATFORM_ANDROID
        float currentTime = 0;
        bool isFirstClick = true;
#endif

        GameController gameController;
        CameraController cameraController;
        UIMainCanvas mainCanvas;

#if ENABLE_INPUT_SYSTEM
        Mouse currentMouse = Mouse.current;
        Keyboard keyboard = Keyboard.current;
#endif

        void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("Cannot create InputController");
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
            if (!gameController)
                return;

            if (mainCanvas && mainCanvas.isLoading)
                return;

            if (ExaminationController.Instance)
                if (ExaminationController.Instance.isExamination)
                    return;

#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetMouseButtonDown(1))
                SelectBodyPart();

            UpdateView();

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (gameController.isolatedMode)
                    gameController.SetIsolatedMode(false);
                if (gameController.transparentMode)
                    gameController.SetTransparentMode(false);
                else
                    gameController.UnSelectBodyPart(false);
            }
#endif

#if ENABLE_INPUT_SYSTEM
            currentMouse = Mouse.current;
            keyboard = Keyboard.current;

            if (currentMouse.rightButton.wasPressedThisFrame)
                SelectBodyPart();

            UpdateView();

            if (keyboard.escapeKey.wasPressedThisFrame)
            {
                if (gameController.isolatedMode)
                    gameController.SetIsolatedMode(false);
                if (gameController.transparentMode)
                    gameController.SetTransparentMode(false);
                else
                    gameController.UnSelectBodyPart(false);
            }
#endif

            ListenForDeletePlayerPrefs();
        }

        public void Init()
        {
            gameController = GameController.Instance;
            cameraController = CameraController.Instance;
            mainCanvas = UIMainCanvas.Instance;

            // Включаем поддержку EnhancedTouch
            EnhancedTouchSupport.Enable();

            // Кешируем устройства ввода
            currentMouse = Mouse.current;

#if PLATFORM_ANDROID
            currentTime = selectionTimeout;
#endif
        }

        void OnDestroy()
        {
            // Отключаем EnhancedTouch при выходе
            if (EnhancedTouchSupport.enabled)
                EnhancedTouchSupport.Disable();
        }

        public void CallBodyPartSelection()
        {
            if (ExaminationController.Instance && ExaminationController.Instance.isExamination)
                return;

            if (LightController.Instance && LightController.Instance.lightRotationMode)
                return;

#if PLATFORM_ANDROID && ENABLE_INPUT_SYSTEM
            // Проверяем, можно ли обрабатывать тап (не по UI)
            bool canProcessTouch = false;

            if (!mainCanvas.BodyPartsListIsOpen)
            {
                // Список закрыт - можно обрабатывать всегда
                canProcessTouch = true;
            }
            else if (Touch.activeTouches.Count > 0)
            {
                // Список открыт - проверяем, что тап не по панели списка
                float touchX = Touch.activeTouches[0].screenPosition.x;
                float maxAnchor = Screen.width * mainCanvas.GetBodyPartsListPanelMaxAnchor();
                if (touchX > maxAnchor)
                {
                    canProcessTouch = true;
                }
            }

            if (canProcessTouch && Touch.activeTouches.Count == 1)
            {
                Touch touch = Touch.activeTouches[0];

                // Начало или удержание касания
                if (touch.phase == UnityEngine.InputSystem.TouchPhase.Began ||
                    touch.phase == UnityEngine.InputSystem.TouchPhase.Moved ||
                    touch.phase == UnityEngine.InputSystem.TouchPhase.Stationary)
                {
                    if (isFirstClick)
                    {
                        currentTime = selectionTimeout;
                        isFirstClick = false;
                    }

                    currentTime -= Time.deltaTime;
                }

                // Конец касания
                if (touch.phase == UnityEngine.InputSystem.TouchPhase.Ended ||
                    touch.phase == UnityEngine.InputSystem.TouchPhase.Canceled)
                {
                    if (currentTime > 0)
                    {
                        // Передаём позицию тача в SelectBodyPart
                        SelectBodyPart(touch.screenPosition);
                    }

                    isFirstClick = true;
                }
            }
#endif
        }

        // Оригинальный метод (для совместимости с ПК)
        void SelectBodyPart()
        {
#if ENABLE_LEGACY_INPUT_MANAGER
    SelectBodyPart(Input.mousePosition);
#endif

#if ENABLE_INPUT_SYSTEM
            if (currentMouse != null)
            {
                Vector2 mousePosition = currentMouse.position.ReadValue();
                SelectBodyPart(mousePosition);
            }
#endif
        }

        // Новая перегрузка с параметром (для Android)
        void SelectBodyPart(Vector2 screenPosition)
        {
            if (!cameraController || !gameController)
                return;

            if (!gameController.isolatedMode && !gameController.transparentMode)
            {
                Ray ray = cameraController.mainCamera.ScreenPointToRay(screenPosition);

                if (Physics.Raycast(ray, out RaycastHit hit, 100000, 1 << 6))
                {
                    BodyPartInfo info = hit.collider.GetComponentInParent<BodyPartInfo>();

                    if (info)
                    {
                        gameController.SelectBodyPart(info);

                        if (gameController.exclusionMode)
                            gameController.HideSelectedBodyPart();
                    }
                }
                else
                {
                    gameController.UnSelectBodyPart(false);
                    gameController.UnSelectBodyPartGroup();
                }
            }
        }

        void UpdateView()
        {
#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetKeyDown(KeyCode.F))
                cameraController?.UpdatePosition();

            if (Input.GetKeyDown(KeyCode.Q) && !gameController.transparentMode)
                gameController?.SetIsolatedMode(!gameController.isolatedMode);

            if (Input.GetKeyDown(KeyCode.W) && !gameController.isolatedMode)
                gameController?.SetTransparentMode(!gameController.transparentMode);
#endif

#if ENABLE_INPUT_SYSTEM
            if (keyboard.fKey.wasPressedThisFrame)
                cameraController?.UpdatePosition();

            if (keyboard.qKey.wasPressedThisFrame && !gameController.transparentMode)
                gameController?.SetIsolatedMode(!gameController.isolatedMode);

            if (keyboard.wKey.wasPressedThisFrame && !gameController.isolatedMode)
                gameController?.SetTransparentMode(!gameController.transparentMode);
#endif
        }

        void ListenForDeletePlayerPrefs()
        {
#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.D))
                PlayerPrefs.DeleteAll();
#endif

#if ENABLE_INPUT_SYSTEM
            if (keyboard.leftCtrlKey.isPressed && keyboard.leftShiftKey.isPressed && keyboard.leftAltKey.isPressed && keyboard.dKey.wasPressedThisFrame)
                PlayerPrefs.DeleteAll();
#endif
        }
    }
}
