using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

public class WorldController : MonoBehaviour {

	MouseController mouseController;
	WorldView worldView;
	World world;
	private SpatialAStar<SearchPathNode, System.Object> aStar;

	public Sprite mouseCursorSprite;

	// Use this for initialization
	void Start () {
		Debug.Log ("Starting game...");

		Entity player = new Entity (Entity.EntityType.PLAYER, 10, 15);
		List<Entity> enemies = new List<Entity> ();
		enemies.Add(new Entity(Entity.EntityType.WEREWOLF, 11, 15));
		world = new World (player, enemies);

		GameObject mouseControllerGameObject = new GameObject ();
		mouseController = mouseControllerGameObject.AddComponent<MouseController> ();
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

	public SpatialAStar<SearchPathNode, Object> GetAStarInstance() {
		if (aStar != null) {
			return aStar;
		}

		SearchPathNode[,] searchGrid = new SearchPathNode[world.Width, world.Height];
		for(int x = 0; x < world.Width; x++){
			for(int y = 0; y < world.Height; y++){
				searchGrid [x, y] = new SearchPathNode (world.GetTileAt (x, y));
			}
		}
		return new SpatialAStar<SearchPathNode, Object> (searchGrid);
	}

}

