using System.Collections.Generic;
using UnityEngine;
using Vopere.Common;

namespace AnimalAnatomy
{
    public class LightController : MonoBehaviour
    {
        public static LightController Instance;

        [Header("Main Light")]
        [SerializeField] Light mainLight;
        [SerializeField] float rotationSpeed = 5f;

        [Header("Background")]
        [SerializeField] MeshRenderer skyboxRenderer;
        [SerializeField] List<Material> skyboxMaterials = new List<Material>();
        [SerializeField] Color colorTop;
        [SerializeField] Color colorBottom;
        
        public bool lightRotationMode = false;

        Vector3 currentMousePosition;
        Vector2 lastMousePosition;
        int backgroundColorSchemeId;

        ObjectRotator lightRotator;

        void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("Cannot create LightController");
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        void Start()
        {
            Init();
        }

        public void Init()
        {
            backgroundColorSchemeId = DataSaveLoad.Instance.GetSavedInt("BackgroundColorScheme");

            if (backgroundColorSchemeId != -1)
                SetSkyboxColors(backgroundColorSchemeId);

            lightRotator = mainLight.GetComponent<ObjectRotator>();
        }

        void Update()
        {
            currentMousePosition = Input.mousePosition;

            if (App.Instance && !App.Instance.isXR)
                if (Input.GetMouseButton(0))
                    RotateLight();

            lastMousePosition = currentMousePosition;
        }

        void RotateLight()
        {
            if (!lightRotationMode)
                return;
            
            float mouseDeltaX = -(currentMousePosition.x - lastMousePosition.x) * rotationSpeed * Time.deltaTime;
            mainLight.transform.rotation = Quaternion.Euler(mainLight.transform.eulerAngles.x, mainLight.transform.eulerAngles.y - mouseDeltaX, 0);            
        }

        public void SetSkyboxColors(int id)
        {
            if (skyboxRenderer)
                skyboxRenderer.material = skyboxMaterials[id];
        }

        public void SetLightingMode(bool state)
        {
            lightRotationMode = state;
        }

        public ObjectRotator GetLightRotator()
        {
            return lightRotator;
        }
    }
}
