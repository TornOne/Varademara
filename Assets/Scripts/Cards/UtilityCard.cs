using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityCard : Card
{

    protected override bool Activate(Tile tile, Unit caster)
    {
        return false;
    }

    protected override bool PreActivate(Unit caster, bool select)
    {
        return true;
    }
}
