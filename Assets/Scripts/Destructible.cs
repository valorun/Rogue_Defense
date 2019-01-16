using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Destructible : MonoBehaviour {
	public GameObject damageParticles;
	public int hp;
	protected int maxHp=0;
	protected virtual void Awake () {
		maxHp=hp;
	}
	public abstract void damage (int loss);
	public virtual void heal(int value){
		if (value + hp > maxHp || maxHp == 0)
			hp = maxHp;
		else
			hp += value;
	}
	public virtual int getMissingHp(){
		return maxHp - hp;
	}

	public int getHp(){
		return hp;
	}
}
