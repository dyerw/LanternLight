﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World {

	// A two-dimensional array to hold our tile data.
	public Tile[,] tiles { get; protected set; }

	// An array of all entities in the world
	public List<Entity> Entities { 
		get {
			List<Entity> allEntities = new List<Entity> ();
			allEntities.AddRange (EnemyEntities);
			allEntities.Add (Player);
			return allEntities;
		}
	}

	public List<Entity> AliveEntities {
		get {
			List<Entity> aliveEntities = new List<Entity> ();
			foreach (Entity entity in EnemyEntities) {
				if (!entity.IsDead ()) {
					aliveEntities.Add (entity);
				}
			}
			if (!Player.IsDead()) {
				aliveEntities.Add (Player);
			}
			return aliveEntities;
		}
	}

	public Entity Player { get; protected set; }
	public List<Entity> EnemyEntities { get; protected set; }

	// The tile width of the world.
	public int Width { get; protected set; }

	// The tile height of the world
	public int Height { get; protected set; }

	public World(Entity player, List<Entity> enemyEntities, int height=30, int width=30) {

		Width = width;
		Height = height;
		Player = player;
		EnemyEntities = enemyEntities;

		tiles = new Tile[Width,Height];

		for (int x = 0; x < Width; x++) {
			for (int y = 0; y < Height; y++) {
				Tile.TileType tileType = Random.Range(0, 10) == 0 ? Tile.TileType.Tree : Tile.TileType.Empty;
				tiles[x,y] = new Tile(tileType);
			}
		}

		Debug.Log ("World created with " + (Width*Height) + " tiles.");

	}

	public Entity GetEntityAt(Vector3 position) {
		foreach (Entity entity in Entities) {
			if (position.x == entity.X && position.y == entity.Y) {
				return entity;
			}
		}
		Debug.LogError ("No entity found at position (" + position.x + "," + position.y + ")");
		return null;
	}

	public Tile GetTileAt(int x, int y) {
		return tiles [x, y];
	}

	public bool EntityOnTile(int x, int y) {
		foreach (Entity entity in Entities) {
			if (entity.X == x && entity.Y == y) {
				return true;
			}
		}

		return false;
	}

	public void MoveEntity(Entity entity, Vector2 newPosition) {
		foreach (Entity entity2 in Entities) {
			if (entity2 == entity) {
				float distance = Mathf.Floor (Vector2.Distance (new Vector2 (entity.X, entity.Y), new Vector2(newPosition.x, newPosition.y)));
				if (entity.Stats.RemainingMovement >= distance && !GetBlockingTiles().Contains(newPosition)) {
					entity.X = (int)newPosition.x;
					entity.Y = (int)newPosition.y;
					entity.Stats.RemainingMovement = (int) (entity.Stats.RemainingMovement - distance);
				}
			}
		}
	}

	public List<Vector2> GetBlockingTiles() {
		List<Vector2> blockingTiles = new List<Vector2> ();
		foreach (Entity entity in EnemyEntities) {
			if (!entity.IsDead ()) { 
				blockingTiles.Add (new Vector2 (entity.X, entity.Y));
			}
		}
		blockingTiles.Add (new Vector2 (Player.X, Player.Y));

		for (int x = 0; x < Width; x++) {
			for (int y = 0; y < Height; y++) {
				if (GetTileAt (x, y).Type != Tile.TileType.Empty) {
					blockingTiles.Add (new Vector2 (x, y));
				}
			}
		}

		return blockingTiles;
	}

	public bool ContainsEnemy(Vector3 position) {
		foreach (Entity enemy in EnemyEntities) {
			if (enemy.X == position.x && enemy.Y == position.y && !enemy.IsDead()) {
				return true;
			}
		}
		return false;
	}

	public void EndTurn() {
		foreach (Entity entity in EnemyEntities) {
			WerewolfAI.TakeTurn (entity, this);
		}

		foreach (Entity entity in Entities) {
			entity.Stats.RemainingMovement = entity.Stats.MovementRange;
		}
	}

}
