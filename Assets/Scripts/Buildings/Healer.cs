using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : PoweredBuilding {
	public GameObject healingMask;
	public GameObject healingParticles;
	public int healingPower;
	public float healSpeed = 1.5f;
	protected float nextTick;
	protected float time;
	protected GameObject target;

	// Use this for initialization
	protected override void Awake () {
		base.Awake ();
	}
	// Update is called once per frame
	void Update () {
		time=GameManager.instance.getTime();
		target = findMostDamaged();
		if (target != null && power>=consumedPower) {
			if (target.GetComponent<Building>()!=null)giveHeal ();
		}
	}

	protected void giveHeal(){
		if (nextTick <= time && target.GetComponent<Building>().getMissingHp()>0) {
			Vector3 vectorToTarget = target.transform.position - transform.position;
			float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
			Quaternion q = Quaternion.AngleAxis(angle-90, Vector3.forward);
			Instantiate (healingParticles, new Vector3(transform.position.x,transform.position.y,transform.position.z-1),  q);
			target.GetComponent<Building> ().heal (healingPower);
			power-=consumedPower;
			nextTick = time + healSpeed;
		}
	}
	protected GameObject findMostDamaged(){
		GameObject mostDamaged = null;
		//check the most damaged nearby building
		Vector2 pos = new Vector2((int)transform.position.x, (int)transform.position.y);
		foreach(GameObject tempObj in MapManager.instance.getNearbyGameObjects(pos, 1)){
			if (tempObj != null && tempObj.GetComponent<Building> () != null) {
				if (mostDamaged == null || tempObj.GetComponent<Building> ().getMissingHp () > mostDamaged.GetComponent<Building> ().getMissingHp ())
					mostDamaged = tempObj;
			}
		}
		return mostDamaged;
	}

	public int getHealingPower(){
		return healingPower;
	}
	public override void selection(){
		base.selection ();
		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {
				int checkX = (int)transform.position.x + x;
				int checkY = (int)transform.position.y + y;
				if (x == 0 && y == 0)
					continue;
				GameObject mask=Instantiate (healingMask, new Vector3(checkX,checkY, -1),  Quaternion.identity) as GameObject;
				mask.transform.SetParent (selectionTiles);
			}
		}
	}


}
