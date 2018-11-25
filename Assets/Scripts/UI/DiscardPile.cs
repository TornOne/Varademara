using UnityEngine;

public class DiscardPile : MonoBehaviour {
	public static DiscardPile instance;
	Camera cam;

	Card topCard;

	void Awake() {
		instance = this;
	}

	void Start() {
		cam = Camera.main;
	}

	public void AddCard(Card card) {
		topCard = Instantiate(card, cam.transform);
		topCard.transform.position = transform.position;
		topCard.enabled = false;
	}

	public void RemoveCard() {
		if (topCard != null) {
			Destroy(topCard.gameObject);
			topCard = null;
		}
	}

	public void ReplaceCard(Card card) {
		RemoveCard();
		AddCard(card);
	}
}
