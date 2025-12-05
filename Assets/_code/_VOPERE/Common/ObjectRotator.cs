using UnityEngine;

namespace Vopere.Common
{
    public class ObjectRotator : MonoBehaviour
    {
        public enum Axis { x, y, z }
        public Axis axis = Axis.x;

        public float angle = 0;
        public float sensitivity = 1;
        public bool canBeRotated = true;

        public void RotateObject()
        {
            if (!canBeRotated)
                return;

            switch (axis)
            {
                case Axis.x:
                    Rotate(angle * sensitivity, 0, 0);
                    break;
                case Axis.y:
                    Rotate(0, angle * sensitivity, 0);
                    break;
                case Axis.z:
                    Rotate(0, 0, angle * sensitivity);
                    break;
            }
        }

        void Rotate(float xAngle, float yAngle, float zAngle)
        {
            transform.Rotate(xAngle, yAngle, zAngle, Space.Self);
        }
    }
}
