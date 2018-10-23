using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopScript : MonoBehaviour {

	[Header("Ressources costs UI elements")]
	public Text copperUpCostText;
	public Text ironUpCostText;
	public Text coalUpCostText;
	public Text uraniumUpCostText;

	[Header("Gold costs UI elements")]
	public Text copperUpGoldCostText;
	public Text ironUpGoldCostText;
	public Text coalUpGoldCostText;
	public Text uraniumUpGoldCostText;

	[Header("Collect upgrades UI elements")]
	public Text copperCollectUpText;
	public Text ironCollectUpText;
	public Text coalCollectUpText;
	public Text uraniumCollectUpText;

	private Player player;

	void Start(){
		player = GameManager.playerInstance;
		updateUpgradesButtons ();
	}

	void updateUpgradesButtons(){
		copperUpCostText.text = "" + ressourceCalculation ("copper");
		copperUpGoldCostText.text = "" + goldCostsCalculation ("copper");
		ironUpCostText.text = "" + ressourceCalculation ("iron");
		ironUpGoldCostText.text = "" + goldCostsCalculation ("iron");
		coalUpCostText.text = "" + ressourceCalculation ("coal");
		coalUpGoldCostText.text = "" + goldCostsCalculation ("coal");
		uraniumUpCostText.text = "" + ressourceCalculation ("uranium");
		uraniumUpGoldCostText.text = "" + goldCostsCalculation ("uranium");

		copperCollectUpText.text = "" + getPlayerDigDamage ("copper");
		ironCollectUpText.text = "" + getPlayerDigDamage ("iron");
		coalCollectUpText.text = "" + getPlayerDigDamage ("coal");
		uraniumCollectUpText.text = "" + getPlayerDigDamage ("uranium");

	}
	int getPlayerDigDamage(string ressource){
		return player.getDigDamages () [ressource];
	}
	int goldCostsCalculation(string ressource){
		int ressourceAmount = getPlayerDigDamage(ressource);
		return 60+ressourceAmount + ressourceAmount / 3;
	}
	int ressourceCalculation(string ressource){
		int ressourceAmount =getPlayerDigDamage(ressource);
		return 60 + ressourceAmount + ressourceAmount / 2;
	}
	public void upgradeCollect(string ressource){
		if (player.enoughRessource("gold", goldCostsCalculation(ressource)) && player.enoughRessource (ressource, ressourceCalculation(ressource))) {
			player.upgradeDigDamage(ressource, 1);
			player.loseRessource ("gold", goldCostsCalculation(ressource));
			player.loseRessource (ressource, goldCostsCalculation(ressource));
		}
		updateUpgradesButtons ();
	}
}
