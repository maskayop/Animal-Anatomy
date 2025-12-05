using UnityEngine;

namespace AnimalAnatomy
{
    public class UIAlternativeInputWidget : MonoBehaviour
    {
        [SerializeField] GameObject panel;

        void Update()
        {
            if (!InputController.Instance)
                return;

            if (InputController.Instance.isAlternativeInput)
                panel.SetActive(true);            
            else
                panel.SetActive(false);
        }
    }
}
