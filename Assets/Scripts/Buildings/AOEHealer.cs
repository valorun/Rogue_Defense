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
		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {
				if (x == 0 && y == 0)
					continue;
				int checkX = (int)transform.position.x + x;
				int checkY = (int)transform.position.y + y;
				if (checkX >= 0 && checkX < mapManager.getColumns () && checkY >= 0 && checkY < mapManager.getRows ()) {
					GameObject tempObj = mapManager.getObjectAt (checkX, checkY);
					if (tempObj != null && tempObj.GetComponent<Building> () != null && tempObj.GetComponent<Building> ().getMissingHp () > 0) {
						Vector3 vectorToTarget = tempObj.transform.position - transform.position;
						float angle = Mathf.Atan2 (vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
						Quaternion q = Quaternion.AngleAxis (angle - 90, Vector3.forward);
						Instantiate (healingParticles, transform.position, q);
						tempObj.GetComponent<Building> ().heal (healingPower);
					}
				}
			}
		}
			
	}
}
