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

    //public int xPos;    //map position
    //public int yPos;

    public DamageScript attack_calc;
    public MovementScript move_calc;

    public float moveAnimationSpeed;
    private bool animationFinished = false;

    private Vector3 Start;
    private List<Vector3> End;
    private float moveLerp;
    public float attackLerp;

    public MovementScript.TilePlaceholder tile;
    public List<MovementScript.TilePlaceholder> path;

    

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

    public void Move(List<MovementScript.TilePlaceholder> targetTile)
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
            transform.position = Vector3.Lerp(path[(int)moveLerp + 1].Position(), path[(int)moveLerp].Position(), Mathf.Min((1f - moveLerp % 1) * 1.5f, 1f));
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
            Gizmos.DrawLine(path[i-1].Position(), path[i].Position());
        }
    }

}
