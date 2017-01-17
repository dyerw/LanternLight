using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class WorldView {

	public Sprite treeSprite;
	public Sprite grassSprite;
	public Sprite playerSprite;
	public Sprite unseenGroundSprite;
	public Sprite unseenTreeSprite;
	Text remainingMovesText;

	public WorldView() {
		treeSprite = Resources.Load<Sprite> ("tree_sprite");
		grassSprite = Resources.Load<Sprite> ("ground");
		playerSprite = Resources.Load<Sprite> ("player_sprite");
		unseenGroundSprite = Resources.Load<Sprite> ("unseen_ground");
		unseenTreeSprite = Resources.Load<Sprite> ("unseen_tree");
		remainingMovesText = GameObject.Find ("RemainingMovesText").GetComponent<Text> ();
	}

	public List<GameObject> GenerateView(World world, Entity player) {
		List<GameObject> gameObjects = GenerateTiles (world, player);
		gameObjects.AddRange (GenerateEntites (world.Entities));
		UpdateStatsUI (world);
		return gameObjects;
	}

	List<GameObject> GenerateTiles(World world, Entity player) {
		List<GameObject> tileObjects = new List<GameObject> ();

		List<Vector2> visibleTiles = player.CalculateVisiblePoints (world);

		// Create terrain
		for (int x = 0; x < world.Width; x++) {
			for (int y = 0; y < world.Height; y++) {
				if (world.EntityOnTile (x, y)) {
					continue;
				}

				Tile tile = world.GetTileAt (x, y);

				GameObject tileGameObject = new GameObject ();
				tileGameObject.name = "Tile_" + x + "_" + y;
				tileGameObject.transform.position = new Vector3( x, y, 0);

				SpriteRenderer spriteRenderer = tileGameObject.AddComponent<SpriteRenderer> ();


				if (visibleTiles.Contains (new Vector2 (x, y))) {
					if (tile.Type == Tile.TileType.Tree) {
						spriteRenderer.sprite = treeSprite;
					} else {
						spriteRenderer.sprite = grassSprite;
					}
				} else {
					if (tile.Type == Tile.TileType.Tree) {
						spriteRenderer.sprite = unseenTreeSprite;
					} else {
						spriteRenderer.sprite = unseenGroundSprite;
					}
				}

				tileObjects.Add (tileGameObject);
			}
		}

		return tileObjects;
	}

	List<GameObject> GenerateEntites(Entity[] entities) {
		List<GameObject> gameObjects = new List<GameObject> ();

		// Draw entities
		foreach (Entity entity in entities) {
			GameObject entityGameObject = new GameObject(); 
			entityGameObject.name = "Player";
			entityGameObject.transform.position = new Vector3( entity.X, entity.Y, 0);

			SpriteRenderer entitySpriteRenderer = entityGameObject.AddComponent<SpriteRenderer> ();
			entitySpriteRenderer.sprite = playerSprite;

			gameObjects.Add (entityGameObject);
		}

		return gameObjects;
	}

	public void UpdateStatsUI(World world) {
		remainingMovesText.text = "" + world.Entities[0].Stats.RemainingMovement;
	}
}

