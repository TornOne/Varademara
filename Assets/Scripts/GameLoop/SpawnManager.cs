using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour {
	[System.Serializable]
	public class Wave {
		public EnemyAI[] units;
	}

	public static SpawnManager instance;
	public Wave[] waves;
	int waveCounter = -1;

	void Awake() {
		instance = this;
	}

	public void SpawnUnit(Unit unit, int x, int y) {
		SpawnUnit(unit, Map.instance.GetTile(x, y));
	}

	public void SpawnUnit(Unit unit, Tile tile) {
		unit = Instantiate(unit, tile.transform.position, Quaternion.Euler(45, 0, 0));
		tile.unit = unit;
		unit.tile = tile;
	}

	public void SpawnPlayers(PlayerController player1, PlayerController player2, PlayerController player3) {
		SpawnUnit(player1, 11, 10);
		SpawnUnit(player2, 11, 6);
		SpawnUnit(player3, 11, 2);
	}

	public void SpawnNextWave() {
		waveCounter++;
		if (waveCounter >= waves.Length) {
			//StartCoroutine(VictorySequence());
			return;
		}

		//Find unoccupied edge tiles furthest from everything
		int[] distances = new int[38];
		Tile[] spawnTiles = new Tile[38];
		int[] edgeCoords = new int[76] { 1, 1, 1, 2, 1, 3, 1, 4, 1, 5, 1, 6, 1, 7, 1, 8, 1, 9, 1, 10, 2, 11, 3, 10, 4, 11, 5, 10, 6, 11, 7, 10, 8, 11, 9, 10, 10, 11, 11, 10, 11, 9, 11, 8, 11, 7, 11, 6, 11, 5, 11, 4, 11, 3, 11, 2, 11, 1, 10, 1, 9, 1, 8, 1, 7, 1, 6, 1, 5, 1, 4, 1, 3, 1, 2, 1 };
		for (int i = 0; i < 38; i++) {
			int i2 = i * 2;
			Tile tile = Map.instance.tiles[edgeCoords[i2]][edgeCoords[i2 + 1]];
			spawnTiles[i] = tile;
			int minDistance = int.MaxValue;
			foreach (Unit player in TurnManager.instance.friendlies) {
				minDistance = Mathf.Min(tile.DistanceTo(player.tile), minDistance);
			}
			distances[i] = minDistance;
		}
		System.Array.Sort(distances, spawnTiles);

		//Spawn new enemies to those tiles
		Unit[] units = waves[waveCounter].units;
		for (int i = 0; i < units.Length; i++) {
			SpawnUnit(units[i], spawnTiles[37 - i]);
		}
	}

	//TODO: Temporary spawn? Move to unit selection menu?
	public PlayerController player1, player2, player3;
	void Start() {
		SpawnPlayers(player1, player2, player3);
		StartCoroutine(DelayedStart(3));
    }

	IEnumerator DelayedStart(int frames) {
		for (int i = 0; i < frames; i++) {
			yield return null;
		}
		SpawnNextWave();
	}
}
