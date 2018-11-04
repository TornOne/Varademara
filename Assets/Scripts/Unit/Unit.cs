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

	public int ap;  //action points
	public int initiative;  //turn order influencer

	public int pAtt;
	public int mAtt;
	public int pDef;    //physical resistance
	public int mDef;    //magical resistance
	public int balance; //damage modifier

	public DamageScript attack_calc;
	public MovementScript move_calc;

	public float moveAnimationSpeed;
	private bool animationFinished = false;

	private Vector3 start;
	private List<Vector3> end;
	private float moveLerp;
	public float attackLerp;

	public Tile tile; //Useful
	public List<Tile> path;

	void Start() {
		TurnManager.instance.AddNewUnit(this);
	}

	public abstract void Activate();

	//TODO: Replace with Tile List (walk through all)
	public void MoveTo(Tile targetTile) {
		StartCoroutine(LerpMove(tile.transform.position, targetTile.transform.position, 0.2f));
		tile.unit = null;
		tile = targetTile;
		tile.unit = this;
	}

	IEnumerator LerpMove(Vector3 origin, Vector3 target, float duration) {
		float startTime = Time.time;
		float currentTime = startTime;
		float endTime = startTime + duration;

		while (currentTime < endTime) {
			yield return null;
			currentTime = Time.time;
			transform.position = Vector3.Lerp(origin, target, (currentTime - startTime) / duration);
		}
	}





	/*
	// Calculate unit turn weight for turn order
	public int TurnWeight()
	{
		return initiative + 2 * idle;
	}

	// Deal damage to this unit
	public void Damage(int dmg)
	{
		hp -= dmg;
		if (hp <= 0) Destroy(gameObject);
	}

	public void Move(List<Tile> targetTile)
	{
		moveLerp = targetTile.Count;
		path = targetTile;
		tile.unit = null;

		path.Add(tile);
		tile = path[0];
		tile.unit = this;

		animationFinished = true;
	}

	private void Update()
	{
		if (moveLerp > 0)
		{
			moveLerp -= Time.deltaTime * moveAnimationSpeed;
			transform.position = Vector3.Lerp(path[(int)moveLerp + 1].transform.position, path[(int)moveLerp].transform.position, Mathf.Min((1f - moveLerp % 1) * 1.5f, 1f));
		//} else if (attackLerp > 0){
		//    attackLerp -= Time.deltaTime * moveAnimationSpeed;

		} else if (animationFinished){
			

			TurnScript.instance.EndTurn();
			animationFinished = false;
		}
	}

	void OnDrawGizmos()
	{
		if (path == null) return;
		Gizmos.color = Color.yellow;
		for (int i = 1; i < path.Count; i++)
		{
			Gizmos.DrawLine(path[i-1].transform.position, path[i].transform.position);
		}
	}
	*/

}
