using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreVein : Destructible {

	public string type; //mineral type
	void Start () {
		int tempHp = Random.Range(hp/2, hp+hp/2);
		hp = tempHp;
	}

	public override void damage(int loss){
		hp -= loss;
		Vector3 particlesPos = new Vector3 (transform.position.x, transform.position.y, transform.position.z-1);
		Instantiate (damageParticles, particlesPos, Quaternion.identity);
		if (hp <= 0){
			MapManager.instance.resetPosition (gameObject.transform.position);
		}
	}

	public int collect(int loss){
		if(loss > hp)
			loss = hp;
		PopupFloatingText.instance.ShowMessage ("+"+loss, transform, RessourcesManager.getRessourceColor(type));
		damage(loss);
		return loss;
	}

	public string getType(){
		return type;
	}
}
