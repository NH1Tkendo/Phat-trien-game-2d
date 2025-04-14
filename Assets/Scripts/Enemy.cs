using UnityEngine;

public class Enemy : MonoBehaviour
{
	public Transform target;
	public float moveSpeed = 3f;

	void Update()
	{
		if (target != null)
		{
			Vector3 direction = (target.position - transform.position).normalized;
			transform.position += direction * moveSpeed * Time.deltaTime;
		}
	}
}
