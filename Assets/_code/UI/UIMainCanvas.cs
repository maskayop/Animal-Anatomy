using UnityEngine;


namespace AnimalAnatomy
{
    public class UIMainCanvas : MonoBehaviour
    {
        public static UIMainCanvas Instance;
        
        void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("Cannot create UIMainCanvas");
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }
        
        void Start()
        {
            Init();
        }
        
        void Update()
        {

        }
        
        public void Init()
        {

        }

        public void ActivateSystemSkeleton(bool state)
        {
            if (state)
                GameController.Instance.EnableSystem(GameController.SystemType.skeleton);
            else
                GameController.Instance.DisableSystem(GameController.SystemType.skeleton);
        }

        public void ActivateSystemMuscles(bool state)
        {
            if (state)
                GameController.Instance.EnableSystem(GameController.SystemType.muscles);
            else
                GameController.Instance.DisableSystem(GameController.SystemType.muscles);
        }

        public void ActivateSystemNervous(bool state)
        {
            if (state)
                GameController.Instance.EnableSystem(GameController.SystemType.nervous);
            else
                GameController.Instance.DisableSystem(GameController.SystemType.nervous);
        }

        public void ActivateSystemCirculatory(bool state)
        {
            if (state)
                GameController.Instance.EnableSystem(GameController.SystemType.circulatory);
            else
                GameController.Instance.DisableSystem(GameController.SystemType.circulatory);
        }
        
        public void ActivateSystemSensory(bool state)
        {
            if (state)
                GameController.Instance.EnableSystem(GameController.SystemType.sensory);
            else
                GameController.Instance.DisableSystem(GameController.SystemType.sensory);
        }

        public void ActivateSystemRespiratory(bool state)
        {
            if (state)
                GameController.Instance.EnableSystem(GameController.SystemType.respiratory);
            else
                GameController.Instance.DisableSystem(GameController.SystemType.respiratory);
        }

        public void ActivateSystemDigestive(bool state)
        {
            if (state)
                GameController.Instance.EnableSystem(GameController.SystemType.digestive);
            else
                GameController.Instance.DisableSystem(GameController.SystemType.digestive);
        }

        public void ActivateSystemExcretory(bool state)
        {
            if (state)
                GameController.Instance.EnableSystem(GameController.SystemType.excretory);
            else
                GameController.Instance.DisableSystem(GameController.SystemType.excretory);
        }

        public void ActivateSystemReproductive(bool state)
        {
            if (state)
                GameController.Instance.EnableSystem(GameController.SystemType.reproductive);
            else
                GameController.Instance.DisableSystem(GameController.SystemType.reproductive);
        }

        public void ActivateSystemSkin(bool state)
        {
            if (state)
                GameController.Instance.EnableSystem(GameController.SystemType.skin);
            else
                GameController.Instance.DisableSystem(GameController.SystemType.skin);
        }

        public void ActivateAllSystems(bool state)
        {
            if (state)
            {
                GameController.Instance.EnableSystem(GameController.SystemType.skeleton);
                GameController.Instance.EnableSystem(GameController.SystemType.muscles);
                GameController.Instance.EnableSystem(GameController.SystemType.nervous);
                GameController.Instance.EnableSystem(GameController.SystemType.circulatory);
                GameController.Instance.EnableSystem(GameController.SystemType.sensory);
                GameController.Instance.EnableSystem(GameController.SystemType.respiratory);
                GameController.Instance.EnableSystem(GameController.SystemType.digestive);
                GameController.Instance.EnableSystem(GameController.SystemType.excretory);
                GameController.Instance.EnableSystem(GameController.SystemType.reproductive);
                GameController.Instance.EnableSystem(GameController.SystemType.skin);
            }
            else
            {
                GameController.Instance.DisableSystem(GameController.SystemType.skeleton);
                GameController.Instance.DisableSystem(GameController.SystemType.muscles);
                GameController.Instance.DisableSystem(GameController.SystemType.nervous);
                GameController.Instance.DisableSystem(GameController.SystemType.circulatory);
                GameController.Instance.DisableSystem(GameController.SystemType.sensory);
                GameController.Instance.DisableSystem(GameController.SystemType.respiratory);
                GameController.Instance.DisableSystem(GameController.SystemType.digestive);
                GameController.Instance.DisableSystem(GameController.SystemType.excretory);
                GameController.Instance.DisableSystem(GameController.SystemType.reproductive);
                GameController.Instance.DisableSystem(GameController.SystemType.skin);
            }
        }
    }
}
