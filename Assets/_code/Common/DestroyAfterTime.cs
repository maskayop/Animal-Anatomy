using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
	[SerializeField] float time = 1.0f;
	[SerializeField] bool destroyAtStart = true;

	void Start()
	{
		if (destroyAtStart)
			DestroyGameObject();
	}

	public void DestroyGameObject()
	{
		Destroy(gameObject, time);
	}
}
