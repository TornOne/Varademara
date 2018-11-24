using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCard : Card
{
    protected override bool Activate(Tile tile, Unit caster)
    {
        if (caster.tile.DistanceTo(tile) == 1 && tile.unit != null)
        {
            tile.unit.HP--;
            return true;
        }
        return false;
    }

    internal override int CardValue(Tile tile, EnemyAI caster, Object target, ref Object extra)
    {
        return 0;
    }
}
