using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitScript : MonoBehaviour {

    public int hP;  //health points
    public int aP;  //action points
    public int mP;  //movement points

    public int pDef;    //physical resistance
    public int mDef;    //magical resistance

    public int initiative;  //turn order influencer
    public int idle;        //turns waited

    public int xPos;    //map position
    public int yPos;

    public DamageScript attack_calc;
    public MovementScript move_calc;

    public float moveAnimationSpeed;

    private Vector3 Start;
    private List<Vector3> End;
    private float lerp;

    // Calculate unit turn weight for turn order
    public int TurnWeight()
    {
        return initiative + 2 * idle;
    }

    // Deal damage to this unit
    public void Damage(int dmg)
    {
        hP -= dmg;
        if (hP <= 0) Destroy(gameObject);
    }

    public void Move(List<Vector3> target)
    {
        //Start = new Vector3(xPos, yPos, 0);
        End = target;
        lerp = target.Count;
        End.Add(new Vector3(xPos, yPos, 0));
        xPos = (int)End[0].x;
        yPos = (int)End[0].y;
    }

    private void Update()
    {
        if (lerp > 0)
        {
            lerp -= Time.deltaTime * moveAnimationSpeed;
            transform.position = Vector3.Lerp(End[(int)lerp+1], End[(int)lerp], (1f-lerp%1));
        }
    }

}
