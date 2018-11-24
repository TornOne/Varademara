using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour {
	public static TurnManager instance;

	public List<Image> turnBarImages;
	public SortedList<Unit, Unit> thisTurn = new SortedList<Unit, Unit>(new TurnComparer()); //Sorted lists for some reason want both a key and a value so units are just duplicated
	public SortedList<Unit, Unit> nextTurn = new SortedList<Unit, Unit>(new TurnComparer()); //to both key and value. (The value may later be changed for something more useful.)
	public HashSet<Unit> friendlies = new HashSet<Unit>();
	public HashSet<Unit> enemies = new HashSet<Unit>();
	public Unit activeUnit;

	class TurnComparer : IComparer<Unit> {
		public int Compare(Unit x, Unit y) {
			int diff = x.initiative - y.initiative;
			return diff == 0 ? x.GetInstanceID() - y.GetInstanceID() : diff;
		}
	}

	void Awake() {
		instance = this;
	}

	void Start() {
		StartCoroutine(DelayedStart(15));
	}

	//Some stuff needs more frames to get ready
	IEnumerator DelayedStart(int frames) {
		for (int i = 0; i < frames; i++) {
			yield return null;
		}
		NextTurn();
	}

	//TODO: Currently very inefficient turn order bar, and doesn't support animations
	void UpdateTurnBar() {
		int count = 0;
		while (true) {
			foreach (Unit unit in thisTurn.Keys) {
				turnBarImages[count].sprite = unit.avatar;
				count++;
				if (count >= 15) {
					return;
				}
			}
			foreach (Unit unit in nextTurn.Keys) {
				turnBarImages[count].sprite = unit.avatar;
				count++;
				if (count >= 15) {
					return;
				}
			}
		}
	}

	public void NextTurn() {
		//If the current turn has ended, start the next turn
		if (thisTurn.Count == 0) {
			SortedList<Unit, Unit> temp = thisTurn;
			thisTurn = nextTurn;
			nextTurn = temp;
		}
		//Move the unit to the next turn and activate it
		activeUnit = thisTurn.Keys[thisTurn.Count - 1];
		thisTurn.RemoveAt(thisTurn.Count - 1);
		nextTurn.Add(activeUnit, activeUnit);
		UpdateTurnBar();
		activeUnit.StartTurn();
	}

	//Add or update unit, needs to be called after initiative change
	public void AddNewUnit(Unit unit) {
		if (nextTurn.ContainsKey(unit)) {
			nextTurn.Remove(unit);
			nextTurn.Add(unit, unit);
		} else {
			if (thisTurn.ContainsKey(unit)) {
				thisTurn.Remove(unit);
			}
			thisTurn.Add(unit, unit);
		}
		UpdateTurnBar();

		if (unit is PlayerController) {
			friendlies.Add(unit);
		} else {
			enemies.Add(unit);
		}
	}

	public void RemoveUnit(Unit unit) {
		if (nextTurn.ContainsKey(unit)) {
			nextTurn.Remove(unit);
		} else if (thisTurn.ContainsKey(unit)) {
			thisTurn.Remove(unit);
		}
		UpdateTurnBar();

		if (unit is PlayerController) {
			friendlies.Remove(unit);
		} else {
			enemies.Remove(unit);
		}
	}

}
