using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldController : MonoBehaviour {

	MouseController mouseController;
	WorldView worldView;
	World world;

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
			world.MoveEntity(player, coordinates);
			renderGameObjects();
		};
			

		worldView = new WorldView ();
		renderGameObjects ();
	}

	void renderGameObjects() {
		foreach (Transform child in this.transform) {
			GameObject.Destroy(child.gameObject);
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

