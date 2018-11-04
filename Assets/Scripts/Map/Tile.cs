﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
	public int x, y;
	public int difficulty = 1;
	public bool isWall = false;
	public bool isHole = false;
	public Unit unit;
	public List<Tile> neighbors;
	//public List<StatusEffect> effects;

	public bool IsWalkable {
		get {
			return !(isWall || isHole || unit != null);
		}
	}

	public bool IsFlyable {
		get {
			return !isWall && unit == null;
		}
	}

	public int DistanceTo(Tile other) {
		return Map.map.Distance(x, y, other.x, other.y);
	}

}
