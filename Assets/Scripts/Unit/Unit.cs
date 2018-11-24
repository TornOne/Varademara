using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CardManager))]
public abstract class Unit : MonoBehaviour {
	[SerializeField]
	int hp;
	public int HP { //HACK: This entire property
		get {
			return hp;
		} set {
			hp = value;
			if (value <= 0) {
				TurnManager.instance.RemoveUnit(this);
				Destroy(gameObject);
			}
		}
	}

	public int maxAP;
	public int ap;
	public int initiative;
	public int pAtt;
	public int mAtt;
	public int pDef;
	public int mDef;
	public int balance;

	public DamageScript attack_calc;
	public MovementScript move_calc;

	public Tile tile;
	public CardManager cardManager;

	void Start() {
		TurnManager.instance.AddNewUnit(this);
	}

	public void StartTurn() {
		ap = maxAP;
		Activate();
	}

	protected abstract void Activate();

	public void Move(List<Tile> path) {
		StartCoroutine(LerpMove(path, 0.2f));
		tile.unit = null;
		tile = path[path.Count - 1];
		tile.unit = this;
	}

	IEnumerator LerpMove(List<Tile> path, float duration) {
		Vector3 origin;
		Vector3 target = path[0].transform.position;
		for (int i = 1; i < path.Count; i++) {
			float startTime = Time.time;
			float currentTime = startTime;
			float endTime = startTime + duration;
			origin = target;
			target = path[i].transform.position;

			while (currentTime < endTime) {
				yield return null;
				currentTime = Time.time;
				transform.position = Vector3.Lerp(origin, target, (currentTime - startTime) / duration);
			}
		}
	}

}
