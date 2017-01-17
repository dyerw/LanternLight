using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldController : MonoBehaviour {

	MouseController mouseController;
	WorldView worldView;
	World world;

	public Sprite mouseCursorSprite;

	// Use this for initialization
	void Start () {
		Debug.Log ("Starting game...");

		Entity player = new Entity (10, 15);
		Entity[] startEntities = new Entity[1];
		startEntities [0] = player;
		world = new World (startEntities);


		GameObject mouseControllerGameObject = new GameObject ();
		mouseController = mouseControllerGameObject.AddComponent<MouseController> ();
		mouseController.OnTileClick = (Vector3 coordinates) => {
			if (coordinates.x == player.X && coordinates.y == player.Y) {
				player.Rotate();
			} else {
				world.MoveEntity(player, coordinates);
			}
				
			renderGameObjects();
		};

		// Setup up cursor object
		GameObject cursorObject = new GameObject();
		SpriteRenderer cursorSpriteRenderer = cursorObject.AddComponent<SpriteRenderer> ();
		cursorSpriteRenderer.sprite = mouseCursorSprite;
		mouseController.mouseCursor = cursorObject;
		cursorObject.transform.SetParent (this.transform, true); 

		worldView = new WorldView ();
		renderGameObjects ();
	}

	void renderGameObjects() {
		foreach (Transform child in this.transform) {
			if (child != mouseController.mouseCursor.transform) {
				GameObject.Destroy (child.gameObject);
			}
		}

		List<GameObject> gameObjects = worldView.GenerateView (this.world, world.Entities[0]);
		foreach (GameObject gameObject in gameObjects) {
			gameObject.transform.SetParent (this.transform, true);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnEndTurnButtonClicked() {
		world.EndTurn ();
		renderGameObjects ();
	}

}

