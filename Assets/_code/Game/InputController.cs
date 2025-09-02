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
            SelectBodyPart();
            UpdateView();
        }

        public void Init()
        {

        }

        void SelectBodyPart()
        {
            if (Input.GetMouseButtonDown(1))
            {
                Ray ray = CameraController.Instance.mainCamera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit, 100000, 1 << 6))
                {
                    BodyPartInfo info = hit.collider.GetComponentInParent<BodyPartInfo>();

                    if (info)
                        GameController.Instance.SelectBodyPart(info);
                }
                else
                {
                    if (!GameController.Instance.isolated)
                        GameController.Instance.UnSelectBodyPart();
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (GameController.Instance.isolated)
                    GameController.Instance.SetIsolatedMode(false);
                else
                    GameController.Instance.UnSelectBodyPart();
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                GameController.Instance.SetIsolatedMode(!GameController.Instance.isolated);
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