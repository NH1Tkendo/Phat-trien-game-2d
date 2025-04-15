using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class PortalScript : MonoBehaviour
{
	private bool isPlayerNearby = false;

	void Start()
	{

	}
	void Update()
	{
		if (isPlayerNearby && Input.GetKeyDown(KeyCode.Return))
		{
			Debug.Log("Player entered the portal!");
			SceneManager.LoadScene("The Invasion");
		}
	}
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			isPlayerNearby = true;
			Debug.Log("Player is nearby!");
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			isPlayerNearby = false;
			Debug.Log("Player left the portal area.");
		}
	}

}
