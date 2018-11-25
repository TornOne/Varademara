﻿using UnityEngine;
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
		map = Map.instance;
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
			if (selectedCard == null) {
				if (tile != null && tile.unit != null) {
                    GameObject.Find("Sidebar").GetComponent<SidebarInformation>().fillSidebar(tile.unit);
					return;
				}
				return; //No card selected, nothing to do
			}

			if (tile != null) {
				//Try to use card
				//Find the reference to the real card in the hand
				selectedCard = turnManager.activeUnit.cardManager.hand[selectedCard.canvas.sortingOrder];
				selectedCard.Use(tile, turnManager.activeUnit);
			}
			DeselectCard();
		}
	}
}
