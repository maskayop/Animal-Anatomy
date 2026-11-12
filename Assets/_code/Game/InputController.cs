using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
#if PLATFORM_ANDROID
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
#endif

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

        Vector2 mousePosition;

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
            if (!gameController) return;
            if (mainCanvas && mainCanvas.isLoading) return;
            if (ExaminationController.Instance && ExaminationController.Instance.isExamination) return;

            HandleMouseInput();
            HandleKeyboardInput();
        }

        public void Init()
        {
            gameController = GameController.Instance;
            cameraController = CameraController.Instance;
            mainCanvas = UIMainCanvas.Instance;

            EnhancedTouchSupport.Enable();

#if PLATFORM_ANDROID
            currentTime = selectionTimeout;
#endif
        }

        void OnDestroy()
        {
            if (EnhancedTouchSupport.enabled)
                EnhancedTouchSupport.Disable();
        }

        // ─────────────────────────── Mouse ───────────────────────────

        void HandleMouseInput()
        {
            var mouse = Mouse.current;
            if (mouse == null) return;

            if (mouse.rightButton.wasPressedThisFrame)
                SelectBodyPart(mouse.position.ReadValue());

            if (mouse.middleButton.wasPressedThisFrame)
                cameraController?.UpdatePosition();
        }

        // ────────────────────────── Keyboard ──────────────────────────

        void HandleKeyboardInput()
        {
            var keyboard = Keyboard.current;
            if (keyboard == null) return;

            if (keyboard.fKey.wasPressedThisFrame)
                cameraController?.UpdatePosition();

            if (keyboard.qKey.wasPressedThisFrame && !gameController.transparentMode)
                gameController?.SetIsolatedMode(!gameController.isolatedMode);

            if (keyboard.wKey.wasPressedThisFrame && !gameController.isolatedMode)
                gameController?.SetTransparentMode(!gameController.transparentMode);

            if (keyboard.escapeKey.wasPressedThisFrame)
                HandleEscape();

            ListenForDeletePlayerPrefs(keyboard);
        }

        void HandleEscape()
        {
            if (gameController.isolatedMode)
                gameController.SetIsolatedMode(false);

            if (gameController.transparentMode)
                gameController.SetTransparentMode(false);
            else
                gameController.UnSelectBodyPart(false);
        }

        void ListenForDeletePlayerPrefs(Keyboard keyboard)
        {
            if (keyboard.leftCtrlKey.isPressed &&
                keyboard.leftShiftKey.isPressed &&
                keyboard.leftAltKey.isPressed &&
                keyboard.dKey.wasPressedThisFrame)
            {
                PlayerPrefs.DeleteAll();
            }
        }

        // ─────────────────────── Selection ───────────────────────────

        void SelectBodyPart(Vector2 screenPosition)
        {
            if (!cameraController || !gameController) return;
            if (gameController.isolatedMode || gameController.transparentMode) return;

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

        // ─────────────────────── Android touch ───────────────────────

        public void CallBodyPartSelection()
        {
            if (ExaminationController.Instance && ExaminationController.Instance.isExamination) return;
            if (LightController.Instance && LightController.Instance.lightRotationMode) return;

#if PLATFORM_ANDROID
            HandleAndroidTouch();
#endif
        }

#if PLATFORM_ANDROID
        void HandleAndroidTouch()
        {
            if (!CanProcessTouch()) return;
            if (Touch.activeTouches.Count != 1) return;

            Touch touch = Touch.activeTouches[0];

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

            if (touch.phase == UnityEngine.InputSystem.TouchPhase.Ended ||
                touch.phase == UnityEngine.InputSystem.TouchPhase.Canceled)
            {
                if (currentTime > 0)
                    SelectBodyPart(touch.screenPosition);

                isFirstClick = true;
            }
        }

        bool CanProcessTouch()
        {
            if (!mainCanvas.BodyPartsListIsOpen)
                return true;

            if (Touch.activeTouches.Count == 0)
                return false;

            float touchX = Touch.activeTouches[0].screenPosition.x;
            float maxAnchor = Screen.width * mainCanvas.GetBodyPartsListPanelMaxAnchor();
            return touchX > maxAnchor;
        }
#endif
    }
}
