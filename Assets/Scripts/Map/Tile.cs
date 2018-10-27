using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
	public int x, y;
	public int difficulty = 1;
	public bool isWall = false;
	public bool isImpassable = false;
	public Unit unit;
	public List<Tile> neighbours;
	//public List<StatusEffect> effects;

	public bool isOccupied {
		get {
			return isWall || isImpassable || unit != null;
		}
	}

	public int DistanceTo(Tile other) {
        if (other.isOccupied) return int.MaxValue;
        return Map.map.Distance(x, y, other.x, other.y);
        //return Map.map.Distance((int)transform.position.x, (int)transform.position.y, (int)other.transform.position.x, (int)other.transform.position.y);
    }

}
