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
        public string sceneName;
        public GameObject container;
        public GameObject selected;
        public GameObject unselected;

        public void Select(bool state)
        {
            selected.SetActive(state);
            unselected.SetActive(false);
        }
    }

    public class MainMenuAnimalSelection : MonoBehaviour
    {
        public static MainMenuAnimalSelection Instance;

        [SerializeField] List<AnimalSelection> animals = new List<AnimalSelection>();
        [SerializeField] List<AnimalSelection> XR_Animals = new List<AnimalSelection>();

        App app;

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
            app = App.Instance;

            EnableAnimals(animals, false);
            EnableAnimals(XR_Animals, false);

            StartCoroutine(InitDelayed());
        }

        IEnumerator InitDelayed()
        {
            yield return new WaitForSeconds(1.5f);

            if (app)
            {
                if (app.isXR)
                {
                    DestroyAnimals(animals);
                    EnableAnimals(XR_Animals, true);
                }
                else
                {
                    DestroyAnimals(XR_Animals);
                    EnableAnimals(animals, false);
                }

                UnselectAll();
            }
        }

        public void SelectAnimalBySceneName(string INname)
        {
            if (!app) return;

            List<AnimalSelection> animalsSelection = new List<AnimalSelection>();

            if (!app.isXR)
                animalsSelection = animals;
            else
                animalsSelection = XR_Animals;

            for (int i = 0; i < animalsSelection.Count; i++)
                if (animalsSelection[i].sceneName == INname)
                    SelectAnimal(i);
        }

        public void SelectAnimal(int id)
        {
            if (!app) return;

            UnselectAll();

            if (app.isXR)
            {
                if (XR_Animals.Count > id)
                    XR_Animals[id].Select(true);
            }
            else
            {
                if (animals.Count > id)
                    animals[id].Select(true);
            }
        }

        void UnselectAll()
        {
            if (!app) return;

            if (app.isXR)
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
                if (list[i].container != null)
                    DestroyImmediate(list[i].container);
        }
    }
}
