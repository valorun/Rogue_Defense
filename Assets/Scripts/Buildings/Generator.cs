using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : Building {

	public int power;
	public float powerGenSpeed = 1.5f;
	float nextTick;
	protected float time;
	protected MapManager mapManager;
	public GameObject powerMask;
	public GameObject powerParticles;
	public AudioClip powerSound;
	private GameObject target;
	// Use this for initialization
	protected override void Awake () {
		base.Awake ();
		mapManager=MapManager.instance;
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		time=GameManager.instance.getTime();
		target = findLesserPowered();
		if (target != null) {
			givePower ();
		}
	}
	void givePower(){
		if (nextTick <= time) {
			Vector3 vectorToTarget = target.transform.position - transform.position;
			float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
			Quaternion q = Quaternion.AngleAxis(angle-90, Vector3.forward);
			Instantiate (powerParticles, new Vector3(transform.position.x,transform.position.y,transform.position.z-1),  q);
			audioSource.PlayOneShot (powerSound, 0.25f);
			target.GetComponent<PoweredBuilding> ().gainPower (power);
			nextTick = time + powerGenSpeed;
		}
	}
	GameObject findLesserPowered(){
		GameObject lesserPowered = null;
		for (int x = -1; x <= 1; x++){
			for (int y = -1; y <= 1; y++){
				if (x == 0 && y == 0)
					continue;
				int checkX = (int)transform.position.x + x;
				int checkY = (int)transform.position.y + y;
				if (checkX >= 0 && checkX < mapManager.getColumns() && checkY >= 0 && checkY < mapManager.getRows() ) {
					GameObject tempObj = mapManager.getObjectAt (checkX, checkY);
					if (tempObj != null && tempObj.GetComponent<PoweredBuilding> () != null) {
						if (lesserPowered == null || tempObj.GetComponent<PoweredBuilding> ().getMissingPower () > lesserPowered.GetComponent<PoweredBuilding> ().getMissingPower ())
							lesserPowered = tempObj;
					}
				}
			}
		}
		return lesserPowered;
	}
	public int getProducedPower(){
		return power;
	}
	public override void selection(){
		base.selection ();
		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {
				int checkX = (int)transform.position.x + x;
				int checkY = (int)transform.position.y + y;
				if (x == 0 && y == 0)
					continue;
				GameObject mask=Instantiate (powerMask, new Vector3(checkX,checkY, -1),  Quaternion.identity) as GameObject;
				mask.transform.SetParent (selectionTiles);
			}
		}
	}
}
