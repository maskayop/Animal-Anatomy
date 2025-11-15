using UnityEngine;

namespace AnimalAnatomy
{
    public class InputController : MonoBehaviour
    {
        public static InputController Instance;

        [Header("Android")]
        [SerializeField] float selectionTimeout = 1.0f;

        public float currentTime = 0;
        public bool isFirstClick = true;

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

            if (mainCanvas.isLoading)
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
                if (GameController.Instance.isolatedMode)
                    GameController.Instance.SetIsolatedMode(false);
                if (GameController.Instance.transparentMode)
                    GameController.Instance.SetTransparentMode(false);
                else
                    GameController.Instance.UnSelectBodyPart();
            }
        }

        public void Init()
        {
            mainCanvas = UIMainCanvas.Instance;
            currentTime = selectionTimeout;
        }

        public void CallBodyPartSelection()
        {
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
            if (!GameController.Instance.isolatedMode && !GameController.Instance.transparentMode)
            {
                Ray ray = CameraController.Instance.mainCamera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit, 100000, 1 << 6))
                {
                    BodyPartInfo info = hit.collider.GetComponentInParent<BodyPartInfo>();

                    if (info)
                    {
                        GameController.Instance.SelectBodyPart(info);

                        if (GameController.Instance.exclusionMode)
                            GameController.Instance.HideSelectedBodyPart();
                    }
                }
                else
                {
                    GameController.Instance.UnSelectBodyPart();
                    GameController.Instance.UnSelectBodyPartGroup();
                }
            }
        }

        void UpdateView()
        {
            if (Input.GetKeyDown(KeyCode.F))
                CameraController.Instance.UpdatePosition();

            if (Input.GetKeyDown(KeyCode.Q) && !GameController.Instance.transparentMode)
                GameController.Instance.SetIsolatedMode(!GameController.Instance.isolatedMode);

            if (Input.GetKeyDown(KeyCode.W) && !GameController.Instance.isolatedMode)
                GameController.Instance.SetTransparentMode(!GameController.Instance.transparentMode);
        }

        void SetExclusionMode()
        {
            if (!GameController.Instance.isolatedMode && !GameController.Instance.transparentMode)
            {
#if !PLATFORM_ANDROID
                if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
                    GameController.Instance.SetExclusionMode(true);
                else
                    GameController.Instance.SetExclusionMode(false);
#endif
            }
        }
    }
}
