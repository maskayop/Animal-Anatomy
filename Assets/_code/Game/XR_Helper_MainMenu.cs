using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using Vopere.Common;

namespace AnimalAnatomy
{
    public class XR_Helper_MainMenu : MonoBehaviour
    {
        [SerializeField] int gameId = -1;

        XRGrabInteractable grabInteractable;

        void Start()
        {
            if (!App.Instance.isXR)
            {
                Destroy(this);
                return;
            }

            Init();
        }

        public void Init()
        {            
            grabInteractable = GetComponent<XRGrabInteractable>();

            if (grabInteractable)
                grabInteractable.selectEntered.AddListener(OnSelectEntered);

            if (grabInteractable)
                grabInteractable.selectExited.AddListener(OnSelectExited);
        }

        void OnSelectEntered(SelectEnterEventArgs args)
        {
            SelectGame();
        }

        void OnSelectExited(SelectExitEventArgs args)
        {
            
        }

        void OnDestroy()
        {
            if (grabInteractable)
            {
                grabInteractable.selectEntered.RemoveListener(OnSelectEntered);
                grabInteractable.selectExited.RemoveListener(OnSelectExited);
            }
        }

        void SelectGame()
        {
            if (XR_MainMenu.Instance)
                XR_MainMenu.Instance.SelectGame(gameId);

            if (GameplayAudioPlayer.Instance)
                GameplayAudioPlayer.Instance.PlayBodyPartSelectionAudio();
        }
    }
}
