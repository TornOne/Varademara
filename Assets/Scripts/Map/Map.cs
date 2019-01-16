using System;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {
	public static Map instance;

	Camera cam;
	Dictionary<Tile, int> distanceMap;
	Dictionary<Tile, Tile> backtrackMap;
	List<Tile> litTiles = new List<Tile>();

	public int width, height;
	public Tile tile;
	public Tile[][] tiles;

	public Tile GetTile(int x, int y) {
		int w1 = width - 1;
		if (y < height && y >= 0 && x >= 0 && (x < w1 || x == w1 && y % 2 == 0)) {
			return tiles[y][x];
		} else {
			return null;
		}
	}

	void Awake() {
		instance = this;
		GenerateMap();
	}

	void Start() {
		cam = Camera.main;
	}

	public Tile WorldPosToTile(float x, float y) {
		float yDivSqrt3 = y * 0.57735f;
		int row = Mathf.RoundToInt(2 * yDivSqrt3);
		int col = Mathf.RoundToInt(x - yDivSqrt3) + row / 2;
		return GetTile(col, row);
	}

	//Gets the tile under the mouse cursor
	public Tile GetMouseTile() {
		Ray ray = cam.ScreenPointToRay(Input.mousePosition);
		Vector3 cursorPos = ray.origin + -ray.origin.y / ray.direction.y * ray.direction;
		return WorldPosToTile(cursorPos.x, cursorPos.z);
	}

	//-1 = No LoS; n = Number of units
	public int LineOfSight(Tile origin, Tile destination) {
		int distance = Distance(origin.x, origin.y, destination.x, destination.y);
		float dx = (destination.transform.position.x - origin.transform.position.x) / distance;
		float dy = (destination.transform.position.y - origin.transform.position.y) / distance;
		float x = origin.transform.position.x + dx;
		float y = origin.transform.position.y + dy;

		int unitCount = 0;
		for (int i = 1; i < distance; x += dx, y += dy) {
			Tile tile = WorldPosToTile(x, y);
			if (tile.isWall) {
				return -1;
			}
			if (tile.unit != null) {
				unitCount++;
			}
		}
		return unitCount;
	}

	public void HighlightTiles<T>(T tiles) where T : IEnumerable<Tile> {
		//Dehighlight old tiles
		foreach (Tile tile in litTiles) {
			tile.HighLit = false;
		}
		//Light new tiles
		litTiles = new List<Tile>(tiles);
		foreach (Tile tile in tiles) {
			tile.HighLit = true;
		}
	}

	public int Distance(int x1, int y1, int x2, int y2) {
		int dy = Mathf.Abs(y2 - y1);
		return Mathf.Max(dy, Mathf.Abs(x2 - x1) + dy / 2 + ((y1 ^ y2) & (y1 ^ (x1 < x2 ? 1 : 0)) & 1));
	}

	//Must be called before other pathfinding functions
	//Returns a collection of all applicable tiles
	public Dictionary<Tile, int> BuildWalkMap(Tile origin, int maxDistance) {
		return BuildTraverseMap(origin, maxDistance, (Tile tile) => tile.IsWalkable);
	}

	public Dictionary<Tile, int> BuildFlightMap(Tile origin, int maxDistance) {
		return BuildTraverseMap(origin, maxDistance, (Tile tile) => tile.IsFlyable);
	}

	Dictionary<Tile, int> BuildTraverseMap(Tile origin, int maxDistance, Func<Tile, bool> isTraversable) {
		HashSet<Tile> visited = new HashSet<Tile>();
		SortedSet<Tile> unvisited = new SortedSet<Tile>(new TileComparer());
		distanceMap = new Dictionary<Tile, int>();
		backtrackMap = new Dictionary<Tile, Tile>();
		distanceMap[origin] = 0;
		unvisited.Add(origin);

		while (unvisited.Count != 0) {
			Tile current = unvisited.Min;
			unvisited.Remove(current);
			foreach (Tile tile in current.neighbors) {
				//Don't check tiles that can't be traversed or are visited
				if (!isTraversable(tile) || visited.Contains(tile)) {
					continue;
				}
				//Don't check tiles that would be too far
				int newDistance = distanceMap[current] + tile.difficulty;
				if (newDistance > maxDistance) {
					continue;
				}
				//Replace other tile distance values with the new shortest
				bool exists = distanceMap.TryGetValue(tile, out int prevDistance);
				if (!exists) {
					backtrackMap[tile] = current;
					distanceMap[tile] = newDistance;
					unvisited.Add(tile);
				} else if (newDistance < prevDistance) {
					unvisited.Remove(tile);
					backtrackMap[tile] = current;
					distanceMap[tile] = newDistance;
					unvisited.Add(tile);
				}
			}
			visited.Add(current);
		}

		return distanceMap;
	}

	class TileComparer : IComparer<Tile> {
		public int Compare(Tile a, Tile b) {
			int diff = instance.distanceMap[a] - instance.distanceMap[b];
			return diff == 0 ? a.GetInstanceID() - b.GetInstanceID() : diff;
		}
	}

	public int TraverseDistanceTo(Tile destination) {
		return distanceMap[destination];
	}

	public List<Tile> PathTo(Tile destination) {
		List<Tile> path = new List<Tile>();
		if (destination == null) {
			return path;
		}
		path.Add(destination);

		while (backtrackMap.TryGetValue(destination, out destination)) {
			path.Add(destination);
		}
		path.Reverse();
		return path;
	}

	void GenerateMap() {
		tiles = new Tile[height][];

		for (int h = 0; h < height; h++) {
			Tile[] row;

			if (h % 2 == 0) { //Full row
				row = new Tile[width];

				for (int w = 0; w < width; w++) {
					Tile t = Instantiate(tile, new Vector3(w, 0, 0.5f * Mathf.Sqrt(3) * h), Quaternion.Euler(90, 0, 0), transform);
					t.x = w;
					t.y = h;

					#region Neighbors
					Tile t2;
					if (w != 0) { //Add left and right
						t2 = row[w - 1];
						t.neighbors.Add(t2);
						t2.neighbors.Add(t);
					}
					if (h != 0) { //Add bottom and top...
						if (w != 0) { //...left and right
							t2 = tiles[h - 1][w - 1];
							t.neighbors.Add(t2);
							t2.neighbors.Add(t);
						}
						if (w != width - 1) { //...right and left
							t2 = tiles[h - 1][w];
							t.neighbors.Add(t2);
							t2.neighbors.Add(t);
						}
					}
					#endregion

					row[w] = t;
				}
			} else { //Short row
				int rowWidth = width - 1;
				row = new Tile[rowWidth];

				for (int w = 0; w < rowWidth; w++) {
					Tile t = Instantiate(tile, new Vector3(w + 0.5f, 0, 0.5f * Mathf.Sqrt(3) * h), Quaternion.Euler(90, 0, 0), transform);
					t.x = w;
					t.y = h;

					#region Neighbors
					Tile t2;
					if (w != 0) { //Add left and right
						t2 = row[w - 1];
						t.neighbors.Add(t2);
						t2.neighbors.Add(t);
					}
					t2 = tiles[h - 1][w]; //Add bottom left and top right
					t.neighbors.Add(t2); 
					t2.neighbors.Add(t);
					t2 = tiles[h - 1][w + 1];
					t.neighbors.Add(t2); //Add bottom right and top left
					t2.neighbors.Add(t);
					#endregion

					row[w] = t;
				}
			}

			tiles[h] = row;
		}
	}
}
