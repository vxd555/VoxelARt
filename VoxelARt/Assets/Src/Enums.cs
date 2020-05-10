using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public enum Tool
{
	Attach		= 0,
	Erase		= 1,
	Paint 		= 2
}

[SerializeField]
public enum Brush
{
	Voxel		= 0,
	Rect		= 1,
	Line 		= 2,
	Fill		= 3
}

[SerializeField]
public enum Grid
{
	None		= 0,
	Slim		= 1,
	Fat		 	= 2
}

[SerializeField]
public enum Sides
{
	Up			= 0,
	Down		= 1,
    Front 		= 2,
    Back 		= 3,
	Right 		= 4,
	Left 		= 5
}