using System.Collections.Generic;
using UnityEngine;

//TODO: Needs to be hooked up to the UI
public class CardManager : MonoBehaviour {
	public int handSize;
	public List<Card> hand = new List<Card>();
	public List<Card> deck = new List<Card>(); //End of the list is the top of the deck
	public List<Card> discard = new List<Card>(); //End of the list is the top of the discard pile
	public Unit owner;

	//Shuffle the deck
	public void Shuffle() {
		for (int i = 0; i < deck.Count; i++) {
			Card temp = deck[i];
			int j = Random.Range(i, deck.Count);
			deck[i] = deck[j];
			deck[j] = temp;
		}
	}

	//Shuffle the discard into the deck
	public void Reshuffle() {
		deck.AddRange(discard);
		Shuffle();
	}

	public void FillHand() {
		while (hand.Count < handSize) {
			DrawCard();
		}
	}

	public void DrawCard() {
		if (deck.Count == 0) {
			Reshuffle();
		}
		hand.Add(Pop());
	}

	public Card Peek() {
		return deck[deck.Count - 1];
	}

	public Card Pop() {
		int i = deck.Count - 1;
		Card card = deck[i];
		deck.RemoveAt(i);
		return card;
	}
}
