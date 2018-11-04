using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour {
	public static InputManager instance;
	//TODO: Will not accept input during enemy turn, animations, etc.

	Card selectedCard;
	Map map;
	TurnManager turnManager;
	EventSystem eventSystem;

	void Awake() {
		instance = this;
	}

	void Start() {
		map = Map.map;
		turnManager = TurnManager.instance;
		eventSystem = EventSystem.current;
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

		if (Input.GetMouseButtonDown(0) && !eventSystem.IsPointerOverGameObject()) {
			if (selectedCard == null && tile != null && tile.unit != null) {
				//TODO: Show unit info on tile
				return;
			}

			if (tile != null) {
				if (selectedCard.Activate(tile, turnManager.activeUnit)) {
					turnManager.NextTurn(); //TODO: Next turn should begin when explicitly ending turn instead
				}
			}
			DeselectCard();
		}
	}
}
