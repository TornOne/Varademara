using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {


    private UnitScript unit;
	// Use this for initialization
	void Start () {
        unit = gameObject.transform.GetComponent<UnitScript>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AITurn()
    {
        AIMove();
        AIPlayCard();
    }

    private void AIMove()
    {
        List<MovementScript.TilePlaceholder> possible_tiles = unit.move_calc.CalculateMovement(unit.tile, unit.mP);


        UnitScript targetAlly = null;
        float targetDistance = float.PositiveInfinity;

        foreach (UnitScript ally in TurnScript.instance.PCunits)
        {
            if (Vector3.Distance(unit.tile.Position(), ally.tile.Position()) < targetDistance)
            {
                targetDistance = Vector3.Distance(unit.tile.Position(), ally.transform.position);
                targetAlly = ally;
            }
        }

        MovementScript.TilePlaceholder targetTile = null;
        targetDistance = float.PositiveInfinity;

        foreach (MovementScript.TilePlaceholder tile in possible_tiles)
        {
            if (Vector3.Distance(tile.Position(), targetAlly.tile.Position()) < targetDistance)
            {
                targetDistance = Vector3.Distance(tile.Position(), targetAlly.tile.Position());
                targetTile = tile;
            }
        }

        List<MovementScript.TilePlaceholder> path = unit.move_calc.FindPathTo(targetTile);


        unit.move_calc.MoveToTile(path, unit);
    }

    private void AIPlayCard()
    {

    }
}
