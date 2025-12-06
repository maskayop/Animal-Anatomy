using UnityEngine;

namespace AnimalAnatomy
{
    public class UIWidgetsController : MonoBehaviour
    {
        [Header("Widgets")]
        [SerializeField] GameObject isolatedModeWidget;
        [SerializeField] GameObject transparentModeWidget;
        [SerializeField] GameObject exclusionModeWidget;
        [SerializeField] GameObject lightingModeWidget;

        GameController gameController;

        void Start()
        {
            Init();
        }

        void Update()
        {
            UpdateWidgets();
        }

        public void Init()
        {
            gameController = GameController.Instance;
        }

        void UpdateWidgets()
        {
            if (ExaminationController.Instance && ExaminationController.Instance.isExamination)
            {
                isolatedModeWidget.SetActive(false);
                transparentModeWidget.SetActive(false);
                exclusionModeWidget.SetActive(false);
                lightingModeWidget.SetActive(false);

                return;
            }
            
            isolatedModeWidget.SetActive(gameController.isolatedMode);
            transparentModeWidget.SetActive(gameController.transparentMode);
            exclusionModeWidget.SetActive(gameController.exclusionMode);
            lightingModeWidget.SetActive(gameController.lightingMode);
        }
    }
}
