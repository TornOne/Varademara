using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
	Map map;

	public int x, y;
	public int difficulty = 1;
	public bool isWall = false;
	public bool isHole = false;
	public Unit unit;
	public List<Tile> neighbors;
    //public List<StatusEffect> effects;

    public SpriteRenderer sprite;
    public Color baseColor = new Color(0,0,0);

    public void SetMovableHighlight(Color value)
    {
        if (value.a == 0)
        {
            if (unit != null)
            {
                Color unitColor = unit.GetMyColor();
                if (unitColor.a == 0) sprite.color = baseColor;
                else sprite.color = unitColor;
            }
            else sprite.color = baseColor;
        }
        else
        {
            //baseColor = sprite.color;
            sprite.color = value;
        }
    }

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

	void Start() {
		map = Map.instance;
	}

	public int LineOfSightTo(Tile other) {
		return map.LineOfSight(this, other);
	}

	public int DistanceTo(Tile other) {
		return map.Distance(x, y, other.x, other.y);
	}

	public Dictionary<Tile, int> BuildWalkMap(int distance) {
		return map.BuildWalkMap(this, distance);
	}

	public Dictionary<Tile, int> BuildFlightMap(int distance) {
		return map.BuildFlightMap(this, distance);
	}

	public int TraverseDistanceTo(Tile tile) {
		return map.TraverseDistanceTo(tile);
	}

	public List<Tile> PathTo(Tile tile) {
		return map.PathTo(tile);
	}
}
