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
	public Sprite werewolfSprite;
	public Sprite healthBarSprite;
	Text remainingMovesText;

	public WorldView() {
		treeSprite = Resources.Load<Sprite> ("tree_sprite");
		grassSprite = Resources.Load<Sprite> ("ground");
		playerSprite = Resources.Load<Sprite> ("player_sprite");
		unseenGroundSprite = Resources.Load<Sprite> ("unseen_ground");
		unseenTreeSprite = Resources.Load<Sprite> ("unseen_tree");
		werewolfSprite = Resources.Load<Sprite> ("werewolf");
		healthBarSprite = Resources.Load<Sprite> ("healthbar");
		remainingMovesText = GameObject.Find ("RemainingMovesText").GetComponent<Text> ();
	}

	public List<GameObject> GenerateView(World world) {
		List<GameObject> gameObjects = GenerateTiles (world, world.Player);
		gameObjects.AddRange (GenerateEntites (world, world.AliveEntities));
		gameObjects.AddRange (DrawHealthBars (world.AliveEntities));
		UpdateStatsUI (world);
		return gameObjects;
	}

	List<GameObject> GenerateTiles(World world, Entity player) {
		List<GameObject> tileObjects = new List<GameObject> ();

		List<Vector2> visibleTiles = player.CalculateVisiblePoints (world);

		// Create terrain
		for (int x = 0; x < world.Width; x++) {
			for (int y = 0; y < world.Height; y++) {
//				if (world.EntityOnTile (x, y)) {
//					continue;
//				}

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

	List<GameObject> GenerateEntites(World world, List<Entity> entities) {
		List<GameObject> gameObjects = new List<GameObject> ();

		// Draw entities
		foreach (Entity entity in entities) {
			GameObject entityGameObject = new GameObject(); 
			entityGameObject.name = "Player";
			entityGameObject.transform.position = new Vector3( entity.X, entity.Y, -1);

			SpriteRenderer entitySpriteRenderer = entityGameObject.AddComponent<SpriteRenderer> ();

			if (entity.Type == Entity.EntityType.PLAYER) {
				entitySpriteRenderer.sprite = playerSprite;
			} else {
				if (world.Player.CalculateVisiblePoints (world).Contains (new Vector2 (entity.X, entity.Y))) {
					entitySpriteRenderer.sprite = werewolfSprite;
				} else {
					entitySpriteRenderer.sprite = unseenGroundSprite;
				}
			}


			gameObjects.Add (entityGameObject);
		}

		return gameObjects;
	}

	public void UpdateStatsUI(World world) {
		remainingMovesText.text = "" + world.Player.Stats.RemainingMovement;
	}

	public List<GameObject> DrawHealthBars(List<Entity> entities) {
		List<GameObject> gameObjects = new List<GameObject> ();

		foreach (Entity entity in entities) {
			GameObject entityGameObject = new GameObject (); 
			entityGameObject.name = "HealthBar";
			entityGameObject.transform.position = new Vector3 (entity.X, entity.Y, -2);
			SpriteRenderer entitySpriteRenderer = entityGameObject.AddComponent<SpriteRenderer> ();
			entitySpriteRenderer.sprite = healthBarSprite;

			float healthPercentage = (float) entity.Stats.CurrentHealth / (float) entity.Stats.MaxHealth;
			entityGameObject.transform.localScale = new Vector3 (healthPercentage, 1, 1);

			gameObjects.Add (entityGameObject);
		}

		return gameObjects;
	}

}

