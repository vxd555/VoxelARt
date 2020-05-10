using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EditorManager : MonoBehaviour
{
    [Header("Prefabs")]
	public	GameObject						cubePrefab		= null;
	public	GameObject						floorPrefab		= null;
	public	List<Material>					materials		= new List<Material>();

	[Header("Handles")]
	public	Transform						cubeHandle		= null;
	public	Transform						floorHandle		= null;

    [Header("Components")]
    public  Pointer                         pointer         = new Pointer();
    public  Sprite[]                        grids           = new Sprite[3];

    [HideInInspector]
    public  CubeInfo[,,]                    cubeMap;
    [HideInInspector]
    public  CubeInfo[,]                     floorMap;
    [HideInInspector]
    public  SpriteRenderer[,]               floorView;

    private bool                            click           = false;
    private CubeInfo                        beginCube       = null;
    private CubeInfo                        lastCube        = null;
    private Sides                           beginSide       = Sides.Up;
    private Line                            line            = new Line();
    private Fill                            fill            = new Fill();

    void Awake()
    {
		ClearMap();

		PreLoad();

		SpawnFloor();
        SpawnCubes();
        SaveCubeInfos();
        fill.editorManager = this;

		PostLoad();
    }

	private void ClearMap()
	{
		foreach(Transform child in cubeHandle.transform)
		{
			Destroy(child.gameObject);
		}
		foreach(Transform child in floorHandle.transform)
		{
			Destroy(child.gameObject);
		}
	}

	private void SpawnFloor()
    {
        floorMap = new CubeInfo[Settings.Instance.areaSize.x, Settings.Instance.areaSize.z];
        floorView = new SpriteRenderer[Settings.Instance.areaSize.x, Settings.Instance.areaSize.z];
        Vector3 floorPos;
        GameObject obj;
        for(int x = 0; x < Settings.Instance.areaSize.x; ++x)
        {
            for(int z = 0; z < Settings.Instance.areaSize.z; ++z)
            {
                floorPos = new Vector3((x - (Settings.Instance.areaSize.x / 2)) * (Settings.Instance.currSize),
										0f,
										(z - (Settings.Instance.areaSize.z / 2)) * (Settings.Instance.currSize));
                obj = Instantiate(floorPrefab, floorPos, Quaternion.identity, floorHandle) as GameObject;
                floorMap[x, z] = obj.GetComponent<CubeInfo>();
                floorMap[x, z].position = new Vector3Int(x, -1, z);
                floorView[x, z] = obj.transform.Find("frame").GetComponent<SpriteRenderer>();
            }
        }
    }

    private void SpawnCubes()
    {
        cubeMap = new CubeInfo[Settings.Instance.areaSize.x, Settings.Instance.areaSize.y, Settings.Instance.areaSize.z];
        Vector3 cubePos;
        GameObject obj;
        for(int y = 0; y < Settings.Instance.areaSize.y; ++y)
        {
            for(int x = 0; x < Settings.Instance.areaSize.x; ++x)
            {
                for(int z = 0; z < Settings.Instance.areaSize.z; ++z)
                {
                    cubePos = new Vector3((x - (Settings.Instance.areaSize.x / 2)) * (Settings.Instance.currSize),
                                           y,
                                           (z - (Settings.Instance.areaSize.z / 2)) * (Settings.Instance.currSize));
                    obj = Instantiate(cubePrefab, cubePos, Quaternion.identity, cubeHandle) as GameObject;
                    cubeMap[x, y, z] = obj.GetComponent<CubeInfo>();
                    cubeMap[x, y, z].position = new Vector3Int(x, y, z);
                    obj.SetActive(false);
                }
            }
        }
    }

    private void SaveCubeInfos()
    {
        for(int y = 0; y < Settings.Instance.areaSize.y; ++y)
        {
            for(int x = 0; x < Settings.Instance.areaSize.x; ++x)
            {
                for(int z = 0; z < Settings.Instance.areaSize.z; ++z)
                {
                    if(y + 1 < Settings.Instance.areaSize.y)
                        cubeMap[x, y, z].neighbors[(int)Sides.Up] = cubeMap[x, y + 1, z];
                    if(y - 1 > 0)
                        cubeMap[x, y, z].neighbors[(int)Sides.Down] = cubeMap[x, y - 1, z];
                    if(z - 1 > 0)
                        cubeMap[x, y, z].neighbors[(int)Sides.Front] = cubeMap[x, y, z - 1];
                    if(z + 1 < Settings.Instance.areaSize.z)
                        cubeMap[x, y, z].neighbors[(int)Sides.Back] = cubeMap[x, y, z + 1];
                    if(x + 1 < Settings.Instance.areaSize.x)
                        cubeMap[x, y, z].neighbors[(int)Sides.Right] = cubeMap[x + 1, y, z];
                    if(x - 1 > 0)
                        cubeMap[x, y, z].neighbors[(int)Sides.Left] = cubeMap[x - 1, y, z];
                }
            }
        }

        for(int x = 0; x < Settings.Instance.areaSize.x; ++x)
        {
            for(int z = 0; z < Settings.Instance.areaSize.z; ++z)
            {
                floorMap[x, z].neighbors[(int)Sides.Up] = cubeMap[x, 0, z];
            }
        }
    }

	void PreLoad()
	{
		if(Settings.Instance.levelToLoad == -1) return;
		Settings.Instance.areaSize.x = Settings.Instance.levels[Settings.Instance.levelToLoad].x;
		Settings.Instance.areaSize.y = Settings.Instance.levels[Settings.Instance.levelToLoad].y;
		Settings.Instance.areaSize.z = Settings.Instance.levels[Settings.Instance.levelToLoad].z;

		SaveManager.Instance.lastName = Settings.Instance.lastName;
	}

	void PostLoad()
	{
		if(Settings.Instance.levelToLoad == -1) return;

		for(int y = 0; y < Settings.Instance.areaSize.y; ++y)
		{
			for(int x = 0; x < Settings.Instance.areaSize.x; ++x)
			{
				for(int z = 0; z < Settings.Instance.areaSize.z; ++z)
				{
					if(Settings.Instance.levels[Settings.Instance.levelToLoad].data[x, y, z] >= 0)
					{
						cubeMap[x, y, z].material = Settings.Instance.levels[Settings.Instance.levelToLoad].data[x, y, z];
						cubeMap[x, y, z].meshRenderer.material = materials[cubeMap[x, y, z].material];
						cubeMap[x, y, z].gameObject.SetActive(true);
					}
				}
			}
		}
	}

	private int frames = 0;
	void Update()
	{
        //zabezpieczenie żeby po kliknięciu w UI nie stawiałt się klocki
		if(EventSystem.current.IsPointerOverGameObject()) return;
        if(Settings.Instance.timer <= 0.5f)
        {
            Settings.Instance.timer += Time.deltaTime;
            pointer.Hide();
            return;
        }
        ++frames;

        if(Input.GetMouseButtonDown(0) && click == false) //rozpoczącie rysowania
		{
            BeginDraw();
        }
        if(click) //kontynuowanie rysowania
		{
            if(frames % 4 == 0) ContinueDraw();
		}
        if(Input.GetMouseButtonUp(0) && click) //zakończenie rysowania
		{
			EndDraw();
        }
    }

    public void AttachCube(CubeInfo info, Sides side, int material)
	{
        if(info.neighbors[(int)side] != null)
        {
            info.neighbors[(int)side].material = material;
            info.neighbors[(int)side].meshRenderer.material = materials[material];
            info.neighbors[(int)side].gameObject.SetActive(true);
        }
    }

    public void EraseCube(CubeInfo info)
	{
        info.gameObject.SetActive(false);
		info.material = -1;
    }

    public void PaintCube(CubeInfo info, int material)
	{
        if(info.material != -1)
        {
            info.material = material;
            info.meshRenderer.material = materials[material];
        }
    }

    public void SetCube(CubeInfo info, int material)
	{
        info.material = material;
        info.meshRenderer.material = materials[material];
        info.gameObject.SetActive(true);
    }

    public void BeginDraw()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null)
            {
                switch(Settings.Instance.currBrush)
                {
                    case Brush.Voxel:
                    {
                        click = true;
                        break;
                    }
                    case Brush.Rect:
                    case Brush.Line:
                    case Brush.Fill:
                    {
                        beginCube = new CubeInfo(hit.collider.transform.parent.GetComponent<CubeInfo>());
                        beginSide = Settings.Instance.String2Sides(hit.collider.name);
                        click = true;
                        break;
                    }
                }
            }
        }
    }

    public void ContinueDraw()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null) pointer.SetPointer(hit); //ustawienie wskaźnika gdzie się rysuje
        }
    }

    public void EndDraw()
    {
        click = false;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        pointer.Hide();

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null)
            {
                switch(Settings.Instance.currBrush)
                {
                    case Brush.Voxel:
                    {
                        if(Settings.Instance.currTool == Tool.Attach)
                        {
                            AttachCube(hit.collider.transform.parent.GetComponent<CubeInfo>(), 
                                        Settings.Instance.String2Sides(hit.collider.name), 
                                        Settings.Instance.currMaterial);
                        }
                        else if(Settings.Instance.currTool == Tool.Erase)
                        {
                            lastCube = hit.collider.transform.parent.GetComponent<CubeInfo>();
                            if(lastCube.y > -1) EraseCube(lastCube);
                        }
                        else if(Settings.Instance.currTool == Tool.Paint)
                        {
                            PaintCube(hit.collider.transform.parent.GetComponent<CubeInfo>(), 
                                        Settings.Instance.currMaterial);
                        }
                        break;
                    }
                    case Brush.Rect:
                    {
                        if(Settings.Instance.currTool == Tool.Attach)
                        {
                            Vector3Int pos = Settings.Instance.UseSidesOnCoordinate(beginCube.position, beginSide);
                            beginCube = new CubeInfo(cubeMap[pos.x, pos.y, pos.z]);
                        }
                        lastCube = hit.collider.transform.parent.GetComponent<CubeInfo>();
                        if(Settings.Instance.currTool == Tool.Attach)
                        {
                            Vector3Int pos = Settings.Instance.UseSidesOnCoordinate(lastCube.position, Settings.Instance.String2Sides(hit.collider.name));
                            lastCube = new CubeInfo(cubeMap[pos.x, pos.y, pos.z]);
                        }
                        int minX = lastCube.x < beginCube.x ? lastCube.x : beginCube.x;
                        int maxX = lastCube.x > beginCube.x ? lastCube.x : beginCube.x;
                        int minY = lastCube.y < beginCube.y ? lastCube.y : beginCube.y;
                        int maxY = lastCube.y > beginCube.y ? lastCube.y : beginCube.y;
                        int minZ = lastCube.z < beginCube.z ? lastCube.z : beginCube.z;
                        int maxZ = lastCube.z > beginCube.z ? lastCube.z : beginCube.z;
                        if(minY == -1) minY = 0;

                        int mat = Settings.Instance.currMaterial;

                        if(Settings.Instance.currTool == Tool.Attach)
                        {
                            for(int x = minX; x <= maxX; ++x)
                            {
                                for(int y = minY; y <= maxY; ++y)
                                {
                                    for(int z = minZ; z <= maxZ; ++z)
                                    {
                                        SetCube(cubeMap[x, y, z], mat);
                                    }
                                }
                            }
                        }
                        else if(Settings.Instance.currTool == Tool.Erase)
                        {
                            for(int x = minX; x <= maxX; ++x)
                            {
                                for(int y = minY; y <= maxY; ++y)
                                {
                                    for(int z = minZ; z <= maxZ; ++z)
                                    {
                                        EraseCube(cubeMap[x, y, z]);
                                    }
                                }
                            }
                        }
                        else if(Settings.Instance.currTool == Tool.Paint)
                        {
                            for(int x = minX; x <= maxX; ++x)
                            {
                                for(int y = minY; y <= maxY; ++y)
                                {
                                    for(int z = minZ; z <= maxZ; ++z)
                                    {
                                        PaintCube(cubeMap[x, y, z], mat);
                                    }
                                }
                            }
                        }
                        break;
                    }
                    case Brush.Line:
                    {
                        //if(beginCube.y < 0) beginCube = cubeMap[beginCube.x, 0, beginCube.z];
                        if(Settings.Instance.currTool == Tool.Attach)
                        {
                            Vector3Int pos = Settings.Instance.UseSidesOnCoordinate(beginCube.position, beginSide);
                            beginCube = new CubeInfo(cubeMap[pos.x, pos.y, pos.z]);
                        }
                        lastCube = hit.collider.transform.parent.GetComponent<CubeInfo>();
                        if(Settings.Instance.currTool == Tool.Attach)
                        {
                            Vector3Int pos = Settings.Instance.UseSidesOnCoordinate(lastCube.position, Settings.Instance.String2Sides(hit.collider.name));
                            lastCube = new CubeInfo(cubeMap[pos.x, pos.y, pos.z]);
                        }
                        Debug.Log(beginCube.position + " " + lastCube.position);
                        List<Vector3Int> newLine = line.Draw3DLine(beginCube, lastCube, 
                                                            Settings.Instance.String2Sides(hit.collider.name));
                        
                        int mat = Settings.Instance.currMaterial;
                        foreach(var l in newLine)
                        {
                            if(Settings.Instance.currTool == Tool.Attach)
                            {
                                SetCube(cubeMap[l.x, l.y, l.z], mat);
                            }
                            else if(Settings.Instance.currTool == Tool.Erase)
                            {
                                if(l.y > -1) EraseCube(cubeMap[l.x, l.y, l.z]);
                            }
                            else if(Settings.Instance.currTool == Tool.Paint)
                            {
                                PaintCube(cubeMap[l.x, l.y, l.z], mat);
                            }
                        }
                        break;
                    }
                    case Brush.Fill:
                    {
                        lastCube = hit.collider.transform.parent.GetComponent<CubeInfo>();
                        beginSide = Settings.Instance.String2Sides(hit.collider.name);
                        if(Settings.Instance.currTool == Tool.Attach)
                        {
                            Vector3Int pos = Settings.Instance.UseSidesOnCoordinate(lastCube.position, beginSide);
                            lastCube = cubeMap[pos.x, pos.y, pos.z];
                            fill.AttachFill(lastCube, beginSide, cubeMap);
                        }
                        else if(Settings.Instance.currTool == Tool.Erase)
                        {
                            fill.EraseFill(lastCube, beginSide, cubeMap);
                        }
                        else if(Settings.Instance.currTool == Tool.Paint)
                        {
                            fill.PaintFill(lastCube, beginSide, cubeMap);
                        }

                        
                        break;
                    }
                }
            }
        }
    }

    public void UpdateGrid()
    {
        int currGrid = (int)Settings.Instance.currGrid;

        for(int x = 0; x < Settings.Instance.areaSize.x; ++x)
        {
            for(int z = 0; z < Settings.Instance.areaSize.z; ++z)
            {
                floorView[x, z].sprite = grids[currGrid];
            }
        }
    }
}
