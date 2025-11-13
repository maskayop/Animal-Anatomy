using UnityEngine;

namespace Vopere.Common
{
    public class App : MonoBehaviour
	{
		public static App Instance;

		[SerializeField] int defaultGraphicsLevel = 0;
		[SerializeField] int screenResolutionLevel = 1;

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

			SetResolution(screenResolutionLevel);
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

		public void SetResolution(int level)
		{
			if (level == 0)
				Screen.SetResolution(1280, 720, FullScreenMode.FullScreenWindow);
			else if (level == 1)
                Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow);
            else if (level == 2)
                Screen.SetResolution(2560, 1440, FullScreenMode.FullScreenWindow);
			else
                Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow);
        }
	}
}
