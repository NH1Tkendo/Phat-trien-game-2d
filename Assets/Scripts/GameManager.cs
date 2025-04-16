using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	[Header("References")]
	public Spawner spawnerPrefab;
	public TextMeshProUGUI roundIndi;

	[Header("Spawn Settings")]
	private int portalsPerRound = 1;
	private float xSpawnRange = 30f; 
	private float ySpawnPosition = 0f;
	private float currentPortalDuration = 10f;
	private float currentEnemyInterval = 2f;

	private int currentRound = 0;
	private List<Spawner> activeSpawners = new List<Spawner>();

	public static GameManager Instance;

	public int score = 0;
	public TextMeshProUGUI scoreText;

	private int maxRound = 8;
	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}
	void Start()
	{
		StartCoroutine(RoundLoop());
	}

	private IEnumerator RoundLoop()
	{
		while (true)
		{
			yield return new WaitForSeconds(1.5f);

			currentRound++;
			if (maxRound == currentRound)
			{
				SaveManager.SaveCoins(score);
				SceneManager.LoadScene("Limbo");
			}
			DifficultSetting(currentRound);
			roundIndi.text = "Round: " + currentRound;

			// Spawn nhiều portal cùng lúc
			SpawnPortals();

			// Đợi cho đến khi tất cả portal hoàn tất (tức đã tự đóng)
			yield return new WaitUntil(() => AllPortalsClosed());

			// Chờ thêm một chút trước khi bắt đầu round mới
			yield return new WaitForSeconds(1.5f);
		}
	}

	private void DifficultSetting(int currentRound)
	{
		switch (currentRound)
		{
			case 2:
				currentPortalDuration = 10f;
				currentEnemyInterval = 1.5f;
				break;
			case 3:
				portalsPerRound = 2;
				currentPortalDuration = 10f;
				currentEnemyInterval = 2f;
				break;
			case 4:
				currentPortalDuration = 6f;
				currentEnemyInterval = 3f;
				break;
			case 5:
				portalsPerRound = 3;
				currentPortalDuration = 9f;
				currentEnemyInterval = 2f;
				break;
			case 7:
				portalsPerRound = 1;
				currentPortalDuration = 1f;
				currentEnemyInterval = 1f;
				break;
			case 8:
				portalsPerRound = 0;
				currentPortalDuration = 0f;
				currentEnemyInterval = 0f;
				break;

		}
	}
	private void SpawnPortals()
	{
		activeSpawners.Clear();

		for (int i = 0; i < portalsPerRound; i++)
		{
			Vector3 spawnPos = new Vector3(Random.Range(-xSpawnRange, xSpawnRange), ySpawnPosition, 0);
			Spawner newSpawner = Instantiate(spawnerPrefab, transform); // hoặc không cần parent nếu muốn

			newSpawner.SetPortalActiveDuration(currentPortalDuration);
			newSpawner.SetSpawnInteval(currentEnemyInterval);
			newSpawner.StartSpawning(spawnPos);

			activeSpawners.Add(newSpawner);
		}
	}

	private bool AllPortalsClosed()
	{
		foreach (var spawner in activeSpawners)
		{
			if (spawner.IsSpawning())
				return false;
		}
		return true;
	}
	public void AddScore(int value)
	{
		score += value;
		if (scoreText != null)
			scoreText.text = "" + score;
	}
}
