﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World {

	// A two-dimensional array to hold our tile data.
	public Tile[,] tiles { get; protected set; }

	// An array of all entities in the world
	public Entity[] Entities { get; protected set; }

	// The tile width of the world.
	public int Width { get; protected set; }

	// The tile height of the world
	public int Height { get; protected set; }

	public World(Entity[] entities, int height=30, int width=30) {

		Width = width;
		Height = height;
		Entities = entities;

		tiles = new Tile[Width,Height];

		for (int x = 0; x < Width; x++) {
			for (int y = 0; y < Height; y++) {
				Tile.TileType tileType = Random.Range(0, 5) == 0 ? Tile.TileType.Tree : Tile.TileType.Empty;
				tiles[x,y] = new Tile(tileType);
			}
		}

		Debug.Log ("World created with " + (Width*Height) + " tiles.");

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

	public void MoveEntity(Entity entity, Vector3 newPosition) {
		foreach (Entity entity2 in Entities) {
			if (entity2 == entity) {
				float distance = Mathf.Floor (Vector2.Distance (new Vector2 (entity.X, entity.Y), new Vector2(newPosition.x, newPosition.y)));
				if (entity.Stats.RemainingMovement >= distance) {
					entity.X = (int)newPosition.x;
					entity.Y = (int)newPosition.y;
					entity.Stats.RemainingMovement = (int) (entity.Stats.RemainingMovement - distance);
				}
			}
		}
	}

	public void EndTurn() {
		foreach (Entity entity in Entities) {
			entity.Stats.RemainingMovement = entity.Stats.MovementRange;
		}
	}

}