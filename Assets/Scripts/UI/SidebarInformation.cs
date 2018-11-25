using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SidebarInformation : MonoBehaviour {
    public Text[] textFields;
    public Image avatar;
    public Text title; //TODO Units needs names?

    public void FillSidebar(Unit unit){
        textFields[0].text = "" + unit.ap;
        textFields[1].text = "" + unit.HP;
        textFields[2].text = "" + unit.mAtt;
        textFields[3].text = "" + unit.mDef;
        textFields[4].text = "" + unit.pAtt;
        textFields[5].text = "" + unit.pDef;
        textFields[6].text = "" + unit.balance+"%";
        textFields[7].text = "" + unit.HandSize;
        avatar.sprite = unit.avatar;
    }
}
