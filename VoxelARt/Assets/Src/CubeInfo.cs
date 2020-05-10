using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeInfo : MonoBehaviour
{
    public Vector3Int       position        = new Vector3Int(0, 0, 0);
    public int              material        = -1;
    public CubeInfo[]       neighbors       = new CubeInfo[6];
    public MeshRenderer     meshRenderer    = null;

    public int x
    {
        get
        {
            return position.x;
        }
        set
        {
            position.x = value;
        }
    }
    public int y
    {
        get
        {
            return position.y;
        }
        set
        {
            position.y = value;
        }
    }
    public int z
    {
        get
        {
            return position.z;
        }
        set
        {
            position.z = value;
        }
    }

    public CubeInfo(CubeInfo ci)
    {
        position = ci.position;
        material = ci.material;
        for(int i = 0; i < ci.neighbors.Length; ++i)
        {
            neighbors[i] = ci.neighbors[i];
        }
        meshRenderer = ci.meshRenderer;
    }
}
