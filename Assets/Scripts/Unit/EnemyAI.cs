using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyAI : Unit {
	void Awake() {
		unitColor = Color.red;
	}

	//TODO: aggro system, unit who caused most damage becomes main aggor target
	//will need to keep track aggro dmg dealt by each opponent
	private Unit aggroTarget;

    //TODO: AI movement, best distance for the ai to be from its target
    public int optimalDistane;

    protected override void Activate() {
        //TODO: Add AI (current thing is some weird hack, don't look at it)


        //TODO: implement aggro system instead of next friendly
		HashSet<Unit>.Enumerator friendlies = TurnManager.instance.friendlies.GetEnumerator();
		friendlies.MoveNext();

		aggroTarget = friendlies.Current;

        //cardManager.hand.Add(new MoveCard());//TODO: ai needs cards


        /*
		if (tile.DistanceTo(target.tile) == 1) {
			target.HP--;
		} else {
			//Find adjacent tile that's nearest to target
			Tile nearestTile = tile;
			foreach (Tile adjacentTile in tile.neighbors) {
				if (adjacentTile.IsWalkable && adjacentTile.DistanceTo(target.tile) < nearestTile.DistanceTo(target.tile)) {
					nearestTile = adjacentTile;
				}
			}
			Move(new List<Tile>() { tile, nearestTile });
		}
        */
        StartCoroutine("AIStateMachine");

    }

    IEnumerator AIStateMachine()
    {

        while (ap > 0)
        {
            if (!CalculateHandValuesMove()) break;
            while (isAnimating) yield return null;
        }

        while (ap > 0)
        {
            if (!CalculateHandValuesAction()) break;
            while (isAnimating) yield return null;
        }


        TurnManager.instance.NextTurn();
        yield return null;
    }

    private bool CalculateHandValuesAction()
    {


        int[] cardValues = new int[cardManager.hand.Count];
        for (int i = 0; i < cardManager.hand.Count; i++)
        {
            //cardValues[i] = int.MaxValue;
            if (cardManager.hand[i] == null) continue;
            if (cardManager.hand[i].apCost > ap) continue;

            if (cardManager.hand[i] is AttackCard)
            {
                //Attack/debuff cards
                cardValues[i] = CardValueToTarget(cardManager.hand[i], aggroTarget);
            }
            /*if (cardManager.hand[i] is DefendCard)
            {
                //Buff/heal cards
                cardValues[i] = cardValueToTarget(cardManager.hand[i], aggroTarget);
            }*/
            if (cardManager.hand[i] is DrawCard)
            {
                //Card draw
                cardValues[i] = CardValueToTarget(cardManager.hand[i], this);
            }
        }


        int playCardIdx = cardValues.ToList().IndexOf(cardValues.Max());
        if (cardValues[playCardIdx] == 0) return false;

        //ap -= cardManager.hand[playCardIdx].apCost;

        cardManager.hand[playCardIdx].Use(aggroTarget.tile, this);

        return true;
    }

    private bool CalculateHandValuesMove()
    {

        int[] cardValues = new int[cardManager.hand.Count];
        Tile[] cardTargets = new Tile[cardManager.hand.Count];
        for (int i = 0; i < cardManager.hand.Count; i++)
        {
            cardValues[i] = int.MaxValue;
            if (cardManager.hand[i] == null) continue;
            if (cardManager.hand[i].apCost > ap) continue;

            if (cardManager.hand[i] is MoveCard)
            {
                cardValues[i] = CardValueToMove(cardManager.hand[i], ref cardTargets[i], aggroTarget.tile);
            }
        }
        int playCardIdx = cardValues.ToList().IndexOf(cardValues.Min());
        if (cardValues[playCardIdx] == int.MaxValue) return false;

        MoveCard chosenCard = (MoveCard)cardManager.hand[playCardIdx];
        cardManager.hand[playCardIdx].Use(cardTargets[playCardIdx], this);

        return true;
    }

    private int CardValueToTarget(Card card, Unit target)
    {
        Object empty = new Object();
        int ret = card.CardValue(tile, this, target, ref empty);
        //cardTarget = (Tile)passValue;
        return ret;
    }

    private int CardValueToMove(Card card, ref Tile cardTarget, Tile targetTile)
    {
        Object passValue = cardTarget;
        int ret = card.CardValue(tile, this, targetTile, ref passValue);
        cardTarget = (Tile)passValue;
        return ret;
    }

    private void GetAggroTarget()
    {

    }

    private void FindMaxAttackRange()
    {

    }
}
