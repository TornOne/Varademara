using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendCard : Card
{
    protected override bool Activate(Tile tile, Unit caster)
    {
        return true;
    }

    internal override int CardValue(Tile tile, EnemyAI caster, Object target, ref Object extra)
    {
        return 0;
    }
}