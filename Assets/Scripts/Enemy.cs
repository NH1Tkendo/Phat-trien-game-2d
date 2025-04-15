using UnityEngine;

public class Enemy : MonoBehaviour
{
	public Transform target;
	public float moveSpeed = 3f;

	void Update()
	{
		// Nếu chưa có target thì tìm lại
		if (target == null)
		{
			GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
			if (playerObj != null)
			{
				target = playerObj.transform;
			}
		}

		if (target != null)
		{
			Vector3 direction = (target.position - transform.position).normalized;
			transform.position += direction * moveSpeed * Time.deltaTime;
		}
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Enemy"))
		{
			Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.collider);
		}
	}
}
