using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Destructible {
	public string name;
	public RessourcesManager.SerializedRessource[] costs;
	protected int maxHp;
	public UpgradeBuilding[] upgradeBuildings;
	public GameObject selectionMask;
	protected Transform selectionTiles;
	protected AudioSource audioSource;

	void OnValidate(){ //check if the costs corresponds to the existings ressources
		RessourcesManager.SerializedRessource[] ressources = RessourcesManager.getRessourceList ();
		if (costs.Length != ressources.Length)costs = RessourcesManager.getRessourceList ();
		bool namesChanged = false;
		for(int i=0; i<ressources.Length;i++){
			if (costs[i].name != ressources[i].name)
				namesChanged = true;
		}
		if (namesChanged) costs = RessourcesManager.getRessourceList ();
	}
	protected virtual void Awake () {
		selectionTiles = transform.Find ("SelectionTiles");
		audioSource = gameObject.GetComponent<AudioSource> ();
		maxHp = hp;
		buy ();
	}

	public RessourcesManager.SerializedRessource[] getCosts(){
		return costs;
	}

	public void buy(){
		foreach (RessourcesManager.SerializedRessource r in costs) {
			GameManager.playerInstance.GetComponent<Player> ().loseRessource(r.name, r.value);
		}

	}
	public override void damage(int loss){
		hp -= loss;
		Instantiate (damageParticles, transform.position, Quaternion.identity);
		if (hp <= 0){
			MapManager.instance.resetPosition (gameObject.transform.position);
		}
	}
	public int getHp(){
		return hp;
	}
	public void heal(int value){
		if (value + hp > maxHp)
			hp = maxHp;
		else
			hp += value;
	}
	public int getMissingHp(){
		return maxHp - hp;
	}
	public string getName(){
		return name;
	}
	public virtual void selection(){
		GameObject mask=Instantiate (selectionMask, new Vector3((int)transform.position.x, (int)transform.position.y, -1),  Quaternion.identity) as GameObject;
		mask.transform.SetParent (selectionTiles);
	}
	public void deselection(){
		foreach (Transform child in selectionTiles){
			Destroy(child.gameObject);
		}
	}

	public bool upgrade(string type){
		GameObject tempUpgrade = getUpgrade (type);
		if (tempUpgrade != null && GameManager.playerInstance.GetComponent<Player> ().enoughRessources (getUpgradeCosts(type))) {
			MapManager.instance.resetPosition (gameObject.transform.position);
			MapManager.instance.placeObjectAt (tempUpgrade, transform.position);
			foreach (RessourcesManager.SerializedRessource r in costs) { // give back ressources of the building, because the upgrade will be buy after
				GameManager.playerInstance.GetComponent<Player> ().gainRessource (r.name, r.value);
			}
			return true;
		}
		return false;
	}

	public bool hasUpgrade(string type){
		foreach (UpgradeBuilding b in upgradeBuildings) {
			if (b.type == type)
				return true;
		}
		return false;
	}

	public RessourcesManager.SerializedRessource[] getUpgradeCosts(string type){
		if (hasUpgrade (type)) {
			RessourcesManager.SerializedRessource[] upgradeBuildingCosts=getUpgrade (type).GetComponent<Building> ().getCosts ();
			RessourcesManager.SerializedRessource[] upgradeCosts =  new RessourcesManager.SerializedRessource [upgradeBuildingCosts.Length]; // costs after substraction
			int i=0;
			foreach (RessourcesManager.SerializedRessource r in costs) {
				upgradeCosts [i].name = r.name;
				upgradeCosts [i].value = 0;
				foreach (RessourcesManager.SerializedRessource r1 in upgradeBuildingCosts){
					if (r.name == r1.name) {
						upgradeCosts [i].value = r1.value - r.value;
					}
				}
				i++;
			}
			return upgradeCosts;
		}
		return null;
	}

	public GameObject getUpgrade(string type){
		foreach (UpgradeBuilding b in upgradeBuildings) {
			if (b.type == type)
				return b.building;
		}
		return null;
	}
	//sell the building and give back 1/4 of his cost
	public void sell(){
		foreach (RessourcesManager.SerializedRessource r in costs) {
			GameManager.playerInstance.GetComponent<Player> ().gainRessource (r.name, r.value/4);
		}
		MapManager.instance.resetPosition (gameObject.transform.position);
	}

	[System.Serializable]
	public struct UpgradeBuilding {
		public string type;
		public GameObject building;
	}
}
