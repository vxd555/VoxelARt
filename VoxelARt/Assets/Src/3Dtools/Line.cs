using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line
{
    public List<Vector3Int> Draw3DLine(CubeInfo newStart, CubeInfo newEnd, Sides currSide)
	{
		List<Vector3Int> listOfPoints = new List<Vector3Int>();
		CubeInfo start = new CubeInfo(newStart);
		CubeInfo end = new CubeInfo(newEnd);

        if(start.x == end.x && start.y == end.y && start.z == end.z)
        {
            listOfPoints.Add(start.position);
            return listOfPoints;
        }

		if(start.y == -1) start.y = 0;

		int dx = (int)Mathf.Abs(start.x - end.x);
		int dy = (int)Mathf.Abs(start.y - end.y);
		int dz = (int)Mathf.Abs(start.z - end.z);

		int xs = 0;
		int ys = 0;
		int zs = 0;
		if (end.x > start.x) xs = 1;
		else xs = -1;
		if (end.y > start.y) ys = 1;
		else ys = -1;
		if (end.z > start.z) zs = 1;
		else zs = -1;

		listOfPoints.Add(new Vector3Int(start.x, start.y, start.z));

		if (dx >= dy && dx >= dz) //X
		{
			int p1 = 2 * dy - dx;
			int p2 = 2 * dz - dx;
			while (start.x != end.x)
			{
				start.x += xs;
				if (p1 >= 0)
				{
					start.y += ys;
					p1 -= 2 * dx;
				}
					
				if (p2 >= 0)
				{
					start.z += zs;
					p2 -= 2 * dx;
				}
				p1 += 2 * dy;
				p2 += 2 * dz;
				listOfPoints.Add(new Vector3Int(start.x, start.y, start.z));
			}
			
		}
		else if (dy >= dx && dy >= dz) //Y
		{
			int p1 = 2 * dx - dy;
			int p2 = 2 * dz - dy;
			while (start.y != end.y)
			{
				start.y += ys;
				if (p1 >= 0)
				{
					start.x += xs;
					p1 -= 2 * dy; 
				}
				if (p2 >= 0)
				{
					start.z += zs;
					p2 -= 2 * dy;
				}
				p1 += 2 * dx;
				p2 += 2 * dz;
				listOfPoints.Add(new Vector3Int(start.x, start.y, start.z));
			}
		}      
		else //Z
		{
			int p1 = 2 * dy - dz;
			int p2 = 2 * dx - dz;
			while (start.z != end.z)
			{
				start.z += zs;
				if (p1 >= 0)
				{
					start.y += ys;
					p1 -= 2 * dz;
				}
				if (p2 >= 0)
				{
					start.x += xs;
					p2 -= 2 * dz;
				}
				p1 += 2 * dy;
				p2 += 2 * dx;
				listOfPoints.Add(new Vector3Int(start.x, start.y, start.z));
			}
		}
			
		return listOfPoints;
	}
}
