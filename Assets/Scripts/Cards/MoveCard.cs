using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MoveCard : Card {

    public int moveValue = 3;

	protected override bool Activate(Tile tile, Unit caster) {
		Dictionary<Tile, int> tiles = caster.tile.BuildWalkMap(moveValue);

		if (tiles.ContainsKey(tile)) {
			caster.Move(Map.instance.PathTo(tile));
			return true;
		}
		return false;
	}


    internal override int CardValue(Tile tile, EnemyAI caster, Object target, ref Object extra)
    {

        int currentDistance = tile.DistanceTo((Tile)target);

        if (caster.optimalDistane == currentDistance) return int.MaxValue;

        Dictionary<Tile, int> tiles = tile.BuildWalkMap(moveValue);
        Dictionary<Tile, int> errors = new Dictionary<Tile, int>();

        foreach (KeyValuePair <Tile,int> potentialTile in tiles)
        {
            errors[potentialTile.Key] = Mathf.Abs(caster.optimalDistane - potentialTile.Key.DistanceTo((Tile)target));

            if (caster.optimalDistane == (potentialTile.Key.DistanceTo((Tile)target)))
            {
                extra = potentialTile.Key;

                return errors[potentialTile.Key];
            }
        }
        
        extra = errors.OrderBy(kvp => kvp.Value).First().Key;
        Debug.Log(errors[(Tile)extra]);


        return errors[(Tile)extra];
    }
}
