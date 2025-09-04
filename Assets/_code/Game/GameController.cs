using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimalAnatomy
{
    [Serializable]
    public class BodyPartsList
    {
        public GameController.SystemType systemType;
        public bool isActive = true;
        public List<BodyPartInfo> bodyParts = new List<BodyPartInfo>();
    }

    public class GameController : MonoBehaviour
    {
        public static GameController Instance;

        public enum SystemType { skeleton, muscles, nervous, circulatory, sensory, respiratory, digestive, excretory, reproductive, skin, lymphatic, endocrine }
        public SystemType systemType;

        public List<BodyPartInfo> allBodyParts = new List<BodyPartInfo>();
        public List<BodyPartsList> bodyPartsLists = new List<BodyPartsList>();

        [Header("Materials")]
        public Material selectedMaterial;
        public Material transparentModeMaterial;

        [Header("Info")]
        public BodyPartInfo selectedBodyPart;
        public bool isolatedMode = false;
        public bool transparentMode = false;

        void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("Cannot create GameController");
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
            CreateBodyPartsLists();
            StartCoroutine(UpdateBodyPartsLists());
            UnSelectBodyPart();
        }

        void CreateBodyPartsLists()
        {
            int count = Enum.GetValues(typeof(SystemType)).Length;

            for (int i = 0; i < count; i++)
            {
                BodyPartsList newBodyPartsList = new BodyPartsList();

                if (i == 0)
                    newBodyPartsList.systemType = SystemType.skeleton;
                else if (i == 1)
                    newBodyPartsList.systemType = SystemType.muscles;
                else if (i == 2)
                    newBodyPartsList.systemType = SystemType.nervous;
                else if (i == 3)
                    newBodyPartsList.systemType = SystemType.circulatory;
                else if (i == 4)
                    newBodyPartsList.systemType = SystemType.sensory;
                else if (i == 5)
                    newBodyPartsList.systemType = SystemType.respiratory;
                else if (i == 6)
                    newBodyPartsList.systemType = SystemType.digestive;
                else if (i == 7)
                    newBodyPartsList.systemType = SystemType.excretory;
                else if (i == 8)
                    newBodyPartsList.systemType = SystemType.reproductive;
                else if (i == 9)
                    newBodyPartsList.systemType = SystemType.skin;
                else if (i == 10)
                    newBodyPartsList.systemType = SystemType.lymphatic;
                else if (i == 11)
                    newBodyPartsList.systemType = SystemType.endocrine;

                bodyPartsLists.Add(newBodyPartsList);
            }
        }

        IEnumerator UpdateBodyPartsLists()
        {
            yield return new WaitForSeconds(1.0f);

            for (int i = 0; i < allBodyParts.Count; i++)
            {
                for (int l = 0; l < bodyPartsLists.Count; l++)
                {
                    if (bodyPartsLists[l].systemType == allBodyParts[i].systemType)
                        bodyPartsLists[l].bodyParts.Add(allBodyParts[i]);
                }
            }
        }

        public void EnableSystem(SystemType type)
        {
            ActivateSystem(type, true);
        }

        public void DisableSystem(SystemType type)
        {
            ActivateSystem(type, false);
        }

        void ActivateSystem(SystemType type, bool state)
        {
            for (int i = 0; i < bodyPartsLists.Count; i++)
            {
                for (int l = 0; l < bodyPartsLists[i].bodyParts.Count; l++)
                {
                    if (bodyPartsLists[i].systemType == type)
                    {
                        bodyPartsLists[i].isActive = state;
                        bodyPartsLists[i].bodyParts[l].gameObject.SetActive(state);
                    }
                }
            }
        }

        public void ActivateAllSystems(bool state)
        {
            for (int i = 0; i < bodyPartsLists.Count; i++)
            {
                for (int l = 0; l < bodyPartsLists[i].bodyParts.Count; l++)
                {
                    bodyPartsLists[i].isActive = state;
                    bodyPartsLists[i].bodyParts[l].gameObject.SetActive(state);
                }
            }

            UIMainCanvas.Instance.ActivateAllSystems(state);
        }

        public void SelectBodyPart(BodyPartInfo info)
        {
            UnSelectBodyPart();

            selectedBodyPart = info;
            UIMainCanvas.Instance.SelectBodyPart(info);
            info.Select();

            CameraController.Instance.distanceLimitsMultiplier = selectedBodyPart.cameraDistanceLimitsMultiplier;
        }

        public void UnSelectBodyPart()
        {
            selectedBodyPart = null;
            UIMainCanvas.Instance.UnSelectBodyPart();

            for (int i = 0; i < allBodyParts.Count; i++)
            {
                allBodyParts[i].UnSelect();
            }

            CameraController.Instance.distanceLimitsMultiplier = 1.0f;
        }

        public void SetIsolatedMode(bool state)
        {
            IsolateBodyPart(state);
            UIMainCanvas.Instance.SetIsolatedMode(state);
        }

        void IsolateBodyPart(bool state)
        {
            if (selectedBodyPart == null)
                return;

            isolatedMode = state;

            if (state)
            {
                selectedBodyPart.UnSelect();

                for (int i = 0; i < allBodyParts.Count; i++)
                {
                    allBodyParts[i].gameObject.SetActive(false);

                    if (allBodyParts[i] == selectedBodyPart)
                        selectedBodyPart.gameObject.SetActive(true);
                }
            }
            else
            {
                selectedBodyPart.Select();

                for (int i = 0; i < bodyPartsLists.Count; i++)
                {
                    for (int l = 0; l < bodyPartsLists[i].bodyParts.Count; l++)
                    {
                        bodyPartsLists[i].bodyParts[l].gameObject.SetActive(bodyPartsLists[i].isActive);
                    }
                }
            }

            CameraController.Instance.UpdatePosition();
        }

        public void SetTransparentMode(bool state)
        {
            IsolateBodyPartTransparent(state);
            UIMainCanvas.Instance.SetTransparentMode(state);
        }

        void IsolateBodyPartTransparent(bool state)
        {
            if (selectedBodyPart == null)
                return;

            transparentMode = state;

            if (state)
                selectedBodyPart.UnSelect();
            else
                selectedBodyPart.Select();

            for (int i = 0; i < allBodyParts.Count; i++)
            {
                if (allBodyParts[i] != selectedBodyPart)
                    allBodyParts[i].SetAsTransparent(state);
            }

            CameraController.Instance.UpdatePosition();
        }
    }
}
