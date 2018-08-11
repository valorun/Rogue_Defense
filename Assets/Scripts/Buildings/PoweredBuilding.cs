using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoweredBuilding : Building {
	protected MapManager mapManager;
	public int maxPower;
	protected int power=0;
	public int consumedPower;
	protected override void Awake(){
		base.Awake ();
		mapManager=MapManager.instance;
	}

	public int getConsumedPower(){
		return consumedPower;
	}
	public int getPower(){
		return power;
	}
	public int getMaxPower(){
		return maxPower;
	}
	public int getMissingPower(){
		return maxPower - power;
	}
	public void gainPower(int value){
		if (value + power > maxPower)
			power = maxPower;
		else
			power += value;
	}
}
