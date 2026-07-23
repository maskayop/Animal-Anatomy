using UnityEngine;
using UnityEngine.InputSystem;

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

#if ENABLE_LEGACY_INPUT_MANAGER
        bool isFirstClick = true;
#endif
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

#if PLATFORM_ANDROID
            currentTime = selectionTimeout;
#endif
        }

        public void CallBodyPartSelection()
        {
            if (ExaminationController.Instance)
                if (ExaminationController.Instance.isExamination)
                    return;

#if PLATFORM_ANDROID
#if ENABLE_LEGACY_INPUT_MANAGER
            if (!mainCanvas.BodyPartsListIsOpen ||
                mainCanvas.BodyPartsListIsOpen &&
                Input.mousePosition.x > Screen.width * mainCanvas.GetBodyPartsListPanelMaxAnchor())
            {
                if (Input.GetMouseButton(0))
                {
                    if (isFirstClick)
                    {
                        currentTime = selectionTimeout;
                        isFirstClick = false;
                    }

                    currentTime -= Time.deltaTime;
                }

                if (Input.GetMouseButtonUp(0))
                {
                    if (currentTime > 0)
                        SelectBodyPart();
                
                    isFirstClick = true;
                }
            }
#endif
#endif
        }

        void SelectBodyPart()
        {
            if (!cameraController || !gameController)
                return;

            if (!gameController.isolatedMode && !gameController.transparentMode)
            {
#if ENABLE_LEGACY_INPUT_MANAGER
                Ray ray = cameraController.mainCamera.ScreenPointToRay(Input.mousePosition);
#endif

#if ENABLE_INPUT_SYSTEM
                currentMouse = Mouse.current;
                Vector2 mousePosition = currentMouse.position.ReadValue();
                Ray ray = cameraController.mainCamera.ScreenPointToRay(mousePosition);
#endif

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
