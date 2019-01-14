using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MoveCard : Card {

	public int moveValue = 3;

	//public Color highlightColor = new Color(0.7f,0.7f,0); //Doesnt init
	//protected Dictionary<Tile, int> tiles;
	//protected int value = 0;

	protected override bool PreActivate(Unit caster, bool select) {
		if (select) {
			tiles = caster.tile.BuildWalkMap(moveValue);
			foreach (KeyValuePair<Tile, int> tile in tiles) {
				tile.Key.sprite.color = new Color(0, 0.8f, 0.8f);
			}
		} else {
			foreach (KeyValuePair<Tile, int> tile in tiles) {
				tile.Key.sprite.color = Color.black;
			}
		}
		return true;
	}

	protected override bool Activate(Tile tile, Unit caster) {
		//Dictionary<Tile, int> tiles = caster.tile.BuildWalkMap(moveValue);

		if (tiles.ContainsKey(tile)) {
			caster.Move(Map.instance.PathTo(tile));
			return true;
		}
		return false;
	}


	public override int CardValue(Tile tile, EnemyAI caster, Object target, ref Object extra) {

		int currentDistance = tile.DistanceTo((Tile) target);

		if (caster.optimalDistane == currentDistance)
			return int.MaxValue;

		//Dictionary<Tile, int> 
		tiles = tile.BuildWalkMap(moveValue);
		Dictionary<Tile, int> errors = new Dictionary<Tile, int>();

		foreach (KeyValuePair<Tile, int> potentialTile in tiles) {
			errors[potentialTile.Key] = Mathf.Abs(caster.optimalDistane - potentialTile.Key.DistanceTo((Tile) target));

			if (caster.optimalDistane == (potentialTile.Key.DistanceTo((Tile) target))) {
				extra = potentialTile.Key;

				return errors[potentialTile.Key];
			}
		}

		extra = errors.OrderBy(kvp => kvp.Value).First().Key;
		//Debug.Log(errors[(Tile)extra]);
		if (errors[(Tile) extra] == currentDistance)
			return int.MaxValue;

		return errors[(Tile) extra];
	}
}
