using UnityEngine;

namespace Vopere.Common
{
    public class RotationAnimator : MonoBehaviour
    {
        public enum Axis { x, y, z }
        public Axis axis = Axis.x;

        public float angle = 0;
        public float rotationTime = 1.0f;
        public bool canBeRotated = true;

        [Header("Info")]
        public bool isRotating = false;
        public float timePassed = 0;

        Quaternion startRotation;
        Vector3 deltaRotation = Vector3.zero;
        float endAngle = 0;
        float summDeltaRotation = 0.0f;
        float deltaOverRotation = 0.0f;

        [Header("Limitations")]
        public bool useLimitations = false;
        public int minAngle = 0;
        public int maxAngle = 360;

        float currentRotation = 0.0f;

        [Header("Around Point")]
        public bool rotateAroudPoint = false;
        public Transform rotationPoint;

        void Start()
        {
            startRotation = transform.localRotation;
        }

        void Update()
        {
            if (isRotating && canBeRotated)
            {
                if (useLimitations)
                    RotateLimited();
                else
                    RotateUnlimited();
            }
        }

        void RotateLimited()
        {
            if (currentRotation >= minAngle && currentRotation <= maxAngle)
                RotateUnlimited();
            else if (currentRotation < minAngle)
                CompensateRotation(minAngle);
            else if (currentRotation > maxAngle)
                CompensateRotation(maxAngle);
        }

        void RotateUnlimited()
        {
            switch (axis)
            {
                case Axis.x:
                    Rotate(angle, 0, 0);
                    break;
                case Axis.y:
                    Rotate(0, angle, 0);
                    break;
                case Axis.z:
                    Rotate(0, 0, angle);
                    break;
            }
        }

        void Rotate(float xAngle, float yAngle, float zAngle)
        {
            if (rotationTime > 0)
            {
                endAngle = xAngle + yAngle + zAngle;

                if (rotationTime - timePassed > 0)
                {
                    deltaRotation.x = xAngle;
                    deltaRotation.y = yAngle;
                    deltaRotation.z = zAngle;
                    deltaRotation = deltaRotation.normalized;
                    deltaRotation *= (Time.deltaTime * Mathf.Abs(endAngle)) / rotationTime;

                    RotateObjectByAngle(deltaRotation.x, deltaRotation.y, deltaRotation.z, Space.Self);

                    timePassed += Time.deltaTime;
                    summDeltaRotation += deltaRotation.x + deltaRotation.y + deltaRotation.z;
                    currentRotation += deltaRotation.x + deltaRotation.y + deltaRotation.z;
                }
                else
                {
                    if (!rotateAroudPoint)
                    {
                        transform.localRotation = startRotation;
                        RotateObjectByAngle(xAngle, yAngle, zAngle, Space.Self);
                    }

                    deltaOverRotation = summDeltaRotation - endAngle;
                    currentRotation -= deltaOverRotation;
                    UpdateStartRotation();
                }
            }
            else if (!useLimitations)
            {
                currentRotation += angle;
                RotateObjectByAngle(xAngle, yAngle, zAngle, Space.Self);
                UpdateStartRotation();
            }
            else if (useLimitations)
            {
                currentRotation += angle;
                RotateObjectByAngle(xAngle, yAngle, zAngle, Space.Self);

                if (currentRotation >= minAngle && currentRotation <= maxAngle)
                    UpdateStartRotation();
                else if (currentRotation < minAngle)
                    CompensateRotation(minAngle);
                else if (currentRotation > maxAngle)
                    CompensateRotation(maxAngle);
            }
        }

        void UpdateStartRotation()
        {
            isRotating = false;

            startRotation = transform.localRotation;
            deltaRotation = Vector3.zero;
            timePassed = 0;
            summDeltaRotation = 0;
            deltaOverRotation = 0;
            currentRotation = Mathf.Round(currentRotation);
        }

        public void ActivateRotation()
        {
            UpdateStartRotation();
            isRotating = true;
        }

        public void DeactivateRotation()
        {
            isRotating = false;
        }

        void CompensateRotation(int angle)
        {
            deltaOverRotation = currentRotation - angle;

            switch (axis)
            {
                case Axis.x:
                    RotateObjectByAngle(-deltaOverRotation, 0, 0, Space.Self);
                    break;
                case Axis.y:
                    RotateObjectByAngle(0, -deltaOverRotation, 0, Space.Self);
                    break;
                case Axis.z:
                    RotateObjectByAngle(0, 0, -deltaOverRotation, Space.Self);
                    break;
            }

            currentRotation = angle;
            UpdateStartRotation();
        }

        void RotateObjectByAngle(float INxAngle, float INyAngle, float INzAngle, Space space)
        {
            if (rotateAroudPoint && rotationPoint)
                RotateObjectAroundAxis(INxAngle, INyAngle, INzAngle, rotationPoint);
            else
                transform.Rotate(INxAngle, INyAngle, INzAngle, space);
        }

        void RotateObjectAroundAxis(float INxAngle, float INyAngle, float INzAngle, Transform point)
        {
            float thisAngle = INxAngle + INyAngle + INzAngle;
            switch (axis)
            {
                case Axis.x:
                    transform.RotateAround(point.position, Vector3.left, thisAngle);
                    break;
                case Axis.y:
                    transform.RotateAround(point.position, Vector3.up, thisAngle);
                    break;
                case Axis.z:
                    transform.RotateAround(point.position, Vector3.back, thisAngle);
                    break;
            }
        }
    }
}
