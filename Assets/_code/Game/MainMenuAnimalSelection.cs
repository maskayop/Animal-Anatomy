using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vopere.Common;

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
        [SerializeField] List<AnimalSelection> XR_Animals = new List<AnimalSelection>();

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
            EnableAnimals(animals, false);
            EnableAnimals(XR_Animals, false);

            StartCoroutine(InitDelayed());
        }

        IEnumerator InitDelayed()
        {
            yield return new WaitForSeconds(1.5f);

            if (App.Instance.isXR)
            {
                DestroyAnimals(animals);
                EnableAnimals(XR_Animals, true);
            }
            else
            {
                DestroyAnimals(XR_Animals);
                EnableAnimals(animals, true);
            }

            UnselectAll();
        }

        public void SelectAnimal(int id)
        {
            UnselectAll();

            if (App.Instance && App.Instance.isXR)
                XR_Animals[id].Select(true);
            else
                animals[id].Select(true);
        }

        void UnselectAll()
        {
            if (App.Instance && App.Instance.isXR)
                for (int i = 0; i < XR_Animals.Count; i++)
                    XR_Animals[i].Select(false);
            else
                for (int i = 0; i < animals.Count; i++)
                    animals[i].Select(false);
        }

        void EnableAnimals(List<AnimalSelection> list, bool state)
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i].selected.SetActive(state);
                list[i].unselected.SetActive(state);
            }
        }

        void DestroyAnimals(List<AnimalSelection> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                DestroyImmediate(list[i].selected);
                DestroyImmediate(list[i].unselected);
            }
        }
    }
}
