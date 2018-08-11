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
		int previousHp = hp;
		hp -= loss;
		PopupFloatingText.instance.ShowMessage ("+"+loss, transform, RessourcesManager.getRessourceColor(type));
		//GameManager.playerInstance.GetComponent<Player>().getRessources ()[type]+=previousHp-hp;
		GameManager.playerInstance.GetComponent<Player>().gainRessource(type, previousHp-hp);

		Vector3 particlesPos = new Vector3 (transform.position.x, transform.position.y, transform.position.z-1);
		Instantiate (damageParticles, particlesPos, Quaternion.identity);
		if (hp <= 0){
			MapManager.instance.resetPosition (gameObject.transform.position);
			//Destroy(gameObject);
		}

	}
	public string getType(){
		return type;
	}
	public int getHp (){
		return hp;
	}
	public void heal(int value){
		hp += value;
	}
}
