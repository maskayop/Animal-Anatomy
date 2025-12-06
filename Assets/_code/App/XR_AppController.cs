using System.Collections;
using UnityEngine;
using Vopere.Common;
using Unity.XR.CoreUtils;

namespace AnimalAnatomy
{
    public class XR_AppController : MonoBehaviour
    {
        public static XR_AppController Instance;

        [Header("Camera")]
        [SerializeField] GameObject cameraController;
        [SerializeField] GameObject PICO_XR_Camera;

        [Header("UI")]
        [SerializeField] GameObject displayUI;
        [SerializeField] GameObject XR_UI;
        [SerializeField] GameObject eventSystemGameObject;

        XROrigin XR_Origin;

        void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("Cannot create XR_AppController");
                Destroy(gameObject);
                return;
            }

            Instance = this;

            Init();
        }

        public void Init()
        {
            if (!App.Instance)
                return;

            if (GetComponent<AudioListener>())
                GetComponent<AudioListener>().enabled = true;

            StartCoroutine(InitDelayed());
        }

        IEnumerator InitDelayed()
        {
            yield return new WaitForSeconds(1.0f);

            if (App.Instance.isXR)
            {
                DestroyImmediate(cameraController);
                DestroyImmediate(displayUI);

                XR_Origin = FindAnyObjectByType<XROrigin>();

                if (XR_Origin)
                    if (XR_Origin.Camera)
                        if (XR_Origin.Camera.GetComponent<AudioListener>())
                            XR_Origin.Camera.GetComponent<AudioListener>().enabled = true;

                if (eventSystemGameObject)
                    eventSystemGameObject.SetActive(false);
            }
            else
            {
                DestroyImmediate(PICO_XR_Camera);
                DestroyImmediate(XR_UI);

                if (CameraController.Instance)
                    CameraController.Instance.Init();

                if (eventSystemGameObject)
                    eventSystemGameObject.SetActive(true);
            }

            if (GetComponent<AudioListener>())
                GetComponent<AudioListener>().enabled = false;            
        }
    }
}
