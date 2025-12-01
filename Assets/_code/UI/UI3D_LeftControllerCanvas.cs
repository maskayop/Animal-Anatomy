using UnityEngine;
using UnityEngine.InputSystem;
using Vopere.Common;

namespace AnimalAnatomy
{
    public class UI3D_LeftControllerCanvas : MonoBehaviour
    {
        public static UI3D_LeftControllerCanvas Instance;

        [Header("Input Actions")]
        [SerializeField] InputActionAsset inputActionsAsset;
        [SerializeField] string inputActionMapName;
        [SerializeField] string X_ButtonName;
        [SerializeField] string Y_ButtonName;

        InputActionMap actionMap;
        InputAction on_X_ButtonPressed;
        InputAction on_Y_ButtonPressed;

        [Header("UI")]
        [SerializeField] UISystemActivatingButtonsPanel systemActivatingButtonsPanel;

        bool isInitialized = false;

        void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("Cannot create UI3D_LeftControllerCanvas");
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        void Start()
        {
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

            on_X_ButtonPressed = actionMap.FindAction(X_ButtonName);
            on_Y_ButtonPressed = actionMap.FindAction(Y_ButtonName);

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

            if (on_X_ButtonPressed != null)
                on_X_ButtonPressed.performed += OnXButton;

            if (on_Y_ButtonPressed != null)
                on_Y_ButtonPressed.performed += OnYButton;
        }

        void DisableInputActions()
        {
            if (on_X_ButtonPressed != null)
                on_X_ButtonPressed.performed -= OnXButton;

            if (on_Y_ButtonPressed != null)
                on_Y_ButtonPressed.performed -= OnYButton;

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

        void OnXButton(InputAction.CallbackContext context)
        {
            if (systemActivatingButtonsPanel)
                systemActivatingButtonsPanel.gameObject.SetActive(!systemActivatingButtonsPanel.gameObject.activeInHierarchy);
        }

        void OnYButton(InputAction.CallbackContext context)
        {
            
        }
    }
}
