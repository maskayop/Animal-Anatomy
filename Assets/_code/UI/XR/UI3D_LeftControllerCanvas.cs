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
        [SerializeField] string stickAction;
        [SerializeField] string triggerAction;

        [Header("Dead Zones")]
        [SerializeField] float stickDeadZone = 0.5f;
        [SerializeField] float triggerDeadZone = 0.2f;

        InputActionMap actionMap;
        InputAction on_X_ButtonPressed;
        InputAction on_Y_ButtonPressed;
        InputAction on_StickAction;
        InputAction on_TriggerAction;

        [Header("UI")]
        [SerializeField] UISystemActivatingButtonsPanel systemActivatingButtonsPanel;

        [Header("Audio")]
        [SerializeField] AudioClip buttonClickAudioClip;

        bool isInitialized = false;

        GameController gameController;
        LightController lightController;

        Vector2 stickValue = Vector2.zero;
        float triggerValue = 0;

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
            gameController = GameController.Instance;
            lightController = LightController.Instance;

            InitializeInputActions();
        }

        void Update()
        {
            if (!gameController)
                return;

            if (isInitialized)
            {
                triggerValue = on_TriggerAction.ReadValue<float>();

                if (triggerValue < triggerDeadZone)
                    gameController.leftTriggerPushed = false;
                else
                    gameController.leftTriggerPushed = true;

                stickValue = on_StickAction.ReadValue<Vector2>();
                OnStickAction();
            }
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
            on_StickAction = actionMap.FindAction(stickAction);
            on_TriggerAction = actionMap.FindAction(triggerAction);

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

            if (on_StickAction != null)
                on_StickAction.Enable();

            if (on_TriggerAction != null)
                on_TriggerAction.Enable();
        }

        void DisableInputActions()
        {
            if (on_X_ButtonPressed != null)
                on_X_ButtonPressed.performed -= OnXButton;

            if (on_Y_ButtonPressed != null)
                on_Y_ButtonPressed.performed -= OnYButton;

            if (on_StickAction != null)
                on_StickAction.Disable();

            if (on_TriggerAction != null)
                on_TriggerAction.Disable();

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

            AudioController.Instance.PlayUIAudioClip(buttonClickAudioClip);
        }

        void OnYButton(InputAction.CallbackContext context)
        {
            if (UI3D_BodyPartsListCanvas.Instance)
            {
                if (UI3D_BodyPartsListCanvas.Instance.IsBodyPartsListPanelActive())
                    UI3D_BodyPartsListCanvas.Instance.ClosePartsListPanel();
                else
                    UI3D_BodyPartsListCanvas.Instance.OpenPartsListPanel();
            }

            AudioController.Instance.PlayUIAudioClip(buttonClickAudioClip);
        }

        void OnStickAction()
        {
            if (stickValue.x < -stickDeadZone || stickValue.x > stickDeadZone)
            {
                lightController.GetLightRotator().sensitivity = stickValue.x;
                lightController.GetLightRotator().RotateObject();
            }
        }
    }
}
