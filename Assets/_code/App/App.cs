using UnityEngine;

namespace Vopere.Common
{
    public class App : MonoBehaviour
	{
		public static App Instance;

		[SerializeField] int defaultGraphicsLevel = 0;
		[SerializeField] bool useTargetFPS = true;
		[SerializeField] bool initialize = false;

		public bool IsInitialized {  get { return initialize; } }

		int graphicsLevel = 0;


        void Awake()
		{
			if (Instance != null)
			{
				Debug.LogWarning("Cannot create App");
				Destroy(gameObject);
				return;
			}

			Instance = this;

			Init();
        }

        public void Init()
        {
			SetTargetFPS(useTargetFPS);

            graphicsLevel = DataSaveLoad.Instance.GetSavedInt("GraphicsLevel");

            if (graphicsLevel != -1)
                SetGraphicsLevel(graphicsLevel);
			else
				SetGraphicsLevel(defaultGraphicsLevel);
        }

        public void ExitGame()
		{
			Debug.Log("Выход из программы" + "\n");

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
        }

        public void SetTargetFPS(bool value)
		{
			if (value)
				Application.targetFrameRate = 60;
			else
				Application.targetFrameRate = 30;
		}

		public void SetGraphicsLevel(int level)
		{
			QualitySettings.SetQualityLevel(level, true);
		}
	}
}
