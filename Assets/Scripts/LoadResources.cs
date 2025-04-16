using TMPro;
using UnityEngine;

public class LoadResources : MonoBehaviour
{
    public TextMeshProUGUI coinIndi;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
		int currentCoins = SaveManager.LoadCoins();
		coinIndi.text = currentCoins.ToString();
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
