using UnityEngine;
using UnityEngine.SceneManagement;

public class LimboManager : MonoBehaviour
{
    public void PlayAgain()
    {
		ClearDontDestroyObjects();
		SceneManager.LoadScene("The Invasion");
	}

	public void BackToHall()
	{
		ClearDontDestroyObjects();
		SceneManager.LoadScene("WaitingHall");
	}

	void ClearDontDestroyObjects()
	{
		GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
		foreach (GameObject obj in allObjects)
		{
			// Nếu object nằm trong scene "DontDestroyOnLoad", nó sẽ không có scene name
			if (obj.scene.name == null || obj.scene.name == "")
			{
				Destroy(obj);
			}
		}
	}

}
