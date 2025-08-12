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
        public List<BodyPartInfo> bodyParts = new List<BodyPartInfo>();
    }

    public class GameController : MonoBehaviour
    {
        public static GameController Instance;

        public enum SystemType { skeleton, muscles, nervous, circulatory, sensory, respiratory, digestive, excretory, reproductive, skin }
        public SystemType systemType;

        public List<BodyPartInfo> allBodyParts = new List<BodyPartInfo>();
        public List<BodyPartsList> bodyPartsLists = new List<BodyPartsList>();

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

                bodyPartsLists.Add(newBodyPartsList);
            }

            StartCoroutine(UpdateBodyPartsLists());
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
                        bodyPartsLists[i].bodyParts[l].gameObject.SetActive(state);
                }
            }
        }
    }
}
