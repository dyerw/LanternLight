using UnityEngine;
using System.Collections;

public class WerewolfAI
{

	public static void TakeTurn(Entity entity, World world) {
		Vector2 playerPos = new Vector2 (world.Player.X, world.Player.Y);
		Vector2 entityPos = new Vector2 (entity.X, entity.Y);
		if (Vector2.Distance (playerPos, entityPos) == 1) {
			Actions.Attack (entity, world.Player);
		} else {
			MovementAI.MoveTowards (world, entity, playerPos);
		}
	}

}

