using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyScript : MonoBehaviour {

    private UnitScript unit;
    // Use this for initialization
    void Start () {
        unit = gameObject.transform.GetComponent<UnitScript>();
	}

    //move player randomly
    public void PlayerTurn()
    {
        List<MovementScript.TilePlaceholder> possible_tiles = unit.move_calc.CalculateMovement(unit.tile, unit.mP);
        List<MovementScript.TilePlaceholder> path = unit.move_calc.FindPathTo(possible_tiles[Random.Range(0, possible_tiles.Count)]);
        unit.move_calc.MoveToTile(path, unit);
    }
}
