using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public abstract class Card : MonoBehaviour {
	InputManager inputManager;
	public Canvas canvas;
	public RectTransform rectTransform;
	public bool deleteOnUse = false;
	public int apCost;

	protected Dictionary<Tile, int> tiles;
	public Vector3 handPos;

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

	public bool Select(Unit caster, bool select) {
		if (caster.ap < apCost) {
			return false;
		} else if (PreActivate(caster, select)) {
			return true;
		} else {
			return false;
		}
	}

	public IEnumerator MoveWithMouse() {
		handPos = transform.localPosition;
		Camera cam = Camera.main;
		float camHeightDeci = cam.pixelHeight / 10f;
		while (true) {
			transform.position = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y - camHeightDeci, 10));
			yield return null;
		}
	}

	//Cards that should do precalculations before activating
	protected abstract bool PreActivate(Unit caster, bool select);

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
