using UnityEngine;
using UnityEngine.InputSystem;
using Vopere.Common;

namespace AnimalAnatomy
{
    public class UI3D_RightControllerCanvas : MonoBehaviour
    {
        public static UI3D_RightControllerCanvas Instance;

        [Header("Input Actions")]
        [SerializeField] InputActionAsset inputActionsAsset;
        [SerializeField] string inputActionMapName;
        [SerializeField] string A_ButtonName;
        [SerializeField] string B_ButtonName;

        InputActionMap actionMap;
        InputAction on_A_ButtonPressed;
        InputAction on_B_ButtonPressed;

        [Header("Audio")]
        [SerializeField] AudioClip buttonClickAudioClip;

        bool isInitialized = false;

        GameController gameController;

        void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("Cannot create UI3D_RightControllerCanvas");
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        void Start()
        {
            gameController = GameController.Instance;

            InitializeInputActions();
        }

        void InitializeInputActions()
        {
            if (!inputActionsAsset)
                return;

            if (!App.Instance || !App.Instance.isXR)
                return;

            actionMap = inputActionsAsset.FindActionMap(inputActionMapName);

            if (actionMap == null)
                return;

            on_A_ButtonPressed = actionMap.FindAction(A_ButtonName);
            on_B_ButtonPressed = actionMap.FindAction(B_ButtonName);

            isInitialized = true;

            if (gameObject.activeInHierarchy && enabled)
                EnableInputActions();
        }

        public void Reinitialize()
        {
            DisableInputActions();
            isInitialized = false;
            InitializeInputActions();
        }

        void EnableInputActions()
        {
            if (actionMap != null)
                actionMap.Enable();

            if (on_A_ButtonPressed != null)
                on_A_ButtonPressed.performed += OnAButton;

            if (on_B_ButtonPressed != null)
                on_B_ButtonPressed.performed += OnBButton;
        }

        void DisableInputActions()
        {
            if (on_A_ButtonPressed != null)
                on_A_ButtonPressed.performed -= OnAButton;

            if (on_B_ButtonPressed != null)
                on_B_ButtonPressed.performed -= OnBButton;

            if (actionMap != null)
                actionMap.Disable();
        }

        void OnEnable()
        {
            if (!isInitialized)
                return;

            EnableInputActions();
        }

        void OnDisable()
        {
            DisableInputActions();
        }

        void OnAButton(InputAction.CallbackContext context)
        {
            if (gameController)
                gameController.SetTransparentMode(!gameController.transparentMode);

            AudioController.Instance.PlayUIAudioClip(buttonClickAudioClip);
        }

        void OnBButton(InputAction.CallbackContext context)
        {
            if (gameController)
                gameController.SetIsolatedMode(!gameController.isolatedMode);

            AudioController.Instance.PlayUIAudioClip(buttonClickAudioClip);
        }
    }
}
