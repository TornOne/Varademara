using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MultifunCard : Card
{
    public List<Card> cardfunctions = new List<Card>();

    protected override bool Activate(Tile tile, Unit caster)
    {

        bool retval = true;

        foreach (Card function in cardfunctions)
        {
            Debug.Log("played");
            retval &= function.Use(tile, caster);
        }
        return retval;
    }

    protected override bool PreActivate(Unit caster, bool select)
    {
        bool retval = true;

        foreach (Card function in cardfunctions)
        {
            Debug.Log("selected");
            retval &= function.Select(caster, select);
        }
        return retval;
    }
}
