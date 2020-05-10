using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings
{
    private static Settings _instance = null;

    public static Settings Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new Settings();
            }

            return _instance;
        }
    }

    public  int                 currMaterial    = 19;
	public  float               currSize        = 1f;
	[HideInInspector]
	public	int					scaleLevel		= 9;

	public  Tool                currTool        = Tool.Attach;
	public  Brush               currBrush       = Brush.Voxel;
	public  Grid                currGrid        = Grid.Slim;

    public  Vector3Int          areaSize        = new Vector3Int(16, 16, 16);

	public	List<LevelData>     levels			= new List<LevelData>();
	public  int                 levelToLoad     = -1;
	public  string              lastName        = "";

	[HideInInspector]
	public	float timer = 0;

	public void ResetSettings()
	{
		currMaterial = 19;
		currSize        = 1f;
		scaleLevel      = 9;

		currTool        = Tool.Attach;
		currBrush       = Brush.Voxel;
		currGrid        = Grid.Slim;
	}

    public Sides String2Sides(string name)
    {
        switch(name)
		{
			case "Up":
            case "FloorUp":
			{
				return Sides.Up;
				break;
			}
			case "Down":
			{
                return Sides.Down;
				break;
			}
			case "Right":
			{
                return Sides.Right;
				break;
			}
			case "Left":
			{
                return Sides.Left;
				break;
			}
			case "Front":
			{
                return Sides.Front;
				break;
			}
			case "Back":
			{
                return Sides.Back;
				break;
			}
		}
        return Sides.Up;
    }

	public Vector3Int UseSidesOnCoordinate(Vector3Int position, Sides side)
	{
		Vector3Int pos = new Vector3Int(position.x, position.y, position.z);
		switch(side)
		{
			case Sides.Up:
			{
				pos.y += 1;
				break;
			}
			case Sides.Down:
			{
				pos.y -= 1;
				break;
			}
			case Sides.Front:
			{
				pos.z -= 1;
				break;
			}
			case Sides.Back:
			{
				pos.z += 1;
				break;
			}
			case Sides.Right:
			{
				pos.x += 1;
				break;
			}
			case Sides.Left:
			{
				pos.x -= 1;
				break;
			}
		}
		//Debug.Log("pos: " + position + " " + pos);
		return pos;
	}

}
