using UnityEngine;

public class Bullet : MonoBehaviour
{
	public float speed = 20f;
	public int damage = 10;
	public Rigidbody2D rb;
	public GameObject impactEffect;
	public float bulletLifetime = 3f;

	void Start()
	{
		rb.linearVelocity = transform.right * speed;
	}

	void OnTriggerEnter2D(Collider2D hitInfo)
	{
		Enemy enemy = hitInfo.GetComponent<Enemy>();
		if (enemy != null)
		{
			enemy.TakeDamage(damage);
		}

		if (impactEffect != null)
		{
			GameObject effect = Instantiate(impactEffect, transform.position, transform.rotation);
			Destroy(effect, 2f); // chỉ phá hủy bản sao
		}

		Destroy(gameObject); // hủy viên đạn
	}
}

