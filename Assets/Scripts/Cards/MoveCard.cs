using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCard : Card {
	protected override bool Activate(Tile tile, Unit caster) {
		Dictionary<Tile, int> tiles = caster.tile.BuildWalkMap(3);

		if (tiles.ContainsKey(tile)) {
			caster.Move(Map.instance.PathTo(tile));
			return true;
		}
		return false;
	}
}
