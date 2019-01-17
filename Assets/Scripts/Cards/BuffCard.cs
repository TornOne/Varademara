using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffCard : Card
{
    public int healValue = 1;
    public int initiativeValue = 1;
  
    public int castRange = 1;

    public bool all_allies = false;
    public bool all_enemies = false;


    protected override bool PreActivate(Unit caster, bool select)
    {
        return true;
    }

    protected override bool Activate(Tile tile, Unit caster)
    {

        if (all_allies || all_enemies)
        {
            if (all_allies)
            {
                foreach (Unit unit in TurnManager.instance.friendlies)
                {
                    unit.HP += healValue;
                    unit.initiative += initiativeValue;
                }
            }

            if (all_enemies)
            {
                foreach (Unit unit in TurnManager.instance.enemies)
                {
                    unit.HP += healValue;
                    unit.initiative += initiativeValue;
                }
            }

            return true;
        }
        else if (castRange == 0)
        {
            caster.HP += healValue;
            caster.initiative += initiativeValue;
            return true;
        }

        else if (caster.tile.DistanceTo(tile) <= castRange && tile.unit != null)
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
