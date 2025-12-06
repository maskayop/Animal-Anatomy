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
        [SerializeField] string grip_ButtonName;
        [SerializeField] string stickAction;
        [SerializeField] string triggerAction;

        [Header("Dead Zones")]
        [SerializeField] float stickDeadZone = 0.5f;
        [SerializeField] float triggerDeadZone = 0.2f;

        InputActionMap actionMap;
        InputAction on_A_ButtonPressed;
        InputAction on_B_ButtonPressed;
        InputAction on_Grip_ButtonPressed;
        InputAction on_StickAction;
        InputAction on_TriggerAction;

        [Header("Audio")]
        [SerializeField] AudioClip buttonClickAudioClip;

        bool isInitialized = false;

        GameController gameController;
        InputController inputController;
        ExaminationController examinationController;

        Vector2 stickValue = Vector2.zero;
        GameObject baseObject;
        ObjectRotator rotator;
        ObjectScaler scaler;

        float triggerValue = 0;

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
            inputController = InputController.Instance;
            examinationController = ExaminationController.Instance;
            baseObject = GameController.Instance.baseGameObject;
            rotator = baseObject.GetComponent<ObjectRotator>();
            scaler = baseObject.GetComponent<ObjectScaler>();

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
                    gameController.rightTriggerPushed = false;
                else
                    gameController.rightTriggerPushed = true;
                
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

            on_A_ButtonPressed = actionMap.FindAction(A_ButtonName);
            on_B_ButtonPressed = actionMap.FindAction(B_ButtonName);
            on_Grip_ButtonPressed = actionMap.FindAction(grip_ButtonName);
            on_StickAction = actionMap.FindAction(stickAction);
            on_TriggerAction = actionMap.FindAction(triggerAction);

            if (gameObject.activeInHierarchy && enabled)
                EnableInputActions();

            isInitialized = true;
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

            if (on_Grip_ButtonPressed != null)
                on_Grip_ButtonPressed.performed += OnGripButton;

            if (on_StickAction != null)
                on_StickAction.Enable();

            if (on_TriggerAction != null)
                on_TriggerAction.Enable();
        }

        void DisableInputActions()
        {
            if (on_A_ButtonPressed != null)
                on_A_ButtonPressed.performed -= OnAButton;

            if (on_B_ButtonPressed != null)
                on_B_ButtonPressed.performed -= OnBButton;

            if (on_Grip_ButtonPressed != null)
                on_Grip_ButtonPressed.performed -= OnGripButton;

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

        void OnAButton(InputAction.CallbackContext context)
        {
            if (examinationController && examinationController.isExamination)
                return;

            if (gameController && inputController)
            {
                if (inputController.isAlternativeInput)
                {                    
                    if (!gameController.transparentMode)
                        gameController.SetIsolatedMode(!gameController.isolatedMode);
                }
                else
                {
                    if (!gameController.isolatedMode)
                        gameController.SetTransparentMode(!gameController.transparentMode);
                }
            }

            AudioController.Instance.PlayUIAudioClip(buttonClickAudioClip);
        }

        void OnBButton(InputAction.CallbackContext context)
        {
            if (examinationController && examinationController.isExamination)
                return;

            if (UI3D_MenuCanvas.Instance)
            {
                if (UI3D_MenuCanvas.Instance.IsMenuWindowActive())
                    UI3D_MenuCanvas.Instance.CloseMenuWindow();
                else
                    UI3D_MenuCanvas.Instance.OpenMenuWindow();
            }

            AudioController.Instance.PlayUIAudioClip(buttonClickAudioClip);
        }

        void OnStickAction()
        {
            if (gameController.leftTriggerPushed || gameController.rightTriggerPushed)
                return;

            if (stickValue.x < -stickDeadZone || stickValue.x > stickDeadZone)
            {
                rotator.sensitivity = -stickValue.x;
                rotator.RotateObject();
            }

            if (stickValue.y < -stickDeadZone || stickValue.y > stickDeadZone)
            {
                scaler.sensitivity = stickValue.y;
                scaler.ScaleObject();
            }
        }

        void OnGripButton(InputAction.CallbackContext context)
        {
            if (examinationController && examinationController.isExamination)
                return;

            if (InputController.Instance)
                InputController.Instance.isAlternativeInput = !InputController.Instance.isAlternativeInput;

            AudioController.Instance.PlayUIAudioClip(buttonClickAudioClip);
        }

        public void SetExaminationMode()
        {
            if (UI3D_MenuCanvas.Instance)
                UI3D_MenuCanvas.Instance.CloseMenuWindow();
        }
    }
}
