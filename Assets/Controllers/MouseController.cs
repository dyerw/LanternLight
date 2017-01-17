using UnityEngine;
using System.Collections;
using System;
using UnityEngine.EventSystems;

public class MouseController : MonoBehaviour
{

	Vector3 lastFramePosition;
	public Action<Vector3> OnTileClick { get; set; }

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		Vector3 currFramePosition = Camera.main.ScreenToWorldPoint( Input.mousePosition );
		currFramePosition.z = 0;

		// Handle screen dragging
		if( Input.GetMouseButton(0)  ) {	

			Vector3 diff = lastFramePosition - currFramePosition;
			Camera.main.transform.Translate( diff );

		}

		lastFramePosition = Camera.main.ScreenToWorldPoint( Input.mousePosition );
		lastFramePosition.z = 0;

		if (Input.GetMouseButtonUp (0)) {
			if (EventSystem.current.IsPointerOverGameObject ()) {
				return;
			}
			Vector3 coordinates = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			coordinates.x = Mathf.Floor (coordinates.x);
			coordinates.y = Mathf.Floor (coordinates.y);
			OnTileClick (coordinates);
		}

	}
}

