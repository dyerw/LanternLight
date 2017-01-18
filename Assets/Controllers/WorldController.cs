using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldController : MonoBehaviour {

	MouseController mouseController;
	WorldView worldView;
	World world;

	public Sprite mouseCursorSprite;
	public Sprite enemyMouseCursorSprite;

	// Use this for initialization
	void Start () {
		Debug.Log ("Starting game...");

		Entity player = new Entity (Entity.EntityType.PLAYER, 5, 5);
		List<Entity> enemies = new List<Entity> ();
		enemies.Add(new Entity(Entity.EntityType.WEREWOLF, 11, 15));
		world = new World (player, enemies);


		GameObject mouseControllerGameObject = new GameObject ();
		mouseController = mouseControllerGameObject.AddComponent<MouseController> ();
		mouseController.world = world;
		mouseController.OnTileClick = (Vector3 coordinates) => {
			if (coordinates.x == player.X && coordinates.y == player.Y) {
				player.Rotate();
			} else if (world.GetTileAt((int) coordinates.x,(int) coordinates.y).Type == Tile.TileType.Empty) {
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

		mouseController.enemyCursorSprite = enemyMouseCursorSprite;
		mouseController.cursorSprite = mouseCursorSprite;

		worldView = new WorldView ();
		renderGameObjects ();
	}

	void renderGameObjects() {
		foreach (Transform child in this.transform) {
			if (child != mouseController.mouseCursor.transform) {
				GameObject.Destroy (child.gameObject);
			}
		}

		List<GameObject> gameObjects = worldView.GenerateView (this.world);
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

