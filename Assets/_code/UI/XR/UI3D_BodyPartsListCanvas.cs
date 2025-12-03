using System.Collections;
using UnityEngine;

namespace AnimalAnatomy
{
    public class UI3D_BodyPartsListCanvas : MonoBehaviour
    {
        public static UI3D_BodyPartsListCanvas Instance;

        [Header("UI")]
        [SerializeField] UIBodyPartsListPanel bodyPartsListPanel;

        Canvas canvas;

        void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("Cannot create UI3D_BodyPartsListCanvas");
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        void Start()
        {
            canvas = GetComponent<Canvas>();
            StartCoroutine(InitializeDelayed());
        }

        void Update()
        {
            if (canvas)
                transform.LookAt(canvas.worldCamera.transform);
        }

        IEnumerator InitializeDelayed()
        {
            yield return new WaitForSeconds(2.0f);

            Init();
        }

        public void Init()
        {
            bodyPartsListPanel.Init();
        }

        public void OpenPartsListPanel()
        {
            bodyPartsListPanel.gameObject.SetActive(true);
        }

        public void ClosePartsListPanel()
        {
            bodyPartsListPanel.gameObject.SetActive(false);
        }

        public bool IsBodyPartsListPanelActive()
        {
            return bodyPartsListPanel.gameObject.activeInHierarchy;
        }
    }
}
