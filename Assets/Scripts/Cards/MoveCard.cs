using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCard : Card {
	public override bool Activate(Tile tile, Unit caster) {
		if (caster.tile.DistanceTo(tile) == 1) {
			caster.MoveTo(tile);
			return true;
		}
		return false;
	}
}
