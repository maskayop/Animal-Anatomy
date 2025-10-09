using System;
using System.Collections.Generic;
using UnityEngine;

namespace AnimalAnatomy
{
    [Serializable]
    public class AnimalSelection
    {
        public GameObject selected;
        public GameObject unselected;

        public void Select(bool state)
        {
            selected.SetActive(state);
            unselected.SetActive(!state);
        }
    }

    public class MainMenuAnimalSelection : MonoBehaviour
    {
        public static MainMenuAnimalSelection Instance;

        [SerializeField] List<AnimalSelection> animals = new List<AnimalSelection>();

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
            UnselectAll();
        }

        public void SelectAnimal(int id)
        {
            UnselectAll();

            animals[id].Select(true);
        }

        void UnselectAll()
        {
            for (int i = 0; i < animals.Count; i++)
            {
                animals[i].Select(false);
            }
        }
    }
}
