using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {


    private UnitScript unit;
	// Use this for initialization
	void Start () {
        unit = gameObject.transform.GetComponent<UnitScript>();
    }

    //main enemy turn handler
    public void AITurn()
    {
        AIMove();
        AIPlayCard();
    }

    //move enemy towareds the closest player character
    private void AIMove()
    {
        List<MovementScript.TilePlaceholder> possible_tiles = unit.move_calc.CalculateMovement(unit.tile, unit.mP);


        UnitScript targetAlly = null;
        float targetDistance = float.PositiveInfinity;

        //find closest player character
        foreach (UnitScript ally in TurnScript.instance.PCunits)
        {
            if (Vector3.Distance(unit.tile.Position(), ally.tile.Position()) < targetDistance)
            {
                targetDistance = Vector3.Distance(unit.tile.Position(), ally.transform.position);
                targetAlly = ally;
            }
        }

        //among the tiles that can move to, find the closest to the target player character
        MovementScript.TilePlaceholder targetTile = null;
        targetDistance = float.PositiveInfinity;

        foreach (MovementScript.TilePlaceholder tile in possible_tiles)
        {
            if (Vector3.Distance(tile.Position(), targetAlly.tile.Position()) < targetDistance)
            {
                targetDistance = Vector3.Distance(tile.Position(), targetAlly.tile.Position());
                //print(targetDistance);
                targetTile = tile;
            }
        }

        //move to tile
        List<MovementScript.TilePlaceholder> path = unit.move_calc.FindPathTo(targetTile);


        unit.move_calc.MoveToTile(path, unit);
    }

    //
    private void AIPlayCard()
    {
        //find closest player character
        UnitScript targetAlly = null;
        foreach (UnitScript ally in TurnScript.instance.PCunits)
        {
            //if they are close enough, attack
            print(Vector3.Distance(unit.tile.Position(), ally.tile.Position()));
            if (Vector3.Distance(unit.tile.Position(), ally.tile.Position()) <= 2)
            {
                targetAlly = ally;
                unit.attackLerp = 1;
                unit.attack_calc.Damage(2, 2, 0, ally);
                break;
            }
        }
    }
}
