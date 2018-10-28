using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyScript : MonoBehaviour {

    private Unit unit;
    private bool turn;
    private List<Tile> possible_tiles;
    // Use this for initialization
    void Start () {
        unit = gameObject.transform.GetComponent<Unit>();
	}

    //move player randomly
    public void PlayerTurn()
    {
        print(unit.tile);
        possible_tiles = unit.move_calc.CalculateMovement(unit.tile, unit.speed);
        //print(possible_tiles.Count);
        //List<Tile> path = unit.move_calc.FindPathTo(possible_tiles[Random.Range(0, possible_tiles.Count)]);
        //print(path.Count);
        //unit.move_calc.MoveToTile(path, unit);
        turn = true;
    }

    private void Update()
    {
        if (turn)
        {
            if (Input.GetMouseButtonUp(0))
            {
                List<Tile> path = unit.move_calc.FindPathTo(Map.map.GetMouseTile());
                if (path.Count <= 0) return;

                unit.move_calc.MoveToTile(path, unit);
                turn = false;
            }
        }
    }
}
