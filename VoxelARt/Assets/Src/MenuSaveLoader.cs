using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSaveLoader : MonoBehaviour
{
	public GameObject       loadButton		= null;
	public Transform        scrollContent   = null;
	public Menu             menu            = null;

	private void Start()
	{
		LoadLevels();
	}

	public void LoadLevels()
	{
		UserData data = SaveSystem.LoadGame();
		UserData.ApplyUserData(data);
		Settings.Instance.levels = data.levels;

		List<string> names = data.GetSaveNames();

		for(int n = 0; n < names.Count; ++n)
		{
			int _n = n;
			string _name = names[n];
			GameObject lb = Instantiate(loadButton, scrollContent) as GameObject;
			lb.transform.Find("Text").GetComponent<Text>().text = names[n];
			lb.GetComponent<Button>().onClick.AddListener(() => menu.LoadLevel(_n, _name));
		}
	}

	
}
