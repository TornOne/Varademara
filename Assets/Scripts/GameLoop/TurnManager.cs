using System;
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

	//Technically backwards, but higher initiative needs to be first
	class TurnComparer : IComparer<Unit> {
		public int Compare(Unit x, Unit y) {
			int diff = y.initiative - x.initiative;
			return diff == 0 ? y.GetInstanceID() - x.GetInstanceID() : diff;
		}
	}

	void Awake() {
		instance = this;
	}

	void Start() {
		SpawnManager.instance.SpawnNextWave();
		StartCoroutine(DelayedStart(5));
	}

	//Some stuff needs more frames to get ready
	IEnumerator DelayedStart(int frames) {
		for (int i = 0; i < frames; i++) {
			yield return null;
		}
		NextTurn();
	}

	//TODO: Currently doesn't support animations
	void UpdateTurnBar() {
		int count = 0;
		void AddImages(IList<Unit> units) {
			foreach (Unit unit in units) {
				//Players face to the left, enemies face to the right
				if (unit is PlayerController) {
					turnBarImages[count].transform.localScale = Vector3.one;
				} else {
					turnBarImages[count].transform.localScale = new Vector3(-1, 1, 1);
				}
				turnBarImages[count].sprite = unit.avatar;
				count++;
				if (count >= 15) {
					return;
				}
			}
		}

		while (count < 15) {
			AddImages(thisTurn.Keys);
			if (count >= 15) {
				return;
			}
			AddImages(nextTurn.Keys);
		}
	}

	public void NextTurn() {
		if (activeUnit != null) { //First turn
			activeUnit.EndTurn();
		}

		//If the current turn has ended, start the next turn
		if (thisTurn.Count == 0) {
			SortedList<Unit, Unit> temp = thisTurn;
			thisTurn = nextTurn;
			nextTurn = temp;
		}

		//Move the unit to the next turn and activate it
		activeUnit = thisTurn.Keys[0];
		thisTurn.RemoveAt(0);
		nextTurn.Add(activeUnit, activeUnit);
		UpdateTurnBar();
		InputManager.instance.sidebar.FillSidebar(activeUnit);
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
			if (enemies.Count == 0) {
				SpawnManager.instance.SpawnNextWave();
			}
		}
	}

}
