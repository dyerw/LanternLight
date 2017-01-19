using UnityEngine;
using System.Collections;

public static class Actions
{

	public static void Attack(Entity attacker, Entity defender) {
		if (attacker.CanAttack ()) {
			defender.TakeDamage (attacker.MakeAttack ());
			Debug.Log (defender.Type + " has " + defender.Stats.CurrentHealth + " health");
		}
	}

}

