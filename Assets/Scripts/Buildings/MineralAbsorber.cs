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
		Vector2 pos = new Vector2((int)transform.position.x, (int)transform.position.y);
		foreach(GameObject tempObj in MapManager.instance.getNearbyGameObjects(pos, 1)){
			if (tempObj.GetComponent<OreVein> () != null) {
				tempObj.GetComponent<OreVein> ().damage (production);
			}
		}
		productionTick = time + powerGenSpeed;
	}
}
