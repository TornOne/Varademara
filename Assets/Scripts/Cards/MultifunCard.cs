using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultifunCard : Card
{
    public List<Card> cardfunctions = new List<Card>();

    protected override bool Activate(Tile tile, Unit caster)
    {
        bool retval = true;

        foreach (Card function in cardfunctions)
        {
            retval &= function.Use(tile, caster);
        }
        return retval;
    }

    protected override bool PreActivate(Unit caster, bool select)
    {
        bool retval = true;

        foreach (Card function in cardfunctions)
        {
            retval &= function.Select(caster, select);
        }
        return retval;
    }
}
