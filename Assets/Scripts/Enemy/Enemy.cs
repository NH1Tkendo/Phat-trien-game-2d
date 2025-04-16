using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public int health = 100;

	public GameObject deathEffect;

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

		if (GameManager.Instance != null)
			GameManager.Instance.AddScore(1);

		Destroy(gameObject);
		
	}

}
