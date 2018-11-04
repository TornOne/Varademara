using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : Unit {
	public override void Activate() {
		//TODO: Add AI
		HashSet<Unit>.Enumerator friendlies = TurnManager.instance.friendlies.GetEnumerator();
		friendlies.MoveNext();
		Unit target = friendlies.Current;

		if (tile.DistanceTo(target.tile) == 1) {
			target.HP--;
		} else {
			//Find adjacent tile that's nearest to target
			Tile nearestTile = tile;
			foreach (Tile adjacentTile in tile.neighbors) {
				if (adjacentTile.IsWalkable && adjacentTile.DistanceTo(target.tile) < nearestTile.DistanceTo(target.tile)) {
					nearestTile = adjacentTile;
				}
			}
			MoveTo(nearestTile);
		}

		TurnManager.instance.NextTurn();
	}
}
