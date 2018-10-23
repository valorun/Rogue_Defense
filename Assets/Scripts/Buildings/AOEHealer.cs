using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEHealer : Healer {

	void Update () {
		time=GameManager.instance.getTime();
		if (power>=consumedPower) {
			if (nextTick <= time) {
				healNearbyBuildings ();
				power -= consumedPower;
				nextTick = time + healSpeed;
			}
		}
	}
	protected void healNearbyBuildings(){
		Vector2 pos = new Vector2((int)transform.position.x, (int)transform.position.y);
		foreach(GameObject tempObj in MapManager.instance.getNearbyGameObjects(pos, 1)){
			if (tempObj.GetComponent<Building> () != null && tempObj.GetComponent<Building> ().getMissingHp () > 0) {
				Vector3 vectorToTarget = tempObj.transform.position - transform.position;
				float angle = Mathf.Atan2 (vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
				Quaternion q = Quaternion.AngleAxis (angle - 90, Vector3.forward);
				Instantiate (healingParticles, transform.position, q);
				tempObj.GetComponent<Building> ().heal (healingPower);
			}
			
		}
			
	}
}
