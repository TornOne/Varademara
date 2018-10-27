using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyScript : MonoBehaviour {

    private Unit unit;
    // Use this for initialization
    void Start () {
        unit = gameObject.transform.GetComponent<Unit>();
	}

    //move player randomly
    public void PlayerTurn()
    {
        List<Tile> possible_tiles = unit.move_calc.CalculateMovement(unit.tile, unit.speed);
        //print(possible_tiles.Count);
        List<Tile> path = unit.move_calc.FindPathTo(possible_tiles[Random.Range(0, possible_tiles.Count)]);
        print(path.Count);
        unit.move_calc.MoveToTile(path, unit);
    }
}
