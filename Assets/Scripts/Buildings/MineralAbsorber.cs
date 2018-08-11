using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineralAbsorber : Generator {
	
	public int production;
	float productionTick;
	// Use this for initialization
	protected override void Awake () {
		base.Awake ();
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();
		if (productionTick <= time) {
			collectNearbyOre ();
		}
	}
	void collectNearbyOre(){
		for (int x = -1; x <= 1; x++){
			for (int y = -1; y <= 1; y++){
				if (x == 0 && y == 0)
					continue;
				int checkX = (int)transform.position.x + x;
				int checkY = (int)transform.position.y + y;
				if (checkX >= 0 && checkX < mapManager.getColumns() && checkY >= 0 && checkY < mapManager.getRows() ) {
					GameObject tempObj = mapManager.getObjectAt (checkX, checkY);
					if (tempObj != null && tempObj.GetComponent<OreVein> () != null) {
						tempObj.GetComponent<OreVein> ().damage (production);
					}
				}
			}
		}
		productionTick = time + powerGenSpeed;
	}
}
