using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card : MonoBehaviour {
	InputManager inputManager;

	void Start() {
		inputManager = InputManager.instance;
	}

	//Must return whether the activation succeded or not
	public abstract bool Activate(Tile tile, Unit caster);

	void OnMouseDown() {
		inputManager.SelectCard(this);
	}
}
