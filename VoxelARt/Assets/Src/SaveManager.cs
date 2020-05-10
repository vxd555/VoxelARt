using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveManager: MonoBehaviour
{
	private static SaveManager _instance = null;

	public static SaveManager Instance
	{
		get
		{
			if(_instance == null)
			{
				_instance = new SaveManager();
			}

			return _instance;
		}
	}

	public bool				resetGame		= false;

	public InputField		saveName		= null;
		
	public string			lastName		= "";

	public EditorManager    editorManager   = null;

	private void Start()
	{
		LoadLevels();
	}

	public void FastSave()
	{
		if(lastName == "") lastName = "temp";
		StartCoroutine(SaveCoroutine());
	}

	public void SaveAs()
	{
		lastName = saveName.text;
		StartCoroutine(SaveAsCoroutine());
	}

	private IEnumerator SaveCoroutine()
	{
		SaveGame(lastName);
		yield return null;
	}

	private IEnumerator SaveAsCoroutine()
	{
		SaveGame(saveName.text);
		yield return null;
	}

	public void SaveGame()
	{
		SaveSystem.SaveGame();
	}

	public void SaveGame(string name)
	{
		SaveSystem.SaveGame(name, editorManager);
	}

	public void LoadLevels()
	{
		UserData data = SaveSystem.LoadGame();
		if(resetGame) data.Reset();
		UserData.ApplyUserData(data);
	}
}
