using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class StartScreen : MonoBehaviour
{
	public Transform cameraTransform; // Camera chính
	public float moveSpeed = 2f; // Tốc độ di chuyển
	public float targetY = -5f; // Vị trí camera sẽ đi tới

	private bool isMoving = false;

	void Update()
	{
		if (!isMoving && Input.anyKeyDown)
		{
			isMoving = true;
			StartCoroutine(MoveCameraDown());
		}
	}

	IEnumerator MoveCameraDown()
	{
		while (cameraTransform.position.y > targetY)
		{
			cameraTransform.position = Vector3.MoveTowards(
				cameraTransform.position,
				new Vector3(cameraTransform.position.x, targetY, cameraTransform.position.z),
				moveSpeed * Time.deltaTime
			);

			yield return null; // Chờ 1 frame
		}
		Debug.Log("Camera reached target!");
		SceneManager.LoadScene("WaitingHall");
	}
}
