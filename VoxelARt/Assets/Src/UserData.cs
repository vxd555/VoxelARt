using System.Collections.Generic;

[System.Serializable]
public class UserData
{
	public List<LevelData>      levels;

	public UserData()
	{
		levels = Settings.Instance.levels;
	}

	public void Reset()
	{
		levels = new List<LevelData>();
	}

	public void PrepareAndSave(string name, CubeInfo[,,] cubeMap)
	{
		LevelData level = new LevelData();
		level.name = name;
		level.x = Settings.Instance.areaSize.x;
		level.y = Settings.Instance.areaSize.y;
		level.z = Settings.Instance.areaSize.z;

		level.data = new int[level.x, level.y, level.z];

		for(int y = 0; y < Settings.Instance.areaSize.y; ++y)
		{
			for(int x = 0; x < Settings.Instance.areaSize.x; ++x)
			{
				for(int z = 0; z < Settings.Instance.areaSize.z; ++z)
				{
					level.data[x, y, z] = cubeMap[x, y, z].material;
				}
			}
		}

		AddSave(level);
	}

	public void AddSave(LevelData level)
	{
		//nadpisywanie modelu o tej samej nazwie
		int lvl = -1;
		for(int i = 0; i < levels.Count; ++i)
		{
			if(levels[i].name == level.name) lvl = i;
		}
		if(lvl != -1) levels.RemoveAt(lvl);

		//dodwanie nowego modelu
		levels.Add(level);
	}

	public static void ApplyUserData(UserData data)
	{
		if(data != null)
		{
			Settings.Instance.levels = data.levels;
		}
	}

	public List<string> GetSaveNames()
	{
		List<string> names = new List<string>();
		foreach(var n in levels)
		{
			names.Add(n.name);
		}
		return names;
	}
}

[System.Serializable]
public class LevelData
{
	public string name;
	public int x;
	public int y;
	public int z;
	public int[,,] data;
}
