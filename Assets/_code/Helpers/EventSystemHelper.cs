using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using Vopere.Common;

namespace AnimalAnatomy
{
    public class EventSystemHelper : MonoBehaviour
    {
        public EventSystem[] allEventSystems;

        void Start()
        {
            UpdateAllEventSystems();
            StartCoroutine(InitDelayed());
        }

        IEnumerator InitDelayed()
        {
            yield return new WaitForSeconds(3.0f);

            Init();
        }

        public void Init()
        {
            if (App.Instance && App.Instance.isXR)
            {
                allEventSystems = FindObjectsByType<EventSystem>(FindObjectsSortMode.None);

                if (allEventSystems.Length > 0)
                {
                    EventSystem.current = allEventSystems[0];
                    DontDestroyOnLoad(allEventSystems[0]);
                }
            }
        }

        public void UpdateAllEventSystems()
        {
            allEventSystems = FindObjectsByType<EventSystem>(FindObjectsSortMode.None);

            if (App.Instance && App.Instance.isXR)
                for (int i = 1; i < allEventSystems.Length; i++)
                    Destroy(allEventSystems[i].gameObject);
        }
    }
}
