using UnityEngine;
using Vopere.Common;

namespace AnimalAnatomy
{
    public abstract class UI_SettingsWindowBase : MonoBehaviour
    {
        [SerializeField] bool closeOnAwake = true;
        [SerializeField] protected GameObject window;

        protected App app;
        protected DataSaveLoad dataSaveLoad;
        protected ScenesManager scenesManager;
        protected GameController gameController;
        protected AudioController audioController;
        protected LightController lightController;
        protected CameraController cameraController;

        bool isOpen = false;
        public bool IsOpen { get { return isOpen; } }

        void Awake()
        {
            if (closeOnAwake)
                Close();
        }

        void Start()
        {
            Init();
        }

        public void Init()
        {
            app = App.Instance;
            dataSaveLoad = DataSaveLoad.Instance;
            scenesManager = ScenesManager.Instance;
            gameController = GameController.Instance;
            audioController = AudioController.Instance;
            lightController = LightController.Instance;
            cameraController = CameraController.Instance;

            OnInit();
        }

        protected virtual void OnInit() { }

        public void Open()
        {
            isOpen = true;
            window.SetActive(true);

            if (cameraController)
                cameraController.Freeze(true);

            OnOpen();
        }

        protected virtual void OnOpen() { }

        public void Close()
        {
            isOpen = false;
            window.SetActive(false);

            if (cameraController)
                cameraController.Freeze(false);

            OnClose();
        }

        protected virtual void OnClose() { }
    }
}
