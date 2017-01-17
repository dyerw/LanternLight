using UnityEngine;
using System.Collections;

public class Stats {

	public int ViewingDistance;
	public int MovementRange;
	public int RemainingMovement { set; get; }

	public Stats(int viewingDistance) {
		ViewingDistance = viewingDistance;
		MovementRange = 5;
		RemainingMovement = 5;
	}

}

