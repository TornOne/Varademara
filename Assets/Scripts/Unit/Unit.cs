﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CardManager))]
public abstract class Unit : MonoBehaviour {
	[Header("Unit Attributes")]
	public int maxHP;
	[SerializeField]
	int hp;
	public int HP {
		get {
			return hp;
		}
		set {
			hp = value;
			if (value > maxHP) {
				hp = maxHP;
			}
			hpBar.fillAmount = (float) hp / maxHP;
			if (value <= 0) {
				if (deathAudio != null)
					deathAudio.Play();
				tile.Color = Color.black;
				TurnManager.instance.RemoveUnit(this);
				Destroy(gameObject);
			}
		}
	}
	public int maxAP;
	public int ap;
	public int HandSize {
		get {
			return cardManager.handSize;
		}
		set {
			cardManager.handSize = value;
		}
	}
	public int initiative;
	public int pAtt;
	public int mAtt;
	public int pDef;
	public int mDef;
	public int balance;
	public string title;

	[HideInInspector]
	public bool isAnimating = false;
	[HideInInspector]
	public Tile tile;
	[Header("Component links")]
	public CardManager cardManager;
	public Sprite avatar;

	public AudioClipGroup deathAudio;
	public AudioClipGroup walkAudio;

	protected Color unitColor;
	public GameObject hpBarPrefab;
	Image hpBar;

	void Start() {
		InitializeHPBar();
		TurnManager.instance.AddNewUnit(this);
		tile.Color = unitColor;
	}

	public void StartTurn() {
		tile.Color = Color.yellow;
		ap = maxAP;
		cardManager.StartTurn();
		Activate();
	}

	public void EndTurn() {
		tile.Color = unitColor;
		cardManager.EndTurn();
	}

	protected abstract void Activate();

	public void Move(List<Tile> path) {
		StartCoroutine(LerpMove(path, 0.2f));
		tile.unit = null;
		tile = path[path.Count - 1];
		tile.unit = this;
	}

	IEnumerator LerpMove(List<Tile> path, float duration) {
		isAnimating = true;

		Vector3 origin;
		Vector3 target = path[0].transform.position;
		for (int i = 1; i < path.Count; i++) {
			float startTime = Time.time;
			float currentTime = startTime;
			float endTime = startTime + duration;
			origin = target;
			target = path[i].transform.position;

			path[i].Color = Color.yellow;
			path[i - 1].Color = Color.black;

			while (currentTime < endTime) {
				yield return null;
				currentTime = Time.time;
				transform.position = Vector3.Lerp(origin, target, (currentTime - startTime) / duration);

				if (walkAudio != null)
					walkAudio.Play();
			}
		}

		isAnimating = false;
	}

	public int CalculatePhysicalDamage(int baseDmg, Unit other) {
		return CalculateDamage(baseDmg, pAtt, other.pDef);
	}

	public int CalculateMagicDamage(int baseDmg, Unit other) {
		return CalculateDamage(baseDmg, mAtt, other.mDef);
	}

	public int CalculateTrueDamage(int baseDmg) {
		float b = Mathf.Clamp(balance, 1, 99);
		return (int) (baseDmg * (0.5f + Mathf.Pow(Random.value, balance >= 50 ? (0.02f * (100 - b)) : (50f / b))));
	}

	public int CalculateDamage(int baseDmg, int att, int def) {
		float b = Mathf.Clamp(balance, 1, 99);
		return (int) (baseDmg * (att + 100) * (0.5f + Mathf.Pow(Random.value, balance >= 50 ? (0.02f * (100 - b)) : (50f / b))) / (def + 100));
	}

	void InitializeHPBar() {
		GameObject canvas = Instantiate(hpBarPrefab, transform);
		canvas.transform.localPosition = new Vector3(0, -0.5f);
		hpBar = canvas.GetComponentsInChildren<Image>()[1];
	}
}
