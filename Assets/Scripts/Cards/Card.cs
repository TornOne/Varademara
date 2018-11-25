using UnityEngine;

public abstract class Card : MonoBehaviour {
	InputManager inputManager;
	public Canvas canvas;
	public RectTransform rectTransform;
	public bool deleteOnUse = false;
	public int apCost;

	void Awake() {
		rectTransform = GetComponent<RectTransform>();
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

	public bool UseNoUI(Tile tile, Unit caster) {
		if (caster.ap < apCost) {
			return false;
		} else if (Activate(tile, caster)) {
			caster.ap -= apCost;
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

	/* -------------------------------------------------- */

	public virtual int CardValue(Tile tile, EnemyAI caster, Object target, ref Object extra) {
		return 0;
	}
}
