using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace AnimalAnatomy
{
    public class MainMenuAnimalSelection : MonoBehaviour
    {
        public static MainMenuAnimalSelection Instance;

        void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("Cannot create MainMenuAnimalSelection");
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
    }
}
