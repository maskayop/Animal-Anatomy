using UnityEngine;

namespace AnimalAnatomy
{
	public class App : MonoBehaviour
	{
		public static App Instance;

		[SerializeField] bool useTargetFPS = true;
		[SerializeField] bool initialize = false;

		public bool IsInitialized {  get { return initialize; } }

		void Awake()
		{
			if (Instance != null)
			{
				Debug.LogWarning("Cannot create App");
				Destroy(gameObject);
				return;
			}

			Instance = this;

			SetTargetFPS(useTargetFPS);
		}

        public void ExitGame()
		{
			Application.Quit();
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
