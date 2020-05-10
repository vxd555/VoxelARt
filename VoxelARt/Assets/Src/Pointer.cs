using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pointer : MonoBehaviour
{
    public	Transform						pointerHandle		= null;

    public  GameObject[]                    sites               = new GameObject[6];

    public	GameObject                      pointerObj          = null;
    private Transform                       pointerTransform    = null;
    private Sides                           currSide            = Sides.Up;

    void Awake()
    {
        pointerTransform = pointerObj.transform;
    }

    public void SetPointer(RaycastHit hit)
	{
		pointerTransform.position = hit.collider.transform.position;
		sites[(int)currSide].SetActive(false);
        string name = hit.collider.gameObject.name;
        switch(name)
		{
			case "Up":
			{
				currSide = Sides.Up;
				sites[(int)currSide].SetActive(true);
				break;
			}
			case "Down":
			case "FloorUp":
			{
				currSide = Sides.Down;
				sites[(int)currSide].SetActive(true);
				break;
			}
			case "Right":
			{
				currSide = Sides.Right;
				sites[(int)currSide].SetActive(true);
				break;
			}
			case "Left":
			{
				currSide = Sides.Left;
				sites[(int)currSide].SetActive(true);
				break;
			}
			case "Front":
			{
				currSide = Sides.Front;
				sites[(int)currSide].SetActive(true);
				break;
			}
			case "Back":
			{
				currSide = Sides.Back;
				sites[(int)currSide].SetActive(true);
				break;
			}
		}
    }

	public void Hide()
	{
		sites[(int)currSide].SetActive(false);
	}
}
