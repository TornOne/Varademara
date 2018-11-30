using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendCard : Card
{
    protected override bool PreActivate(Unit caster, bool select)
    {
        return true;
    }

    protected override bool Activate(Tile tile, Unit caster)
    {
        return true;
    }
}