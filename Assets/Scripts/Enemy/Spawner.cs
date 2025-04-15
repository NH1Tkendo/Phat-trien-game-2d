using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	public GameObject portalPrefab;         // Prefab của Portal
	public GameObject enemyPrefab;          // Prefab của quái
	public Transform player;                // Player để quái đuổi theo
	public float portalLifetime = 5f;       // Sống bao lâu trước khi biến mất
	public float enemySpawnInterval = 0.5f; // Thời gian spawn quái liên tiếp
	public Vector2[] spawnPositions;        // Các vị trí random để spawn Portal

	public float timeBetweenPortals = 10f;  // Khoảng cách giữa 2 đợt portal

	private void Start()
	{
		StartCoroutine(SpawnPortalLoop());
	}

	IEnumerator SpawnPortalLoop()
	{
		while (true)
		{
			SpawnPortal();
			yield return new WaitForSeconds(timeBetweenPortals);
		}
	}

	void SpawnPortal()
	{
		// Chọn vị trí random trong danh sách
		Vector2 spawnPos = spawnPositions[Random.Range(0, spawnPositions.Length)];
		GameObject portal = Instantiate(portalPrefab, spawnPos, Quaternion.identity);

		// Bắt đầu coroutine spawn quái trong một khoảng thời gian
		StartCoroutine(SpawnEnemiesFromPortal(portal.transform, portalLifetime));

		// Hủy cổng sau thời gian sống
		Destroy(portal, portalLifetime);
	}

	IEnumerator SpawnEnemiesFromPortal(Transform portalTransform, float duration)
	{
		float elapsed = 0f;
		while (elapsed < duration)
		{
			GameObject enemy = Instantiate(enemyPrefab, portalTransform.position, Quaternion.identity);
			Enemy followScript = enemy.GetComponent<Enemy>();
			if (followScript != null)
			{
				followScript.target = player;
			}

			yield return new WaitForSeconds(enemySpawnInterval);
			elapsed += enemySpawnInterval;
		}
	}
}
