using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
	private static string path = Application.persistentDataPath + "/sv.txt"; // tak dla zmyły hehe

	public static void SaveGame()
	{
		BinaryFormatter formatter = new BinaryFormatter();
		FileStream stream = new FileStream(path, FileMode.Create);
		UserData data = new UserData();
		formatter.Serialize(stream, data);
		stream.Close();
	}

	public static void SaveGame(string name, EditorManager editorManager)
	{
		BinaryFormatter formatter = new BinaryFormatter();
		FileStream stream = new FileStream(path, FileMode.Create);
		UserData data = new UserData();
		data.PrepareAndSave(name, editorManager.cubeMap);
		formatter.Serialize(stream, data);
		stream.Close();
	}

	public static UserData LoadGame()
	{
		if(File.Exists(path))
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(path, FileMode.Open);
			UserData data = formatter.Deserialize(stream) as UserData;
			stream.Close();
			return data;
		}
		else
		{
			Debug.LogWarning("FILE NOT FOUND");
			return null;
		}
	}
}
