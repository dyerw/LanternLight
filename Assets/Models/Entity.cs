using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity {
	public enum Facing { UP, DOWN, LEFT, RIGHT };

	public Facing CurrentFacing;

	public int X { get; set; }
	public int Y { get; set; }

	public Stats Stats;

	public Entity(int x, int y) {
		X = x;
		Y = y;
		CurrentFacing = Facing.RIGHT;

		Stats = new Stats (8);
	}

	public List<Vector2> CalculateVisiblePoints(World world) {

		List<Vector2> visiblePoints = new List<Vector2> ();

		for (int x = 0; x < world.Width; x++) {
			for (int y = 0; y < world.Height; y++) {
				if (Vector2.Distance (new Vector2 (x, y), new Vector2 (X, Y)) <= Stats.ViewingDistance) {
					visiblePoints.Add (new Vector2 (x, y));
				}
			}
		}

		return visiblePoints;

	}
}

