using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyAI : Unit {

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

        cardManager.hand.Add(new MoveCard());//TODO: ai needs cards

        while (ap > 0) {
            if (!calculateHandValuesMove()) break;
        }

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


        TurnManager.instance.NextTurn();
	}

    private void calculateHandValuesAction()
    {
        List<int> cardValues = new List<int>(cardManager.hand.Count);
        for (int i = 0; i < cardManager.hand.Count; i++)
        {
            if (cardManager.hand[i] is AttackCard)
            {
                //Attack/debuff cards
                cardValues[i] = cardValueToTarget(cardManager.hand[i], aggroTarget);
            }
            if (cardManager.hand[i] is DefendCard)
            {
                //Buff/heal cards
                cardValues[i] = cardValueToTarget(cardManager.hand[i], aggroTarget);
            }

        }
    }

    private bool calculateHandValuesMove()
    {

        int[] cardValues = new int[cardManager.hand.Count];
        Tile[] cardTargets = new Tile[cardManager.hand.Count];
        for (int i = 0; i < cardManager.hand.Count; i++)
        {
            cardValues[i] = int.MaxValue;
            if (cardManager.hand[i] is MoveCard)
            {
                if (cardManager.hand[i].apCost <= ap)
                {
                    cardValues[i] = cardValueToMove(cardManager.hand[i], ref cardTargets[i], aggroTarget.tile);
                }
            }
        }
        int playCardIdx = cardValues.ToList().IndexOf(cardValues.Min());
        if (cardValues[playCardIdx] == int.MaxValue) return false;

        ap -= 1;//TODO: ap calculation


        Move(new List<Tile>() { tile, cardTargets[playCardIdx] });

        return true;
    }

    private int cardValueToTarget(Card card, Unit target)
    {

        return 0;
    }

    private int cardValueToMove(Card card, ref Tile cardTarget, Tile targetTile)
    {
        Object passValue = (Object)cardTarget;
        int ret = card.CardValue(tile, this, (Object)targetTile, ref passValue);
        cardTarget = (Tile)passValue;
        return ret;
    }

    private void getAggroTarget()
    {

    }
}
