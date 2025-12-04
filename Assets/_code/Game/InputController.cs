using UnityEngine;

namespace AnimalAnatomy
{
    public class InputController : MonoBehaviour
    {
        public static InputController Instance;

        [Header("Android")]
        [SerializeField] float selectionTimeout = 1.0f;
        
        float currentTime = 0;

#if PLATFORM_ANDROID
        bool isFirstClick = true;
#endif

        GameController gameController;
        UIMainCanvas mainCanvas;

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
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.D))
                PlayerPrefs.DeleteAll();

            if (mainCanvas && mainCanvas.isLoading)
                return;

            if (ExaminationController.Instance.isExamination)
                return;

#if !PLATFORM_ANDROID
            if (Input.GetMouseButtonDown(1))
                SelectBodyPart();
#endif

            UpdateView();
            SetExclusionMode();

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (gameController.isolatedMode)
                    gameController.SetIsolatedMode(false);
                if (gameController.transparentMode)
                    gameController.SetTransparentMode(false);
                else
                    gameController.UnSelectBodyPart(false);
            }
        }

        public void Init()
        {
            gameController = GameController.Instance;
            mainCanvas = UIMainCanvas.Instance;
            currentTime = selectionTimeout;
        }

        public void CallBodyPartSelection()
        {
            if (ExaminationController.Instance.isExamination)
                return;

#if PLATFORM_ANDROID
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
        }

        void SelectBodyPart()
        {
            if (!CameraController.Instance)
                return;

            if (!gameController.isolatedMode && !gameController.transparentMode)
            {
                Ray ray = CameraController.Instance.mainCamera.ScreenPointToRay(Input.mousePosition);

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
            if (Input.GetKeyDown(KeyCode.F))
                CameraController.Instance.UpdatePosition();

            if (Input.GetKeyDown(KeyCode.Q) && !gameController.transparentMode)
                gameController.SetIsolatedMode(!gameController.isolatedMode);

            if (Input.GetKeyDown(KeyCode.W) && !gameController.isolatedMode)
                gameController.SetTransparentMode(!gameController.transparentMode);
        }

        void SetExclusionMode()
        {
#if PLATFORM_ANDROID
            return;
#else
            if (!gameController.isolatedMode && !gameController.transparentMode)
            {
                if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
                    gameController.SetExclusionMode(true);
                else
                    gameController.SetExclusionMode(false);
            }
#endif
        }
    }
}
