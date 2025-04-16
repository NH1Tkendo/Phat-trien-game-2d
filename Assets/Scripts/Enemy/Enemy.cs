using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public int health = 100;//Máu quái

	public GameObject deathEffect;//Hiệu ứng chết cho quái

    public void TakeDamage (int damage)
	{
		health -= damage;

		if (health <= 0)
		{
			Die();
		}
	}

	void Die ()
	{
		GameObject death = Instantiate(deathEffect, transform.position, Quaternion.identity);
		Destroy(death, 2f);
		Destroy(gameObject);
	}
}
