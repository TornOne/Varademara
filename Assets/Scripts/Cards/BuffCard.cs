using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffCard : Card
{
    public int healValue = 1;
    public int initiativeValue = 1;
  
    public int castRange = 1;
    

    protected override bool Activate(Tile tile, Unit caster)
    {
        if (caster.tile.DistanceTo(tile) <= castRange && tile.unit != null)
        {
            tile.unit.HP += healValue;
            tile.unit.initiative += initiativeValue;
            return true;
        }
        return false;
    }

    public override int CardValue(Tile tile, EnemyAI caster, Object target, ref Object extra)
    {
        if (tile.DistanceTo(((Unit)target).tile) <= castRange && (Unit)target != null)
        {
            //TODO: proper heal/buff calculations
            return healValue + initiativeValue/10;
        }
        return 0;
    }
}
