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
}
