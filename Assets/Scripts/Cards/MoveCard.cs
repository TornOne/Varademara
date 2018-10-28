using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCard : Card {
	public override bool Activate(Tile tile, Unit caster) {
		caster.MoveTo(tile);
		return true;
	}
}
