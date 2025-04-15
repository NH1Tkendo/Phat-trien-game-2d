using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MenuController : MonoBehaviour
{
	public GameObject menuPanel;
	private GameObject menuInstance;

	public GameObject aboutUsPanel;
	private GameObject aboutUsInstance;

	public Transform canvasTransform;
	private bool isMenuOpen = false;
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			GameObject menu = getInstance();
			GameObject panel = getUsInstance();
			ToggleMenu(menu);
		}
	}

	private GameObject getInstance()
	{
		if (menuInstance == null)  // Kiểm tra nếu chưa tạo menuInstance
		{
			menuInstance = Instantiate(menuPanel, canvasTransform);  // Instantiate prefab vào canvas

			Button playButton = menuInstance.transform.Find("Play").GetComponent<Button>();
			playButton.onClick.AddListener(PlayGame);
		}
		
		return menuInstance;
	}

	private GameObject getUsInstance()
	{
		if (aboutUsInstance == null)
		{
			aboutUsInstance = Instantiate(aboutUsPanel, canvasTransform);
			aboutUsInstance.SetActive(false);

			// Gán sự kiện cho nút "AboutUs" trong menuInstance
			Button usButton = menuInstance.transform.Find("AboutUs").GetComponent<Button>();
			usButton.onClick.AddListener(AboutUs);

			// Gán nút thoát "ExitAboutUs" trong AboutUs panel
			Button exitButton = aboutUsInstance.transform.Find("Button").GetComponent<Button>();
			exitButton.onClick.AddListener(exitAboutUs);
		}
		return aboutUsInstance;
	}
	public void ToggleMenu(GameObject menu)
	{
		isMenuOpen = !isMenuOpen;
		menu.SetActive(isMenuOpen);  // Đóng hoặc mở menu

		// Dừng hoặc tiếp tục game tùy vào trạng thái menu
		Time.timeScale = isMenuOpen ? 0f : 1f;

	}

	public void PlayGame()
	{
		menuInstance.SetActive(false);  // Ẩn menu
		Time.timeScale = 1f;  // Tiếp tục game
	}

	public void AboutUs()
	{
		aboutUsInstance.SetActive(true);  // Hiện panel đã được Instantiate
	}

	public void exitAboutUs()
	{
		aboutUsInstance.SetActive(false); // Ẩn panel đã được Instantiate
	}

	public void GoToMainMenu()
	{
		Time.timeScale = 1f;  
		SceneManager.LoadScene("MainMenu");
	}
}
