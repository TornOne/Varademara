using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
	public static InputManager instance;
	//TODO: Will not accept input during enemy turn, animations, etc.

	Card selectedCard;
	Map map;
	TurnManager turnManager;

	void Awake() {
		instance = this;
	}

	void Start() {
		map = Map.map;
		turnManager = TurnManager.instance;
	}

	public void SelectCard(Card card) {
		if (selectedCard != null) {
			DeselectCard();
		}
		selectedCard = card;
		//TODO: Highlight the card or something
	}

	public void DeselectCard() {
		selectedCard = null;
	}

	void Update() {
		Tile tile = map.GetMouseTile();
		//TODO: Highlight tile

		if (Input.GetMouseButtonDown(0)) {
			if (tile != null) {
				selectedCard.Activate(tile, turnManager.activeUnit);
			}
			DeselectCard();
		}
	}
}
