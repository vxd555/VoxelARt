using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fill
{
    public EditorManager editorManager = null;

    public void AttachFill(CubeInfo start, Sides currSide, CubeInfo[,,] cubeMap, bool paint = false)
	{
        if(currSide == Sides.Up)
        {
            int mat = -1;
            if(start.y > 0) mat = cubeMap[start.x, start.y - 1, start.z].material;
            floodfillUp(start.x, start.y, start.z, start.material, Settings.Instance.currMaterial, mat, cubeMap, paint);
        }
        else if(currSide == Sides.Down)
        {
            int mat = -1;
            if(start.y < cubeMap.GetLength(1)) mat = cubeMap[start.x, start.y + 1, start.z].material;
            floodfillDown(start.x, start.y, start.z, start.material, Settings.Instance.currMaterial, mat, cubeMap, paint);
        }
        else if(currSide == Sides.Front)
        {
            int mat = -1;
            if(start.z < cubeMap.GetLength(2)) mat = cubeMap[start.x, start.y, start.z + 1].material;
            floodfillFront(start.x, start.y, start.z, start.material, Settings.Instance.currMaterial, mat, cubeMap, paint);
        }
        else if(currSide == Sides.Back)
        {
            int mat = -1;
            if(start.z > 0) mat = cubeMap[start.x, start.y, start.z - 1].material;
            floodfillBack(start.x, start.y, start.z, start.material, Settings.Instance.currMaterial, mat, cubeMap, paint);
        }
        if(currSide == Sides.Right)
        {
            int mat = -1;
            if(start.x > 0) mat = cubeMap[start.x - 1, start.y, start.z].material;
            floodfillRight(start.x, start.y, start.z, start.material, Settings.Instance.currMaterial, mat, cubeMap, paint);
        }
        else if(currSide == Sides.Left)
        {
            int mat = -1;
            if(start.x < cubeMap.GetLength(0)) mat = cubeMap[start.x + 1, start.y, start.z].material;
            floodfillLeft(start.x, start.y, start.z, start.material, Settings.Instance.currMaterial, mat, cubeMap, paint);
        }
    }

    public void PaintFill(CubeInfo start, Sides currSide, CubeInfo[,,] cubeMap)
    {
        if(start.y == -1) return;
        AttachFill(start, currSide, cubeMap, true);
    }

    public void EraseFill(CubeInfo start, Sides currSide, CubeInfo[,,] cubeMap)
	{
        if(currSide == Sides.Up)
        {
            int mat = -1;
            if(start.y > 0) mat = cubeMap[start.x, start.y - 1, start.z].material;
            floodfillUp(start.x, start.y, start.z, start.material, -1, mat, cubeMap);
        }
        else if(currSide == Sides.Down)
        {
            int mat = -1;
            if(start.y < cubeMap.GetLength(1)) mat = cubeMap[start.x, start.y + 1, start.z].material;
            floodfillDown(start.x, start.y, start.z, start.material, -1, mat, cubeMap);
        }
        else if(currSide == Sides.Front)
        {
            int mat = -1;
            if(start.z < cubeMap.GetLength(2)) mat = cubeMap[start.x, start.y, start.z + 1].material;
            floodfillFront(start.x, start.y, start.z, start.material, -1, mat, cubeMap);
        }
        else if(currSide == Sides.Back)
        {
            int mat = -1;
            if(start.z > 0) mat = cubeMap[start.x, start.y, start.z - 1].material;
            floodfillBack(start.x, start.y, start.z, start.material, -1, mat, cubeMap);
        }
        if(currSide == Sides.Right)
        {
            int mat = -1;
            if(start.x > 0) mat = cubeMap[start.x - 1, start.y, start.z].material;
            floodfillRight(start.x, start.y, start.z, start.material, -1, mat, cubeMap);
        }
        else if(currSide == Sides.Left)
        {
            int mat = -1;
            if(start.x < cubeMap.GetLength(0)) mat = cubeMap[start.x + 1, start.y, start.z].material;
            floodfillLeft(start.x, start.y, start.z, start.material, -1, mat, cubeMap);
        }
    }

    public void SetMaterial(int x, int y, int z, int newColor, CubeInfo[,,] cubeMap, bool paint = false)
    {
        cubeMap[x, y, z].material = newColor;
        if(newColor == -1)
        {
            cubeMap[x, y, z].gameObject.SetActive(false);
		}
        else if(paint)
        {
            cubeMap[x, y, z].meshRenderer.material = editorManager.materials[newColor];
        }
        else
        {
            cubeMap[x, y, z].meshRenderer.material = editorManager.materials[newColor];
            cubeMap[x, y, z].gameObject.SetActive(true);
        }
    }

    //up
    private void floodfillUp(int x, int y, int z, int oldColor, int newColor, int otherColor, CubeInfo[,,] cubeMap, bool paint = false)
    {
        if(cubeMap[x, y, z].material != oldColor) return;
        if(otherColor != -1 && cubeMap[x, y - 1, z].material != otherColor) return;
        if(otherColor == -1 && cubeMap[x, y + 1, z].material != otherColor) return;

        SetMaterial(x, y, z, newColor, cubeMap, paint);

        if(x + 1 < cubeMap.GetLength(0)) floodfillUp(x + 1, y, z, oldColor, newColor, otherColor, cubeMap); // right
        if(x > 0) floodfillUp(x - 1, y, z, oldColor, newColor, otherColor, cubeMap); // left
        if(z + 1 < cubeMap.GetLength(2)) floodfillUp(x, y, z + 1, oldColor, newColor, otherColor, cubeMap); // back
        if(z > 0) floodfillUp(x, y, z - 1, oldColor, newColor, otherColor, cubeMap); // front
    }

    //down
    private void floodfillDown(int x, int y, int z, int oldColor, int newColor, int otherColor, CubeInfo[,,] cubeMap, bool paint = false)
    {
        if(cubeMap[x, y, z].material != oldColor) return;
        if(otherColor != -1 && cubeMap[x, y + 1, z].material != otherColor) return;
        if(otherColor == -1 && cubeMap[x, y - 1, z].material != otherColor) return;

        SetMaterial(x, y, z, newColor, cubeMap, paint);

        if(x + 1 < cubeMap.GetLength(0)) floodfillDown(x + 1, y, z, oldColor, newColor, otherColor, cubeMap); // right
        if(x > 0) floodfillDown(x - 1, y, z, oldColor, newColor, otherColor, cubeMap); // left
        if(z + 1 < cubeMap.GetLength(2)) floodfillDown(x, y, z + 1, oldColor, newColor, otherColor, cubeMap); // back
        if(z > 0) floodfillDown(x, y, z - 1, oldColor, newColor, otherColor, cubeMap); // front
    }

    //front
    private void floodfillFront(int x, int y, int z, int oldColor, int newColor, int otherColor, CubeInfo[,,] cubeMap, bool paint = false)
    {
        if(cubeMap[x, y, z].material != oldColor) return;
        if(otherColor != -1 && cubeMap[x, y, z + 1].material != otherColor) return;
        if(otherColor == -1 && cubeMap[x, y, z - 1].material != otherColor) return;

        SetMaterial(x, y, z, newColor, cubeMap, paint);

        if(x + 1 < cubeMap.GetLength(0)) floodfillFront(x + 1, y, z, oldColor, newColor, otherColor, cubeMap); // right
        if(x > 0) floodfillFront(x - 1, y, z, oldColor, newColor, otherColor, cubeMap); // left
        if(y + 1 < cubeMap.GetLength(1)) floodfillFront(x, y + 1, z, oldColor, newColor, otherColor, cubeMap); // up
        if(y > 0) floodfillFront(x, y - 1, z, oldColor, newColor, otherColor, cubeMap); // down
    }

    //back
    private void floodfillBack(int x, int y, int z, int oldColor, int newColor, int otherColor, CubeInfo[,,] cubeMap, bool paint = false)
    {
        if(cubeMap[x, y, z].material != oldColor) return;
        if(otherColor != -1 && cubeMap[x, y, z - 1].material != otherColor) return;
        if(otherColor == -1 && cubeMap[x, y, z + 1].material != otherColor) return;

        SetMaterial(x, y, z, newColor, cubeMap, paint);

        if(x + 1 < cubeMap.GetLength(0)) floodfillBack(x + 1, y, z, oldColor, newColor, otherColor, cubeMap); // right
        if(x > 0) floodfillBack(x - 1, y, z, oldColor, newColor, otherColor, cubeMap); // left
        if(y + 1 < cubeMap.GetLength(1)) floodfillBack(x, y + 1, z, oldColor, newColor, otherColor, cubeMap); // up
        if(y > 0) floodfillBack(x, y - 1, z, oldColor, newColor, otherColor, cubeMap); // down
    }

    //right
    private void floodfillRight(int x, int y, int z, int oldColor, int newColor, int otherColor, CubeInfo[,,] cubeMap, bool paint = false)
    {
        if(cubeMap[x, y, z].material != oldColor) return;
        if(otherColor != -1 && cubeMap[x - 1, y, z].material != otherColor) return;
        if(otherColor == -1 && cubeMap[x + 1, y, z].material != otherColor) return;

        SetMaterial(x, y, z, newColor, cubeMap, paint);

        if(y + 1 < cubeMap.GetLength(1)) floodfillRight(x, y + 1, z, oldColor, newColor, otherColor, cubeMap); // up
        if(y > 0) floodfillRight(x, y - 1, z, oldColor, newColor, otherColor, cubeMap); // down
        if(z + 1 < cubeMap.GetLength(2)) floodfillRight(x, y, z + 1, oldColor, newColor, otherColor, cubeMap); // back
        if(z > 0) floodfillRight(x, y, z - 1, oldColor, newColor, otherColor, cubeMap); // front
    }

    //left
    private void floodfillLeft(int x, int y, int z, int oldColor, int newColor, int otherColor, CubeInfo[,,] cubeMap, bool paint = false)
    {
        if(cubeMap[x, y, z].material != oldColor) return;
        if(otherColor != -1 && cubeMap[x + 1, y, z].material != otherColor) return;
        if(otherColor == -1 && cubeMap[x - 1, y, z].material != otherColor) return;

        SetMaterial(x, y, z, newColor, cubeMap, paint);

        if(y + 1 < cubeMap.GetLength(1)) floodfillLeft(x, y + 1, z, oldColor, newColor, otherColor, cubeMap); // up
        if(y > 0) floodfillLeft(x, y - 1, z, oldColor, newColor, otherColor, cubeMap); // down
        if(z + 1 < cubeMap.GetLength(2)) floodfillLeft(x, y, z + 1, oldColor, newColor, otherColor, cubeMap); // back
        if(z > 0) floodfillLeft(x, y, z - 1, oldColor, newColor, otherColor, cubeMap); // front
    }
}
