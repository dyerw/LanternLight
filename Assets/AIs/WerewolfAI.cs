using UnityEngine;
using System.Collections;

public class WerewolfAI : AIInterface
{

	public void TakeTurn(Entity entity, World world) {
		MovementAI.MoveTowards (world, entity, new Vector2 (world.Player.X, world.Player.Y));
	}

}

