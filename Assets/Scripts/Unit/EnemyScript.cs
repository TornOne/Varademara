using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {


    private Unit unit;
	// Use this for initialization
	void Start () {
        unit = gameObject.transform.GetComponent<Unit>();
    }

    //main enemy turn handler
    public void AITurn()
    {
        AIMove();
        //AIPlayCard();
    }

    //move enemy towareds the closest player character
    private void AIMove()
    {
        List<Tile> possible_tiles = unit.move_calc.CalculateMovement(unit.tile, unit.speed);


        Unit targetAlly = null;
        float targetDistance = float.PositiveInfinity;

        //find closest player character
        foreach (Unit ally in TurnScript.instance.PCunits)
        {
            if (Vector3.Distance(unit.tile.transform.position, ally.tile.transform.position) < targetDistance)
            {
                targetDistance = Vector3.Distance(unit.tile.transform.position, ally.transform.position);
                targetAlly = ally;
            }
        }

        //print("Target position: "+new Vector2(targetAlly.tile.x, targetAlly.tile.y));

        //among the tiles that can move to, find the closest to the target player character
        Tile targetTile = null;
        targetDistance = float.PositiveInfinity;
        //int targetDistance2 = int.MaxValue;
        foreach (Tile tile in possible_tiles)
        {
            //print("Tile[x,y,dist]: " + new Vector3(tile.x, tile.y, targetAlly.tile.DistanceTo(tile)));
            /*if (targetAlly.tile.DistanceTo(tile) < targetDistance2)
            {
                targetDistance2 = targetAlly.tile.DistanceTo(tile);
                //print("Best chosen");
                targetTile = tile;
            }*/
            if (Vector3.Distance(tile.transform.position, targetAlly.tile.transform.position) < targetDistance)
            {
                targetDistance = Vector3.Distance(tile.transform.position, targetAlly.tile.transform.position);
                //print(targetDistance);
                targetTile = tile;
            }
        }

        if (targetTile == null) return;

        //move to tile
        List<Tile> path = unit.move_calc.FindPathTo(targetTile);


        unit.move_calc.MoveToTile(path, unit);
    }

    //
    private void AIPlayCard()
    {
        //find closest player character
        Unit targetAlly = null;
        foreach (Unit ally in TurnScript.instance.PCunits)
        {
            //if they are close enough, attack
            print(Vector3.Distance(unit.tile.transform.position, ally.tile.transform.position));
            if (Vector3.Distance(unit.tile.transform.position, ally.tile.transform.position) <= 2)
            {
                targetAlly = ally;
                unit.attackLerp = 1;
                unit.attack_calc.Damage(2, 2, 0, ally);
                break;
            }
        }
    }
}
