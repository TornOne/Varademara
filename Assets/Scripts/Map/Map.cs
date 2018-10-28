using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {
	public static Map map;

	Camera cam;

	public int width, height;
	public Tile tile;
	public Tile[][] tiles;

    public Transform debugCube;


    public Tile GetTile(int x, int y) {
		int w1 = width - 1;
		if (y < height && y >= 0 && x >= 0 && (x < w1 || x == w1 && y % 2 == 0)) {
			return tiles[y][x];
		} else {
			return null;
		}
	}

	void Awake() {
		map = this;
		GenerateMap();
	}

	void Start() {
		cam = Camera.main;
	}

    void Update()
    {
        //print(GetMouseTile());
        //debugCube.position = GetMouseTile().transform.position;
    }

	public Tile GetMouseTile() {
		Ray ray = cam.ScreenPointToRay(Input.mousePosition);
		Vector3 cursorPos = ray.origin + -ray.origin.y / ray.direction.y * ray.direction;
		float y3 = cursorPos.z * 0.57735f;
		int row = Mathf.RoundToInt(2 * y3);
		int col = Mathf.RoundToInt(cursorPos.x - y3) + row / 2;
		return GetTile(col, row);
	}

	public int Distance(int x1, int y1, int x2, int y2) {
		int dy = Mathf.Abs(y2 - y1);
		return Mathf.Max(dy, Mathf.Abs(x2 - x1) + dy / 2 + ((y1 ^ y2) & (y1 ^ (x1 < x2 ? 1 : 0)) & 1));
        //return (int)Mathf.Pow(Mathf.Pow(x1-y1, 2)+ Mathf.Pow(x2-y2, 2), 0.5f);
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
