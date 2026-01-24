using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vopere.Common;

namespace AnimalAnatomy
{
    [Serializable]
    public class BodyPartsList
    {
        public GameController.SystemType systemType;
        public bool isActive = true;
        public List<BodyPartInfo> bodyParts = new List<BodyPartInfo>();
        public UIPartsListButton partListButton;
    }

    public class GameController : MonoBehaviour
    {
        public static GameController Instance;

        public enum SystemType { skeleton, muscles, nervous, circulatory, sensory, respiratory, digestive, excretory, reproductive, skin, lymphatic, endocrine, none }
        public SystemType systemType;

        public List<BodyPartInfo> allBodyParts = new List<BodyPartInfo>();
        public List<BodyPartGroup> allBodyPartsGroups = new List<BodyPartGroup>();
        public List<BodyPartsList> bodyPartsLists = new List<BodyPartsList>();

        [Header("Info")]
        public BodyPartInfo selectedBodyPart;
        public BodyPartGroup selectedBodyPartsGroup;

        [Header("Modes")]
        public bool isolatedMode = false;
        public bool transparentMode = false;
        public bool exclusionMode = false;
        public bool lightingMode = false;

        [HideInInspector]
        public GameObject baseGameObject;
        [HideInInspector]
        public BodyPartGroup baseBodyPartGroup;

        ObjectRotator baseObjectRotator;
        ObjectScaler baseObjectScaler;

        [HideInInspector]
        public bool leftTriggerPushed = false;
        [HideInInspector]
        public bool rightTriggerPushed = false;
        
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

        void Update()
        {
            if (App.Instance && App.Instance.isXR)
                if (InputController.Instance)
                    exclusionMode = InputController.Instance.isAlternativeInput;
        }

