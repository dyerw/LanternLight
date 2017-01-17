using UnityEngine;
using System.Collections;
using System;
using UnityEngine.EventSystems;

public class MouseController : MonoBehaviour
{

	Vector3 lastFramePosition;
	public Action<Vector3> OnTileClick { get; set; }
	public GameObject mouseCursor { get; set; }
	public Sprite enemyCursorSprite { get; set; }
	public Sprite cursorSprite { get; set; }
	public World      world { get; set; }

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		UpdateMouseCursor ();

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
			OnTileClick (GameCoordinatesOfMouse());
		}

	}

	Vector3 GameCoordinatesOfMouse() {
		Vector3 coordinates = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		coordinates.x = Mathf.Floor (coordinates.x + 0.5f);
		coordinates.y = Mathf.Floor (coordinates.y + 0.5f);
		return coordinates;
	}

	void UpdateMouseCursor() {
		Vector3 mousePosition = GameCoordinatesOfMouse();
		bool containsEnemy = world.ContainsEnemy (mousePosition);
		mouseCursor.GetComponent<SpriteRenderer> ().sprite = containsEnemy ? enemyCursorSprite : cursorSprite;

		mouseCursor.transform.position = new Vector3 (mousePosition.x, mousePosition.y, -1);
	}
}

