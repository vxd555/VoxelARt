using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Interface : MonoBehaviour
{
    [Header("Laps")]
    public GameObject           colorPalette        = null;
    public GameObject           toolObject          = null;
    public GameObject           brushObject         = null;
    public GameObject           gridObject          = null;
    public GameObject           saveObject          = null;
    public GameObject           saveAsObject        = null;
    public GameObject           quitObject          = null;

    [Header("Toolbar")]
    public Text                 tool                = null;
    public Text                 brush               = null;
    public Text                 grid                = null;
    public Image                color               = null;

    [Header("Connected")]
    public EditorManager        editorManager       = null;
	public Transform            rotationRoot        = null;

    public Image                trackInfoImg        = null;

    public Transform            handleScale         = null;

    void Start()
    {
        tool.text = Settings.Instance.currTool.ToString();
        brush.text = Settings.Instance.currBrush.ToString();
        grid.text = Settings.Instance.currGrid.ToString();
        color.color = editorManager.materials[Settings.Instance.currMaterial].color;
    }

    public void NextTool()
    {
        switch(Settings.Instance.currTool)
        {
            case Tool.Attach:
            {
                Settings.Instance.currTool = Tool.Erase;
                break;
            }
            case Tool.Erase:
            {
                Settings.Instance.currTool = Tool.Paint;
                break;
            }
            case Tool.Paint:
            {
                Settings.Instance.currTool = Tool.Attach;
                break;
            }
        }
        tool.text = Settings.Instance.currTool.ToString();
    }

    public void NextBrush()
    {
        switch(Settings.Instance.currBrush)
        {
            case Brush.Voxel:
                Settings.Instance.currBrush = Brush.Rect;
                break;
            case Brush.Rect:
                Settings.Instance.currBrush = Brush.Line;
                break;
            case Brush.Line:
                Settings.Instance.currBrush = Brush.Fill;
                break;
            case Brush.Fill:
                Settings.Instance.currBrush = Brush.Voxel;
                break;
        }
        brush.text = Settings.Instance.currBrush.ToString();
    }

    public void NextGrid()
    {
        switch(Settings.Instance.currGrid)
        {
            case Grid.None:
                Settings.Instance.currGrid = Grid.Slim;
                break;
            case Grid.Slim:
                Settings.Instance.currGrid = Grid.Fat;
                break;
            case Grid.Fat:
                Settings.Instance.currGrid = Grid.None;
                break;
        }
        grid.text = Settings.Instance.currGrid.ToString();
    }

    public void HideAll()
    {
        Settings.Instance.timer = 0f;
        toolObject.SetActive(false);
        brushObject.SetActive(false);
        gridObject.SetActive(false);
        colorPalette.SetActive(false);
        saveObject.SetActive(false);
        saveAsObject.SetActive(false);
        quitObject.SetActive(false);
    }

    public void ShowTool()
    {
        HideAll();
        toolObject.SetActive(true);
    }

    public void SetTool(int newTool)
    {
        Settings.Instance.timer = 0f;
        Settings.Instance.currTool = (Tool)newTool;
        tool.text = Settings.Instance.currTool.ToString();
        toolObject.SetActive(false);
    }

    public void ShowBrush()
    {
        HideAll();
        brushObject.SetActive(true);
    }

    public void SetBrush(int newBrush)
    {
        Settings.Instance.timer = 0f;
        Settings.Instance.currBrush = (Brush)newBrush;
        brush.text = Settings.Instance.currBrush.ToString();
        brushObject.SetActive(false);
    }

    public void ShowGrid()
    {
        HideAll();
        gridObject.SetActive(true);
    }

    public void SetGrid(int newGrid)
    {
        Settings.Instance.timer = 0f;
        Settings.Instance.currGrid = (Grid)newGrid;
        grid.text = Settings.Instance.currGrid.ToString();
        gridObject.SetActive(false);
        editorManager.UpdateGrid();
    }

    public void ShowColors()
    {
        HideAll();
        colorPalette.SetActive(true);
    }

    public void SetColor(int newColor)
    {
        Settings.Instance.timer = 0f;
        color.color = editorManager.materials[newColor].color;
        Settings.Instance.currMaterial = newColor;
        colorPalette.SetActive(false);
    }

    public void ShowSave()
    {
        HideAll();
        saveObject.SetActive(true);
    }

    public void SaveModel()
    {
        Settings.Instance.timer = 0f;
        //TU BĘDZIE ZAPISYWANIE
        saveObject.SetActive(false);
    }

    public void ShowSaveAs()
    {
        HideAll();
        saveAsObject.SetActive(true);
    }

    public void SaveAsModel()
    {
        Settings.Instance.timer = 0f;
        //TU BĘDZIE ZAPISYWANIE
        saveAsObject.SetActive(false);
    }

    public void ShowQuit()
    {
        HideAll();
        quitObject.SetActive(true);
    }

    public void QuitEditor()
    {
        Settings.Instance.timer = 0f;
		SceneManager.LoadScene("Menu");
    }

    public void SetTrackMarker(bool tack)
	{
		if(tack)
		{
			trackInfoImg.color = Color.green;
		}
		else
		{
			trackInfoImg.color = Color.red;
		}
	}

    public void Increase()
    {
        Settings.Instance.timer = 0f;
        if(Settings.Instance.scaleLevel < 20)
        {
            ++Settings.Instance.scaleLevel;
            Resize();
        }
    }

    public void Decrease()
    {
        Settings.Instance.timer = 0f;
        if(Settings.Instance.scaleLevel > 0)
        {
            --Settings.Instance.scaleLevel;
            Resize();
        }
    }

    public void Resize()
    {
        Settings.Instance.currSize = 0.1f + ((float)Settings.Instance.scaleLevel) * 0.1f;
        handleScale.localScale = new Vector3(Settings.Instance.currSize, Settings.Instance.currSize, Settings.Instance.currSize);
    }

	public void TurnLeft()
	{
		rotationRoot.localEulerAngles = new Vector3(rotationRoot.localEulerAngles.x, rotationRoot.localEulerAngles.y - 10, rotationRoot.localEulerAngles.z);
	}

	public void TurnRight()
	{
		rotationRoot.localEulerAngles = new Vector3(rotationRoot.localEulerAngles.x, rotationRoot.localEulerAngles.y + 10, rotationRoot.localEulerAngles.z);
	}
}
