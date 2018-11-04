using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCard : Card {
	public override bool Activate(Tile tile, Unit caster) {
		if (caster.tile.DistanceTo(tile) == 1 && tile.unit != null) {
			tile.unit.HP--;
			return true;
		}
		return false;
	}
}
