using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovementAI
{

	public static void MoveTowards(World world, Entity entity, Vector2 position) {
		List<Vector2> path = GetPathToPoint (new Vector2 (entity.X, entity.Y), position, world);
		world.MoveEntity (entity, path [entity.Stats.RemainingMovement]);
	}

	static List<Vector2> GetNeighbors(Vector2 vector, World world) {
		List<Vector2> neighbors = new List<Vector2> ();
		neighbors.Add (new Vector2 (vector.x + 1, vector.y));
		neighbors.Add (new Vector2 (vector.x - 1, vector.y));
		neighbors.Add (new Vector2 (vector.x, vector.y + 1));
		neighbors.Add (new Vector2 (vector.x, vector.y - 1));
		neighbors.Add (new Vector2 (vector.x + 1, vector.y + 1));
		neighbors.Add (new Vector2 (vector.x - 1, vector.y + 1));
		neighbors.Add (new Vector2 (vector.x + 1, vector.y - 1));
		neighbors.Add (new Vector2 (vector.x - 1, vector.y - 1));

		foreach (Vector2 blocker in world.GetBlockingTiles()) {
			neighbors.Remove (blocker);
		}
		return neighbors;
	} 

	public static List<Vector2> GetPathToPoint(Vector2 origin, Vector2 destination, World world) {
		// A Star implementation

		List<Vector2> closedSet = new List<Vector2> ();
		List<Vector2> openSet = new List<Vector2> ();
		openSet.Add (origin);

		Dictionary<Vector2, Vector2> cameFrom = new Dictionary<Vector2, Vector2> ();

		Dictionary<Vector2, int> gScore = new Dictionary<Vector2, int> ();
		gScore.Add (origin, 0);

		Dictionary<Vector2, int> fScore = new Dictionary<Vector2, int> ();
		fScore.Add (origin, (int) Vector2.Distance (origin, destination));

		while (openSet.Count > 0) {
			Vector2 current = openSet[0];
			foreach (Vector2 vector in openSet) {
				if (fScore [vector] <= fScore [current]) {
					current = vector;
				}
			}

			if (current == destination) {
				List<Vector2> path = new List<Vector2> ();
				while (current != origin) {
					path.Add (current);
					current = cameFrom [current];
				}
				path.Add (origin);
				path.Reverse ();
				return path;
			}

			openSet.Remove (current);
			closedSet.Add (current);

			foreach (Vector2 neighbor in GetNeighbors(current, world)) {
				if (closedSet.Contains(neighbor)) {
					continue;
				}

				int tentativeGScore = gScore [current] + 1;

				if (!openSet.Contains (neighbor)) {
					openSet.Add (neighbor);
				} else if (tentativeGScore >= gScore [neighbor]) {
					continue;
				}

				cameFrom.Add (neighbor, current);
				gScore [neighbor] = tentativeGScore;
				fScore[neighbor] = gScore[neighbor] + (int) Vector2.Distance(neighbor, destination);

			}
		}

		Debug.LogError ("Could not find path from (" + origin.x + "," + origin.y + ") to (" + destination.x + "," + destination.y + ")");
		return new List<Vector2> ();
	}

}

