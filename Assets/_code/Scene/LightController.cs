using System.Collections.Generic;
using UnityEngine;

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

        Vector2 lastMousePosition;

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
            
        }

        void Update()
        {
            Vector3 currentMousePosition = Input.mousePosition;
            
            if (Input.GetKey(KeyCode.C))
            {
                float mouseDeltaX = -(currentMousePosition.x - lastMousePosition.x) * rotationSpeed * Time.deltaTime;
                mainLight.transform.rotation = Quaternion.Euler(mainLight.transform.eulerAngles.x, mainLight.transform.eulerAngles.y - mouseDeltaX, 0);
            }
            
            lastMousePosition = currentMousePosition;
        }

        public void SetSkyboxColors(int id)
        {
            skyboxRenderer.material = skyboxMaterials[id];
        }
    }
}
