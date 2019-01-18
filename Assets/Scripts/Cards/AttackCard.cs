using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCard : Card {
	//TODO: different damage types for attack cards
	public int dmgValue = 1;
	public int castRange = 1;

    public bool all_allies = false;
    public bool all_enemies = false;

    public AudioClipGroup attackAudio;

	protected override bool PreActivate(Unit caster, bool select) {
		if (select) {
			tiles = caster.tile.BuildFlightMap(castRange);
			foreach (KeyValuePair<Tile, int> tile in tiles) {
				tile.Key.Color = new Color(0.8f, 0, 0);
			}
		} else {
			foreach (KeyValuePair<Tile, int> tile in tiles) {
				tile.Key.Color = Color.black;
			}
		}
		return true;
	}

	protected override bool Activate(Tile tile, Unit caster) {

        if (all_allies || all_enemies)
        {
            if (attackAudio != null)
                attackAudio.Play();
            if (all_allies)
            {
                foreach (Unit unit in TurnManager.instance.friendlies)
                {
                    unit.HP -= dmgValue;
                }
            }

            if (all_enemies)
            {
                foreach (Unit unit in TurnManager.instance.enemies)
                {
                    unit.HP -= dmgValue;
                }
            }

            return true;
        }

        if (castRange==0)
        {
            if (attackAudio != null)
                attackAudio.Play();
            caster.HP -= dmgValue;
            return true;
        }

		else if (caster.tile.DistanceTo(tile) <= castRange && tile.unit != null) {
			if (attackAudio != null)
				attackAudio.Play();
			tile.unit.HP -= dmgValue;
			return true;
		}
		return false;
	}

	public override int CardValue(Tile tile, EnemyAI caster, Object target, ref Object extra) {
		if (tile.DistanceTo(((Unit) target).tile) <= castRange && (Unit) target != null) {
			//TODO: proper damage calculation against targets armor
			return dmgValue;
		}
		return 0;
	}
}
