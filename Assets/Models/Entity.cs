using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity {
	public enum EntityType { PLAYER, WEREWOLF }
	public enum Facing { UP, DOWN, LEFT, RIGHT };

	public Facing CurrentFacing;
	public EntityType Type;

	public int X { get; set; }
	public int Y { get; set; }

	public Stats Stats;

	public Entity(EntityType type, int x, int y) {
		X = x;
		Y = y;
		CurrentFacing = Facing.RIGHT;
		Type = type;

		Stats = new Stats (8);
	}

	public List<Vector2> CalculateVisiblePoints(World world) {

		List<Vector2> visiblePoints = new List<Vector2> ();

		for (int x = 0; x < world.Width; x++) {
			for (int y = 0; y < world.Height; y++) {
				if (Vector2.Distance (new Vector2 (x, y), new Vector2 (X, Y)) <= Stats.ViewingDistance &&
					InCone(X, Y, x, y)) {
					visiblePoints.Add (new Vector2 (x, y));
				}
			}
		}

		List<Vector2> removedTiles = new List<Vector2> ();
		foreach (Vector2 position in visiblePoints) {
//			if (world.GetTileAt (position.x, position.y).Type != Tile.TileType.Empty) {
//				BlockHidden
//			}
			if (world.GetTileAt ((int) position.x, (int) position.y).Type != Tile.TileType.Empty &&
				IsBlocked(world, position)) {
				removedTiles.Add(position);
			}
		}
		foreach (Vector2 removedPosition in removedTiles) {
			visiblePoints.Remove (removedPosition);
		}

		return visiblePoints;

	}

	public void BlockCastPoints(World world) {
		
	
	}

	public bool InCone(int entityX, int entityY, int targetX, int targetY) {
		switch (this.CurrentFacing)
		{
			case Facing.DOWN:
				return (entityY - targetY) >= Mathf.Abs (entityX - targetX);
			case Facing.UP:
				return (0 - (entityY - targetY)) >= Mathf.Abs (entityX - targetX);
			case Facing.LEFT:
				return Mathf.Abs (entityY - targetY) <= (entityX - targetX);
			case Facing.RIGHT:
				return Mathf.Abs (entityY - targetY) <= (0 - (entityX - targetX));
			default:
				return true;
		}
	}
		

	public bool IsBlocked(World world, Vector2 targetPosition) {
		Vector2 startPos = new Vector2 (X, Y);
		int tileDist = Vector2.Distance (startPos, targetPosition);
		int pathDist = WorldController.GetAStarInstance()
			.Search(startPos, targetPosition, null).Count;

		return false;
	}



	public void Rotate() {
		switch (this.CurrentFacing) {
			case Facing.DOWN:
				this.CurrentFacing = Facing.LEFT;
				break; 
			case Facing.UP:
				this.CurrentFacing = Facing.RIGHT;
				break; 
			case Facing.LEFT:
				this.CurrentFacing = Facing.UP;
				break;
			case Facing.RIGHT:
				this.CurrentFacing = Facing.DOWN;
				break;
			default:
				this.CurrentFacing = Facing.RIGHT;
				break;
		}
	}
}

