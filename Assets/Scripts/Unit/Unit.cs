using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour {
	[SerializeField]
	int hp;  //health points
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

	public int ap;			//action points
	public int initiative;	//turn order influencer
	public int pAtt;		//physical damage
	public int mAtt;		//magical damage
	public int pDef;		//physical resistance
	public int mDef;		//magical resistance
	public int balance;		//damage modifier

	public DamageScript attack_calc;
	public MovementScript move_calc;

	public Tile tile;

	void Start() {
		TurnManager.instance.AddNewUnit(this);
	}

	public abstract void Activate();

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
