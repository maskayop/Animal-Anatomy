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

        void Update()
        {
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftAlt) &&
                Input.GetKey(KeyCode.F) && Input.GetKey(KeyCode.Alpha0))
                UIMainCanvas.Instance.ShowVopere(true);
            else
                UIMainCanvas.Instance.ShowVopere(false);

            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.D))
                PlayerPrefs.DeleteAll();

            if (UIMainCanvas.Instance.isLoading)
                return;

            if (ExaminationController.Instance.isExamination)
                return;

            SelectBodyPart();
            UpdateView();
            SetExclusionMode();
        }

        void SelectBodyPart()
        {
            if (Input.GetMouseButtonDown(1) || Input.touchCount != 0)
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
                        GameController.Instance.UnSelectBodyPartGroup();
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
                if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
                    UIMainCanvas.Instance.SetExclusionMode(true);
                else
                    UIMainCanvas.Instance.SetExclusionMode(false);
            }
        }
    }
}
