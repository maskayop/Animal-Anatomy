using UnityEngine;

namespace AnimalAnatomy
{
    public class LightController : MonoBehaviour
    {
        [SerializeField] Light mainLight;
        [SerializeField] float rotationSpeed = 5f;

        Vector2 lastMousePosition;

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
    }
}
