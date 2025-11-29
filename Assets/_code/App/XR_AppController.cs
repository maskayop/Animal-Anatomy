using System.Collections;
using UnityEngine;
using Vopere.Common;

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

            StartCoroutine(InitDelayed());
        }

        IEnumerator InitDelayed()
        {
            yield return new WaitForSeconds(1.0f);

            if (App.Instance.isXR)
            {
                DestroyImmediate(cameraController);
                DestroyImmediate(displayUI);
            }
            else
            {
                DestroyImmediate(PICO_XR_Camera);
                DestroyImmediate(XR_UI);
            }
        }
    }
}
