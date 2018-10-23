using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : Destructible {
	public override void damage(int loss){
		hp -= loss;
		Vector3 particlesPos = new Vector3 (transform.position.x, transform.position.y, transform.position.z-1);
		Instantiate (damageParticles, particlesPos, Quaternion.identity);
		if (hp <= 0){
			MapManager.instance.resetPosition (gameObject.transform.position);
			if (gameObject.GetComponent<Loot> () != null) {
				gameObject.GetComponent<Loot> ().spawnLoot ();
			}
			//Destroy(gameObject);
		}

	}


}
