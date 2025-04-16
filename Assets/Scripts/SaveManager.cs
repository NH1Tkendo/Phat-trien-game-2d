using System.IO;
using UnityEngine;

[System.Serializable]
public class SaveData
{
	public int coins;
}

public static class SaveManager
{
	private static string savePath = Application.persistentDataPath + "/save.json";

	public static int LoadDefaultCoins()
	{
		TextAsset defaultJson = Resources.Load<TextAsset>("default_save");
		if (defaultJson != null)
		{
			SaveData data = JsonUtility.FromJson<SaveData>(defaultJson.text);
			return data.coins;
		}
		return 0;
	}

	public static int LoadCoins()
	{
		if (File.Exists(savePath))
		{
			string json = File.ReadAllText(savePath);
			SaveData data = JsonUtility.FromJson<SaveData>(json);
			return data.coins;
		}
		else
		{
			int defaultCoins = LoadDefaultCoins();
			SaveCoins(defaultCoins);
			return defaultCoins;
		}
	}

	public static void SaveCoins(int coinAmount)
	{
		SaveData data;

		// Nếu đã có file lưu, đọc và cộng dồn
		if (File.Exists(savePath))
		{
			string json = File.ReadAllText(savePath);
			data = JsonUtility.FromJson<SaveData>(json);
			data.coins += coinAmount;
		}
		else
		{
			// Nếu chưa có file, khởi tạo mới với giá trị mặc định từ file resources
			data = new SaveData();
			data.coins = LoadDefaultCoins() + coinAmount;
		}

		// Ghi lại dữ liệu đã cập nhật
		string updatedJson = JsonUtility.ToJson(data);
		File.WriteAllText(savePath, updatedJson);
	}

}
