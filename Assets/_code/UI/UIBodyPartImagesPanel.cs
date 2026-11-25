using UnityEngine;
using UnityEngine.UI;

namespace AnimalAnatomy
{
    public class UIBodyPartImagesPanel : MonoBehaviour
    {
        [SerializeField] GameObject panel;
        [SerializeField] Image image;

        public void Init(Sprite sprite)
        {
            image.sprite = sprite;
            image.preserveAspect = true;
        }

        public void Open()
        {
            panel.SetActive(true);
        }

        public void Close()
        {
            panel.SetActive(false);
        }
    }
}
