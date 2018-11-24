using UnityEngine;

public abstract class Card : MonoBehaviour {
	InputManager inputManager;
	public bool deleteOnUse = false;
	public int apCost;

	void Start() {
		inputManager = InputManager.instance;
	}

	public bool Use(Tile tile, Unit caster) {
		if (caster.ap < apCost) {
			return false;
		} else if (Activate(tile, caster)) {
			caster.ap -= apCost;
			Discard(caster);
			return true;
		} else {
			return false;
		}
	}

	//Must return whether the activation succeded or not
	protected abstract bool Activate(Tile tile, Unit caster);

	public void MouseDown() {
		inputManager.SelectCard(this);
	}

	public void Discard(Unit caster) {
		if (deleteOnUse) {
			//TODO: Remove from hand and delete
		} else {
			//TODO: Transfer the card to the discard pile
		}
	}
}
