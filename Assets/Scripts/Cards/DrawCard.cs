using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DrawCard : Card
{
    public int drawCount = 1;
    public bool fillHand = false;
    public bool discardHand = false;

    protected override bool PreActivate(Unit caster, bool select)
    {
        return true;
    }

    protected override bool Activate(Tile tile, Unit caster)
    {
        if (discardHand)
        {
            foreach (Card card in caster.cardManager.hand)
            {
                caster.cardManager.Discard(card);
            }
        }

        if (fillHand) caster.cardManager.FillHand();
        else
        {
            for (int i = 0; i < drawCount; i++)
            {
                caster.cardManager.DrawCard();
            }
        }
        return true;
    }

    public override int CardValue(Tile tile, EnemyAI caster, Object target, ref Object extra)
    {
        float score = 0;

        if (discardHand) score -= caster.cardManager.hand.Count - 1;

        if (fillHand) score += (caster.cardManager.handSize - caster.cardManager.hand.Count - 1);

        else score += (float)(Mathf.Min(caster.cardManager.hand.Count + drawCount, caster.cardManager.handSize)) / (float)(caster.cardManager.hand.Count) * 4;

        return (int)(score);
    }
}
