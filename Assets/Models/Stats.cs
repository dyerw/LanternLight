using UnityEngine;
using System.Collections;

public class Stats {

	public int ViewingDistance;
	public int MovementRange;
	public int RemainingMovement { set; get; }
	public int Damage { get; protected set; }
	public int MaxHealth { get; protected set; }
	public int CurrentHealth { get; set; }
	public int EnergyAttackCost { get; protected set; }

	public Stats(int viewingDistance) {
		ViewingDistance = viewingDistance;
		MovementRange = RemainingMovement = 2;
		Damage = 3;
		MaxHealth = CurrentHealth = 10;
		EnergyAttackCost = 2;
	}

}

