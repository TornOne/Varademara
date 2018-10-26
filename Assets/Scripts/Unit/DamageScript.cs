using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageScript : MonoBehaviour {

    // Damage calculator placeholder, TODO: complex calculations
    //  Physical damage + Physical defence
    //  Magical damage + Magical defence
    //  True damage
	public int DamageCalc(int physDmg, int magiDmg, int trueDmg, Unit target)
    {
        int pDmg = physDmg - target.pDef;
        int mDmg = magiDmg - target.mDef;
        return pDmg + mDmg + trueDmg;
    }

    // Deal damage to target unit
    public void Damage(int physDmg, int magiDmg, int trueDmg, Unit target)
    {
        target.Damage(DamageCalc(physDmg, magiDmg, trueDmg, target));
    }
}
