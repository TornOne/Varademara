using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {
	public Unit knightRider, demonFairy, Ogre;
	public Unit pikeman, swordman, monk, griffin;

	void Start() {
		//TODO: Add a proper wave system with enemy counter, delays, and whatever else
		Tile myTile = Map.instance.GetTile(11, 4);
		Unit myUnit = Instantiate(knightRider, myTile.transform.position, Quaternion.Euler(45, 0, 0));
		myTile.unit = myUnit;
		myUnit.tile = myTile;
		myTile = Map.instance.GetTile(11, 7);
		myUnit = Instantiate(demonFairy, myTile.transform.position, Quaternion.Euler(45, 0, 0));
		myTile.unit = myUnit;
		myUnit.tile = myTile;
        myTile = Map.instance.GetTile(11, 11);
		myUnit = Instantiate(Ogre, myTile.transform.position, Quaternion.Euler(45, 0, 0));
		myTile.unit = myUnit;
		myUnit.tile = myTile;
		for (int i = 2; i < 7; i += 2) {
			Tile enemyTile = Map.instance.GetTile(1, i);
			Unit enemyUnit = Instantiate(pikeman, enemyTile.transform.position, Quaternion.Euler(45, 0, 0));
			enemyUnit.tile = enemyTile;
			enemyTile.unit = enemyUnit;
		}
	}
}
