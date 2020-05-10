using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

	[Header("Buttons")]
    public  GameObject  editor;
    public  GameObject  credits;
    public  GameObject  quit;
    public  GameObject  loading;

	[Header("Inputs")]
	public  InputField  inputX = null;
	public  InputField  inputY = null;
	public  InputField  inputZ = null;

	void Start()
	{
		inputX.text = "20";
		inputY.text = "20";
		inputZ.text = "20";

		Settings.Instance.levelToLoad = -1;
	}

	public void Credits()
    {
        editor.SetActive(false);
        credits.SetActive(false);
        quit.SetActive(false);
        loading.SetActive(true);
        SceneManager.LoadScene("Credits");
    }

    public void StartEditor()
    {
		int x = int.Parse(inputX.text);
		x = Mathf.Clamp(x, 1, 64);
		int y = int.Parse(inputY.text);
		y = Mathf.Clamp(x, 1, 64);
		int z = int.Parse(inputZ.text);
		z = Mathf.Clamp(x, 1, 64);

		Settings.Instance.areaSize = new Vector3Int(x, y, z);
		Settings.Instance.ResetSettings();

		editor.SetActive(false);
        credits.SetActive(false);
        quit.SetActive(false);
        loading.SetActive(true);
        SceneManager.LoadScene("Editor");
    }

	public void LoadLevel(int lvl, string name)
	{
		Settings.Instance.levelToLoad = lvl;
		Settings.Instance.lastName = name; //Settings.Instance.levels[lvl].
		StartEditor();
	}

	public void Quit()
    {
        Application.Quit();
    }
}