        public void Init()
        {
            CreateBodyPartsLists();
            StartCoroutine(UpdateBodyPartsLists());
            UnSelectBodyPart(true);

            baseGameObject = GameObject.FindGameObjectWithTag("Player");
            baseBodyPartGroup = baseGameObject.GetComponent<BodyPartGroup>();
            baseObjectRotator = baseGameObject.GetComponent<ObjectRotator>();
            baseObjectScaler = baseGameObject.GetComponent<ObjectScaler>();
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
                else if (i == 12)
                    newBodyPartsList.systemType = SystemType.none;

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

            for (int i = 0; i < allBodyParts.Count; i++)
            {
                for (int l = 0; l < bodyPartsLists.Count; l++)
                {
                    bodyPartsLists[l].bodyParts.Sort((x, y) => { return x.info.russianName.CompareTo(y.info.russianName); });
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

            if (UIMainCanvas.Instance)
                UIMainCanvas.Instance.ActivateAllSystems(state);
        }

        public void DisableAllSystemsExceptSystem(SystemType systemType)
        {
            ActivateAllSystems(false);
            EnableSystem(systemType);
        }

        public void SelectBodyPart(BodyPartInfo info)
        {
            UnSelectBodyPart(true);
            UnSelectBodyPartGroup();

            selectedBodyPart = info;
            selectedBodyPartsGroup = null;

            if (UIMainCanvas.Instance)
                UIMainCanvas.Instance.SelectBodyPart(info);

            if (UI3D_CentralCanvas.Instance)
                UI3D_CentralCanvas.Instance.SelectBodyPart(info);

            info.Select(false);

            SetCameraDistanceLimitsMultiplier(selectedBodyPart.cameraDistanceLimitsMultiplier);

            if (GameplayAudioPlayer.Instance)
                GameplayAudioPlayer.Instance.PlayBodyPartSelectionAudio();
        }

        public void UnSelectBodyPart(bool playAudioClip)
        {
            selectedBodyPart = null;
            
            if (UIMainCanvas.Instance)
                UIMainCanvas.Instance.UnSelectBodyPart();

            if (UI3D_CentralCanvas.Instance)
                UI3D_CentralCanvas.Instance.UnSelectBodyPart();

            for (int i = 0; i < allBodyParts.Count; i++)
                allBodyParts[i].UnSelect();

            SetCameraDistanceLimitsMultiplier(1.0f);

            if (!playAudioClip)
                if (GameplayAudioPlayer.Instance)
                    GameplayAudioPlayer.Instance.PlayBodyPartUnSelectionAudio();
        }

        public void SelectBodyPartGroup(BodyPartGroup info)
        {
            UnSelectBodyPartGroup();

            selectedBodyPart = null;
            selectedBodyPartsGroup = info;

            if (UIMainCanvas.Instance)
                UIMainCanvas.Instance.SelectBodyPartGroup(info);

            if (UI3D_CentralCanvas.Instance)
                UI3D_CentralCanvas.Instance.SelectBodyPartGroup(info);

            info.Select();

            SetCameraDistanceLimitsMultiplier(selectedBodyPartsGroup.cameraDistanceLimitsMultiplier);
        }

        public void UnSelectBodyPartGroup()
        {
            selectedBodyPartsGroup = null;

            if (UIMainCanvas.Instance)
                UIMainCanvas.Instance.UnSelectBodyPartGroup();

            if (UI3D_CentralCanvas.Instance)
                UI3D_CentralCanvas.Instance.UnSelectBodyPartGroup();

            for (int i = 0; i < allBodyPartsGroups.Count; i++)
            {
                allBodyPartsGroups[i].UnSelect();
            }

            for (int i = 0; i < allBodyParts.Count; i++)
            {
                allBodyParts[i].UnSelect();
            }

            SetCameraDistanceLimitsMultiplier(1.0f);
        }

        public void SetIsolatedMode(bool state)
        {
            IsolateBodyPart(state);
            IsolateBodyPartGroup(state);

            isolatedMode = state;
        }

        void IsolateBodyPart(bool state)
        {
            if (selectedBodyPart == null)
                return;

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
                selectedBodyPart.Select(false);

                for (int i = 0; i < bodyPartsLists.Count; i++)
                {
                    for (int l = 0; l < bodyPartsLists[i].bodyParts.Count; l++)
                    {
                        bodyPartsLists[i].bodyParts[l].gameObject.SetActive(bodyPartsLists[i].isActive);
                    }
                }
            }

            if (CameraController.Instance)
                CameraController.Instance.UpdatePosition();
        }

        void IsolateBodyPartGroup(bool state)
        {
            if (selectedBodyPartsGroup == null)
                return;

            if (state)
            {
                selectedBodyPartsGroup.UnSelect();

                for (int i = 0; i < allBodyParts.Count; i++)
                {
                    allBodyParts[i].gameObject.SetActive(false);
                }

                for (int p = 0; p < selectedBodyPartsGroup.allChildrenBodyParts.Count; p++)
                {
                    selectedBodyPartsGroup.allChildrenBodyParts[p].gameObject.SetActive(true);
                }
            }
            else
            {
                selectedBodyPartsGroup.Select();

                for (int i = 0; i < bodyPartsLists.Count; i++)
                {
                    for (int l = 0; l < bodyPartsLists[i].bodyParts.Count; l++)
                    {
                        bodyPartsLists[i].bodyParts[l].gameObject.SetActive(bodyPartsLists[i].isActive);
                    }
                }
            }

            if (CameraController.Instance)
                CameraController.Instance.UpdatePosition();
        }

        public void SetTransparentMode(bool state)
        {
            IsolateBodyPartTransparent(state);
            IsolateBodyPartGroupTransparent(state);

            transparentMode = state;
        }

        void IsolateBodyPartTransparent(bool state)
        {
            if (selectedBodyPart == null)
                return;

            if (state)
                selectedBodyPart.UnSelect();
            else
                selectedBodyPart.Select(false);

            for (int i = 0; i < allBodyParts.Count; i++)
            {
                if (allBodyParts[i] != selectedBodyPart)
                    allBodyParts[i].SetAsTransparent(state);
            }

            if (CameraController.Instance)
                CameraController.Instance.UpdatePosition();
        }

        void IsolateBodyPartGroupTransparent(bool state)
        {
            if (selectedBodyPartsGroup == null)
                return;

            for (int i = 0; i < allBodyParts.Count; i++)
            {
                allBodyParts[i].SetAsTransparent(state);
            }

            for (int i = 0; i < selectedBodyPartsGroup.allChildrenBodyParts.Count; i++)
            {
                selectedBodyPartsGroup.allChildrenBodyParts[i].SetAsTransparent(false);
            }

            if (state)
                selectedBodyPartsGroup.UnSelect();
            else
                selectedBodyPartsGroup.Select();

            if (CameraController.Instance)
                CameraController.Instance.UpdatePosition();
        }

        public void HideSelectedBodyPart()
        {
            selectedBodyPart.UnSelect();

            for (int i = 0; i < allBodyParts.Count; i++)
            {
                if (allBodyParts[i] == selectedBodyPart)
                    selectedBodyPart.gameObject.SetActive(false);
            }

            UnSelectBodyPart(true);
        }

        public void SetExclusionMode(bool state)
        {
            exclusionMode = state;
        }

        public void SetLightingMode(bool state)
        {
            lightingMode = state;
        }

        void SetCameraDistanceLimitsMultiplier(float value)
        {
            if (CameraController.Instance)
                CameraController.Instance.distanceLimitsMultiplier = value;
        }

        public void ChangeBaseObjectRotationAngle(float INvalue)
        {
            baseObjectRotator.angle = INvalue / 2;
            DataSaveLoad.Instance.Save("XR_RotationSensitivity", INvalue);
        }

        public void ChangeBaseObjectScalingDelta(float INvalue)
        {
            baseObjectScaler.scaleDelta = INvalue / 200;
            DataSaveLoad.Instance.Save("XR_ScalingSensitivity", INvalue);
        }
    }
}
