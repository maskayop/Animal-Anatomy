using System.Collections;
using TMPro;
using UnityEngine;

namespace AnimalAnatomy
{
    public class UI3D_MenuCanvas : MonoBehaviour
    {
        public static UI3D_MenuCanvas Instance;

        [Header("UI")]
        [SerializeField] UIMenuWindow menuWindow;
        [SerializeField] TextMeshProUGUI versionText;

        Canvas canvas;

        void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("Cannot create UI3D_MenuCanvas");
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        void Start()
        {
            canvas = GetComponent<Canvas>();
            CloseMenuWindow();
            StartCoroutine(InitializeDelayed());

            if (versionText != null)
                versionText.text = Application.version;
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
            CloseMenuWindow();
        }

        public void OpenMenuWindow()
        {
            menuWindow.gameObject.SetActive(true);
        }

        public void CloseMenuWindow()
        {
            menuWindow.gameObject.SetActive(false);
        }

        public bool IsMenuWindowActive()
        {
            return menuWindow.gameObject.activeInHierarchy;
        }
    }
}
