using UnityEngine;

namespace Vopere.UI
{
    public abstract class UISelectionButtonBase : MonoBehaviour
    {
        [SerializeField] protected GameObject selected;

        void Awake()
        {
            OnAwake();
        }

        protected virtual void OnAwake() { }

        void Start()
        {
            Init();
        }

        public void Init()
        {
            Select(false);
            OnInit();
        }

        protected virtual void OnInit() { }

        public void Select(bool state)
        {
            selected.SetActive(state);
        }
    }
}
