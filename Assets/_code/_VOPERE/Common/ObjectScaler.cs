using UnityEngine;

namespace Vopere.Common
{
    public class ObjectScaler : MonoBehaviour
    {
        [SerializeField] float scaleDelta = 0;
        [SerializeField] Vector2 scaleLimits = Vector2.one;

        public float sensitivity = 1;
        public bool canBeScaled = true;

        public void ScaleObject()
        {
            if (!canBeScaled)
                return;

            transform.localScale += Vector3.one * scaleDelta * sensitivity;

            if (transform.localScale.x <= scaleLimits.x)
                transform.localScale = Vector3.one * scaleLimits.x;
            else if (transform.localScale.x >= scaleLimits.y)
                transform.localScale = Vector3.one * scaleLimits.y;
        }
    }
}
