using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

//TODO: Will not accept input during enemy turn, animations, etc.
public class InputManager : MonoBehaviour {
	public static InputManager instance;

	public SidebarInformation sidebar;
	Card selectedCard;
	Map map;
	TurnManager turnManager;
	EventSystem eventSystem;

    public TileHighlighter tileHighlighter;

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

        selectedCard = turnManager.activeUnit.cardManager.hand[selectedCard.canvas.sortingOrder];
        selectedCard.Select(turnManager.activeUnit, true);
        //TODO: Highlight the card or something
    }

	public void DeselectCard() {
        selectedCard.Select(turnManager.activeUnit, false);
        selectedCard = null;
	}

	public void DiscardClicked() {
		if (selectedCard != null) {
			CardManager cardManager = turnManager.activeUnit.cardManager;
			selectedCard = cardManager.hand[selectedCard.canvas.sortingOrder];
			cardManager.Discard(selectedCard);
		}
	}

	void Update() {
		Tile tile = map.GetMouseTile();
        //TODO: Highlight tile


        tileHighlighter.setPos(tile);


        if (Input.GetMouseButtonDown(0) && !eventSystem.IsPointerOverGameObject()) {
			if (selectedCard == null) {
				if (tile != null && tile.unit != null) {
					sidebar.FillSidebar(tile.unit);
					return;
				}
				return; //No card selected, nothing to do
			}

			if (tile != null && selectedCard != null) {
                //Try to use card
                //Find the reference to the real card in the hand

                //selectedCard = turnManager.activeUnit.cardManager.hand[selectedCard.canvas.sortingOrder];
                selectedCard.Select(turnManager.activeUnit, false); // double call for fix
                selectedCard.Use(tile, turnManager.activeUnit);
			}


			DeselectCard();
		}

		if (Input.GetKeyDown(KeyCode.Escape)) {
			SceneManager.LoadScene("StartMenu");
		}
	}
}
