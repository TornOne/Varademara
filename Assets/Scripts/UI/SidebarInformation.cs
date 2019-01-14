using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SidebarInformation : MonoBehaviour {
	public Text[] textFields;
	public Image avatar;
	public Text title; //TODO Units need names?
	Unit lastUnit;

	void Start() {
		StartCoroutine(SidebarFillLoop());
	}

	public void FillSidebar(Unit unit) {
		lastUnit = unit;
		textFields[0].text = unit.ap.ToString();
		textFields[1].text = unit.HP.ToString();
		textFields[2].text = unit.mAtt.ToString();
		textFields[3].text = unit.mDef.ToString();
		textFields[4].text = unit.pAtt.ToString();
		textFields[5].text = unit.pDef.ToString();
		textFields[6].text = unit.balance.ToString() + "%";
		textFields[7].text = unit.HandSize.ToString();
		avatar.sprite = unit.avatar;
		title.text = unit.title;
	}

	IEnumerator SidebarFillLoop() {
		while (true) {
			if (lastUnit != null) {
				FillSidebar(lastUnit);
			}
			yield return new WaitForSeconds(1);
		}
	}
}
