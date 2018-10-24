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

    private Vector3 Start;
    private List<Vector3> End;
    private float lerp;

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

    public void Move(List<Vector3> target, List<MovementScript.TilePlaceholder> targetTile)
    {
        lerp = target.Count;
        path = targetTile;

        path.Add(tile);
        tile = path[0];
    }

    private void Update()
    {
        if (lerp > 0)
        {
            lerp -= Time.deltaTime * moveAnimationSpeed;
            transform.position = Vector3.Lerp(path[(int)lerp + 1].Position(), path[(int)lerp].Position(), (1f - lerp % 1));
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
