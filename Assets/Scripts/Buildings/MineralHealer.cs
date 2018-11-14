using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineralHealer : Healer {
	
	// Update is called once per frame
	void Update () {
		time=GameManager.instance.getTime();
		target = findMostDamaged();
		Debug.Log (consumedPower);
		if (target != null && power>=consumedPower) {
			if (target.GetComponent<Building>()!=null)giveHeal ();
			else if (target.GetComponent<OreVein>()!=null)healOreVein();
		}
	}
	void healOreVein(){
		if (nextTick <= time) {
			Vector3 vectorToTarget = target.transform.position - transform.position;
			float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
			Quaternion q = Quaternion.AngleAxis(angle-90, Vector3.forward);
			Instantiate (healingParticles, transform.position,  q);
			target.GetComponent<OreVein> ().heal (healingPower*2);
			power-=consumedPower;
			nextTick = time + healSpeed;
		}
	}
	protected GameObject findMostDamaged(){
		Vector2 pos = new Vector2((int)transform.position.x, (int)transform.position.y);
		GameObject mostDamaged = base.findMostDamaged();
		if((mostDamaged!=null && mostDamaged.GetComponent<Building> ().getMissingHp ()<=0 ) || mostDamaged==null){ //if no building need heal, then heal ore
			foreach(GameObject tempObj in MapManager.instance.getNearbyGameObjects(pos, 1)){
				if (tempObj.GetComponent<OreVein> () != null) {
					if (mostDamaged == null || tempObj.GetComponent<OreVein> ().getHp () < mostDamaged.GetComponent<OreVein> ().getHp ())
						mostDamaged = tempObj;
				}
			}
		}
		return mostDamaged;
	}
}
