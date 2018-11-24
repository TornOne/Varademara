using System.Collections.Generic;
using UnityEngine;

//TODO: Removing cards
public class HandManager : MonoBehaviour {
	public static HandManager instance;

	List<Card> cardObjects = new List<Card>();

	void Awake() {
		instance = this;
	}

	public void AddCards(List<Card> cards) {
		for (int i = 0; i < cards.Count; i++) {
			Card card = Instantiate(cards[i], new Vector3(0.5f * (1 - cards.Count) + i, 0), Quaternion.identity, transform);
			card.canvas.sortingOrder = i;
			cardObjects.Add(card);
		}
	}

	public void EmptyHand() {
		foreach (Card card in cardObjects) {
			Destroy(card.gameObject);
		}
		cardObjects.Clear();
	}

	public void ReplaceCards(List<Card> cards) {
		EmptyHand();
		AddCards(cards);
	}
}
