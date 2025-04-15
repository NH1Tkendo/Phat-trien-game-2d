using System.Collections;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	[Header("Prefabs")]
	public GameObject portalPrefab;
	public GameObject[] enemyPrefab;

	[Header("Spawn Settings")]
	private float portalActiveDuration = 10f;// thời gian portal mở

	public void SetPortalActiveDuration(float duration)
	{
		portalActiveDuration = duration;
	}
	
	private float spawnInterval = 2f; // khoảng cách thời gian giữa các enemy
	public void SetSpawnInteval(float time)
	{
		spawnInterval = time;
	}

	private GameObject currentPortal;
	private bool isSpawning = false;

	/// <summary>
	/// Gọi từ GameManager để bắt đầu quá trình spawn
	/// </summary>
	/// <param name="position">Vị trí spawn portal</param>
	public void StartSpawning(Vector3 position)
	{
		if (isSpawning) return;
		StartCoroutine(SpawnRoutine(position));
	}

	private IEnumerator SpawnRoutine(Vector3 position)
	{
		isSpawning = true;

		// 1. Tạo portal
		currentPortal = Instantiate(portalPrefab, position, Quaternion.identity);

		float elapsedTime = 0f;

		// 2. Bắt đầu spawn enemy cho đến khi hết thời gian portal
		while (elapsedTime < portalActiveDuration)
		{
			SpawnEnemy(position);
			yield return new WaitForSeconds(spawnInterval);
			elapsedTime += spawnInterval;
		}

		// 3. Đóng portal
		if (currentPortal != null)
		{
			Destroy(currentPortal);
		}

		isSpawning = false;
	}

	private void SpawnEnemy(Vector3 position)
	{
		GameObject enemy = Instantiate(enemyPrefab[Random.Range(0, enemyPrefab.Count())], position, Quaternion.identity);

		// Gán target là player ngay khi spawn
		Transform player = GameObject.FindGameObjectWithTag("Player")?.transform;
		if (player != null)
		{
			enemy.GetComponent<Enemy>().target = player;
		}
	}

	public bool IsSpawning()
	{
		return isSpawning;
	}
}
