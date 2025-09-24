using System;
using System.Collections.Generic;
using UnityEngine;

namespace AnimalAnatomy
{
    [Serializable]
    public class SystemColor
    {
        public GameController.SystemType systemType;
        public Color color;
    }

    public class ColorsManager : MonoBehaviour
    {
        public static ColorsManager Instance;

        public List<SystemColor> systemsColors = new List<SystemColor>();

        void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("Cannot create ColorsManager");
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        public Color GetSystemColor(GameController.SystemType type)
        {
            for (int i = 0; i < systemsColors.Count; i++)
            {
                if (systemsColors[i].systemType == type)
                {
                    return systemsColors[i].color;
                }
            }

            return Color.white;
        }
    }
}
