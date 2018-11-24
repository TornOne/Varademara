using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour {
	public static TurnManager instance;

	public SortedSet<Unit> thisTurn = new SortedSet<Unit>(new TurnComparer());
	public SortedSet<Unit> nextTurn = new SortedSet<Unit>(new TurnComparer());
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
		activeUnit.StartTurn();
	}

	//Add or update unit, needs to be called after initiative change
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

		if (unit is PlayerController) {
			friendlies.Add(unit);
		} else {
			enemies.Add(unit);
		}
	}

	public void RemoveUnit(Unit unit) {
		if (nextTurn.Contains(unit)) {
			nextTurn.Remove(unit);
		} else if (thisTurn.Contains(unit)) {
			thisTurn.Remove(unit);
		}

		if (unit is PlayerController) {
			friendlies.Remove(unit);
		} else {
			enemies.Remove(unit);
		}
	}

}
