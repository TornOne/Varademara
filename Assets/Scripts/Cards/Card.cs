using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Card : MonoBehaviour {
	InputManager inputManager;

	void Start() {
		inputManager = InputManager.instance;
	}

	//Must return whether the activation succeded or not
	public abstract bool Activate(Tile tile, Unit caster);

	public void MouseDown() {
		inputManager.SelectCard(this);
	}
}
