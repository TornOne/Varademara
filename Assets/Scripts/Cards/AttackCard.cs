using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AttackCard : Card
{
    //TODO: different damage types for attack cards
    public int dmgValue = 1;
    public int castRange = 1;

    protected override bool Activate(Tile tile, Unit caster)
    {
        if (caster.tile.DistanceTo(tile) == castRange && tile.unit != null)
        {
            tile.unit.HP-= dmgValue;
            return true;
        }
        return false;
    }

    public override int CardValue(Tile tile, EnemyAI caster, Object target, ref Object extra)
    {
        if (tile.DistanceTo(((Unit)target).tile) <= castRange && (Unit)target != null)
        {
            //TODO: proper damage calculation against targets armor
            return dmgValue;
        }
        return 0;
    }
}
