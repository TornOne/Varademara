using UnityEngine;

public abstract class Card : MonoBehaviour {
	InputManager inputManager;
	public Canvas canvas;
	public bool deleteOnUse = false;
	public int apCost;

	void Start() {
		canvas = GetComponent<Canvas>();
		inputManager = InputManager.instance;
	}

	public bool Use(Tile tile, Unit caster) {
		if (caster.ap < apCost) {
			return false;
		} else if (Activate(tile, caster)) {
			caster.ap -= apCost;
			caster.cardManager.Discard(this);
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
}
