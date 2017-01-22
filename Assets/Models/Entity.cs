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

	public bool CanAttack() {
		return Stats.RemainingMovement >= Stats.EnergyAttackCost;
	}

	public int MakeAttack() {
		Stats.RemainingMovement = Stats.RemainingMovement - Stats.EnergyAttackCost;
		return Stats.Damage;
	}

	public void TakeDamage(int damage) {
		Stats.CurrentHealth = Stats.CurrentHealth - damage;
		if (Stats.CurrentHealth < 0) {
			Stats.CurrentHealth = 0;
		}
	}

	public bool IsDead() {
		return Stats.CurrentHealth == 0;
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
			if (IsBlocked(world, position)) {
				removedTiles.Add(position);
			}
		}
		foreach (Vector2 removedPosition in removedTiles) {
			visiblePoints.Remove (removedPosition);
		}

		return visiblePoints;

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

	public bool IsBlocked(World world, Vector2 position) {
		float x1 = position.x + 0.5f;
		float y1 = position.y + 0.5f;
		float x2 = (float) X + 0.5f;
		float y2 = (float) Y + 0.5f;

		float deltaX = x1 - x2;
		float deltaY = y1 - y2;
		float slope = deltaY / deltaX;
		float b = y1 - (slope * x1);

		List<Vector2> blockingTiles = world.GetBlockingTiles ();
		blockingTiles.Remove (new Vector2 (world.Player.X, world.Player.Y));
		foreach (Vector2 blocker in blockingTiles) {
			if (!InCone (X, Y, (int) blocker.x, (int) blocker.y)) {
				continue;
			}
			if (Vector2.Distance (blocker, new Vector2 (X, Y)) >= Vector2.Distance (new Vector2 (X, Y), position)) {
				continue;
			}

			if (deltaX == 0) {
				if (blocker.x == position.x) {
					return true;
				} else {
					continue;
				}
			}
			if (deltaY == 0) {
				if (blocker.y == position.y) {
					return true;
				} else {
					continue;
				}
			}


			float yLeftPoint = blocker.x * slope + b;
			float yRightPoint = (blocker.x + 1) * slope + b;
			float xBottomPoint = (blocker.y - b) / slope;
			float xTopPoint = ((blocker.y + 1) - b) / slope;

			if (yLeftPoint > blocker.y && yLeftPoint < blocker.y + 1) {
//				Debug.Log ("yl Obstacle at (" + blocker.x + "," + blocker.y + ") blocked tile at (" + x1 + "," + y1 + ")" +
//					" at point (" + blocker.x + "," + yLeftPoint + ")" ); 
				return true;
			}
			if (yRightPoint > blocker.y && yRightPoint < blocker.y + 1) {
//				Debug.Log ("yr Obstacle at (" + blocker.x + "," + blocker.y + ") blocked tile at (" + x1 + "," + y1 + ")" +
//					" at point (" + blocker.x + "," + yRightPoint + ")" ); 
				return true;
			}
			if (xBottomPoint > blocker.x && xBottomPoint < blocker.x + 1) {
//				Debug.Log ("xb Obstacle at (" + blocker.x + "," + blocker.y + ") blocked tile at (" + x1 + "," + y1 + ")" +
//					" at point (" + xBottomPoint + "," + blocker.y + ")" ); 
				return true;
			}
			if (xTopPoint > blocker.x && xBottomPoint < blocker.x + 1) {
//				Debug.Log ("xt Obstacle at (" + blocker.x + "," + blocker.y + ") blocked tile at (" + x1 + "," + y1 + ")" +
//					" at point (" + xTopPoint + "," + blocker.y + ")" ); 
				return true;
			}

			// if the center is on the line
			float lineYAtCenterX = (blocker.x + 0.5f) * slope + b;
			if (lineYAtCenterX == blocker.y + 0.5f) {
				return true;
			}
				
		}

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

