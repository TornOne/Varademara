using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AttackCard : Card
{
    //TODO: different damage types for attack cards
    public int dmgValue = 1;
    public int castRange = 1;

    public AudioClipGroup attackAudio;

    protected override bool PreActivate(Unit caster, bool select)
    {
        Color highlightColor;
        if (select)
        {
            tiles = caster.tile.BuildFlightMap(castRange);
            highlightColor = new Color(0.8f, 0, 0);
            foreach (KeyValuePair<Tile, int> tile in tiles)
            {
                tile.Key.SetMovableHighlight(highlightColor);
            }

        }
        else
        {
            highlightColor = new Color();
            foreach (KeyValuePair<Tile, int> tile in tiles)
            {
                tile.Key.SetMovableHighlight(highlightColor);
            }
        }
        return true;
    }

    protected override bool Activate(Tile tile, Unit caster)
    {
        if (caster.tile.DistanceTo(tile) <= castRange && tile.unit != null)
        {
            if (attackAudio != null) attackAudio.Play();
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
