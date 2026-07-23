using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AnimalAnatomy
{
    public class EventSystemHelper : MonoBehaviour
    {
        [SerializeField] GameObject eventSystemPrefab;

        public EventSystem[] allEventSystems;

        void Start()
        {
            Init();
        }

        public void Init()
        {
            allEventSystems = FindObjectsByType<EventSystem>();

            if (allEventSystems.Length > 1)
            {
                for (int i = 1; i < allEventSystems.Length; i++)
                    Destroy(allEventSystems[i].gameObject);
            }

            StartCoroutine(InitDelayed());
        }

        IEnumerator InitDelayed()
        {
            yield return new WaitForSeconds(1.0f);

            allEventSystems = FindObjectsByType<EventSystem>();

            if (allEventSystems.Length == 0)
            {
                GameObject newES = Instantiate(eventSystemPrefab);
                EventSystem.current = newES.GetComponent<EventSystem>();
            }
            else
                EventSystem.current = allEventSystems[0];
        }
    }
}
