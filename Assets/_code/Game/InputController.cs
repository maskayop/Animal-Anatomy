using UnityEngine;

namespace AnimalAnatomy
{
    public class InputController : MonoBehaviour
    {
        public static InputController Instance;

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
            if (UIMainCanvas.Instance.isLoading)
                return;

            SelectBodyPart();
            UpdateView();

            if (!GameController.Instance.isolatedMode && !GameController.Instance.transparentMode)
            {
                if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
                    UIMainCanvas.Instance.SetExclusionMode(true);
                else
                    UIMainCanvas.Instance.SetExclusionMode(false);
            }
        }

        public void Init()
        {

        }

        void SelectBodyPart()
        {
            if (Input.GetMouseButtonDown(1))
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

                            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
                                GameController.Instance.HideSelectedBodyPart();
                        }
                    }
                    else
                    {
                        GameController.Instance.UnSelectBodyPart();
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (GameController.Instance.isolatedMode)
                    GameController.Instance.SetIsolatedMode(false);
                if (GameController.Instance.transparentMode)
                    GameController.Instance.SetTransparentMode(false);
                else
                    GameController.Instance.UnSelectBodyPart();
            }

            if (Input.GetKeyDown(KeyCode.Q) && !GameController.Instance.transparentMode)
            {
                GameController.Instance.SetIsolatedMode(!GameController.Instance.isolatedMode);
            }

            if (Input.GetKeyDown(KeyCode.W) && !GameController.Instance.isolatedMode)
            {
                GameController.Instance.SetTransparentMode(!GameController.Instance.transparentMode);
            }
        }

        void UpdateView()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                CameraController.Instance.UpdatePosition();
            }
        }

        public void DisableAllSystemsExceptSystem(GameController.SystemType systemType)
        {
            GameController.Instance.ActivateAllSystems(false);
            GameController.Instance.EnableSystem(systemType);
        }
    }
}