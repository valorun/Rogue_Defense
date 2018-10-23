using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEMineralHealer : AOEHealer {

	void Update () {
		time=GameManager.instance.getTime();
		if (power>=consumedPower) {
			if (nextTick <= time) {
				healNearbyBuildings ();
				healNearbyOreVeins ();
				power-=consumedPower;
				nextTick = time + healSpeed;
			}
		}
	}
	void healNearbyOreVeins(){
		Vector2 pos = new Vector2((int)transform.position.x, (int)transform.position.y);
		foreach(GameObject tempObj in MapManager.instance.getNearbyGameObjects(pos, 1)){
			if (tempObj.GetComponent<OreVein> () != null) {
				Vector3 vectorToTarget = tempObj.transform.position - transform.position;
				float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
				Quaternion q = Quaternion.AngleAxis(angle-90, Vector3.forward);
				Instantiate (healingParticles, transform.position,  q);
				tempObj.GetComponent<OreVein> ().heal (healingPower*2);
			}
		}
	}
}
