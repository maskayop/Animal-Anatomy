using UnityEngine;

namespace AnimalAnatomy
{
    public class UI3D_BodyPartDescriptionCanvas : MonoBehaviour
    {
        public static UI3D_BodyPartDescriptionCanvas Instance;

        [Header("UI")]
        [SerializeField] UIBodyPartDescriptionPanel bodyPartDescriptionPanel;

        Canvas canvas;

        void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("Cannot create UI3D_BodyPartDescriptionCanvas");
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        void Start()
        {
            canvas = GetComponent<Canvas>();
        }

        void Update()
        {
            if (canvas)
                transform.LookAt(canvas.worldCamera.transform);
        }

        public void OpenBodyPartDescriptionPanel()
        {
            bodyPartDescriptionPanel.gameObject.SetActive(true);
        }

        public void CloseBodyPartDescriptionPanel()
        {
            bodyPartDescriptionPanel.gameObject.SetActive(false);
        }

        public void SelectBodyPart(BodyPartInfo info)
        {
            bodyPartDescriptionPanel.gameObject.SetActive(true);
            bodyPartDescriptionPanel.ShowInfo(info);
        }

        public void UnSelectBodyPart()
        {
            bodyPartDescriptionPanel.gameObject.SetActive(false);
        }

        public void SelectBodyPartGroup(BodyPartGroup info)
        {
            bodyPartDescriptionPanel.gameObject.SetActive(true);
            bodyPartDescriptionPanel.ShowInfo(info);
        }

        public void UnSelectBodyPartGroup()
        {
            bodyPartDescriptionPanel.gameObject.SetActive(false);
        }
    }
}
