using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleMarker : MonoBehaviour
{
    public MeshRenderer[]   isTrack			    = new MeshRenderer[5];
	public Transform[]		markers			    = new Transform[5];
	public Transform        handle			    = null;

	Vector3[]               trackedPosition     = new Vector3[5];
	Quaternion[]            trackedRotation     = new Quaternion[5];
	bool                    currTrackPoint      = false;

	public Interface        ui                  = null;

    void Update()
	{
        for(int i = 0; i < isTrack.Length; ++i)
		{
			if(isTrack[i].isVisible)
			{
				trackedPosition[i] = markers[i].position;
				trackedRotation[i] = markers[i].rotation;

				if(!currTrackPoint)
				{
					handle.position = trackedPosition[i] - (trackedPosition[i] - trackedPosition[0]);
					handle.rotation = trackedRotation[i];
					currTrackPoint = true;
				}
			}
		}
		ui.SetTrackMarker(currTrackPoint);
		currTrackPoint = false;
	}
}
