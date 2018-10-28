using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour {
	public static TurnManager instance;

	public SortedSet<Unit> thisTurn = new SortedSet<Unit>(new TurnComparer());
	public SortedSet<Unit> nextTurn = new SortedSet<Unit>(new TurnComparer());
	public Unit activeUnit;

	class TurnComparer : IComparer<Unit> {
		public int Compare(Unit x, Unit y) {
			return x.initiative - y.initiative; //TODO: Might be backwards
		}
	}

	void Awake() {
		instance = this;
	}

	void Start() {
		StartCoroutine(DelayedStart(15));
        MovementScript.BuildPathGraph();
    }

	IEnumerator DelayedStart(int frames) {
		for (int i = 0; i < frames; i++) {
			yield return null;
		}
		NextTurn();
	}

	public void NextTurn() {
		//If the current turn has ended, start the next turn
		if (thisTurn.Count == 0) {
			SortedSet<Unit> temp = thisTurn;
			thisTurn = nextTurn;
			nextTurn = temp;
		}
		//Move the unit to the next turn and activate it
		activeUnit = thisTurn.Max;
		thisTurn.Remove(activeUnit);
		nextTurn.Add(activeUnit);
		activeUnit.Activate();
	}

	//Add or update unit
	public void AddNewUnit(Unit unit) {
		if (nextTurn.Contains(unit)) {
			nextTurn.Remove(unit);
			nextTurn.Add(unit);
		} else {
			if (thisTurn.Contains(unit)) {
				thisTurn.Remove(unit);
			}
			thisTurn.Add(unit);
		}
	}

	// increment turn idle time and send first unit to its new turn order spot
	/*public void NextTurn() {
		units.RemoveAt(0);

		foreach (Unit u in units)
			u.idle++;

		AddUnitOrdered(activeUnit);

		activeUnit = units[0];
	}

	// Add unit to turn order based on its idle time and initiative
	private void AddUnitOrdered(Unit unit) {
		unit.idle = 1;
		int turnWeight = unit.TurnWeight();
		for (int i = units.Count - 1; i >= 0; i--) {
			if (units[i].TurnWeight() >= turnWeight) {
				units.Insert(i + 1, unit);
				return;
			}
		}
		units.Insert(0, unit);
	}

	//TODO: init actual tilemap
	public Map tilemap;

	// Initialize turn order and find the current active unit
	void Start() {
		// Turn order and movement testing
		//tilemap = new Map(20,20);

		//built tile graph
		MovementScript.BuildPathGraph(tilemap);


		List<Unit> temporary = new List<Unit>(units);
		units.Clear();
		foreach (Unit unit in temporary) {
			AddUnitOrdered(unit);
			unit.tile = tilemap.tiles[(int) unit.transform.position.y][(int) unit.transform.position.x];
			tilemap.tiles[(int) unit.transform.position.y][(int) unit.transform.position.x].unit = unit;
			//add all non AI controlled units to a player list, this will be the target for enemy units
			if (unit.GetComponent<EnemyScript>() == null)
				PCunits.Add(unit);
		}
		activeUnit_ = units[0];






	}

	// Turn order and movement testing
	void Update() {
		//if (gametime < Time.fixedTime - 2f)
		if (turnPassover) {
			turnPassover = false;
			/*List<MovementScript.TilePlaceholder> possible_tiles = activeUnit.move_calc.CalculateMovement(tilemap, activeUnit.tile, activeUnit.mP);
			List<MovementScript.TilePlaceholder> path = activeUnit.move_calc.FindPathTo(tilemap, possible_tiles[Random.Range(0, possible_tiles.Count)]);
			activeUnit.move_calc.MoveToTile(path, activeUnit);*/
            /*
			if (activeUnit_.GetComponent<EnemyScript>() != null)
				activeUnit_.GetComponent<EnemyScript>().AITurn();
			else
				activeUnit_.GetComponent<AllyScript>().PlayerTurn();

			Debug.Log("--------");

			//NextTurn();
			//gametime = Time.fixedTime;
		}
	}

	public void EndTurn() {
		NextTurn();
		turnPassover = true;
	}*/

}
