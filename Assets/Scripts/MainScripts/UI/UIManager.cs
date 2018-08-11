using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour {

	public static UIManager instance = null;

	public event System.Action OnCanvasEnabledEvent;

	private Player player;
	public Canvas uiCanvas;
	public Canvas deathCanvas;
	public Canvas levelEndCanvas;
	public Canvas shopCanvas;
	public Canvas helpCanvas;
	public Text timeLeftText;
	public Text waveText;


	public ItemSlot itemSlot;
	public BuildingsBar buildingsBar;
	public SelectionBar selectionBar;
	public CostsBar costsBar;
	public RessourcesBar ressourcesBar;
	public LevelEndMenu levelEndMenu;
	public DeathMenu deathMenu;
	public HpIcon hpIcon;


	void Start () {
		player = GameManager.playerInstance.GetComponent<Player> ();
		costsBar.updateDisplay();
		hpIcon.updateDisplay ();
		ressourcesBar.updateDisplay ();

		player.OnRessourcesChangeEvent += ressourcesBar.updateDisplay;
		player.OnRessourcesChangeEvent += itemSlot.updateIconColor;
		player.OnSelectBuildingEvent += itemSlot.updateIconColor;
		player.OnDeselectBuildingEvent += itemSlot.updateIconColor;
		player.OnSelectBuildingEvent += costsBar.updateDisplay;
		player.OnDeselectBuildingEvent += costsBar.updateDisplay;
		player.OnPlaceBuildingEvent += costsBar.updateDisplay;

		player.OnSelectBuildingEvent += buildingsBar.updateDisplay;
		player.OnDeselectBuildingEvent += buildingsBar.updateDisplay;

		player.OnSelectBuildingEvent += selectionBar.updateDisplay;
		player.OnDeselectBuildingEvent += selectionBar.updateDisplay;

		player.OnDiedEvent += deathMenu.display;
		player.OnDiedEvent += checkUiCanBeDiplayed;
		player.OnHPChangeEvent += hpIcon.updateDisplay;

		GameManager.instance.OnLevelInfoUpdateEvent += updateLevelInfos;
	}
	void Awake(){
		if (instance == null)instance = this;
	}

	public bool isPointerOverUI(Vector2 pos){
		//Code to be place in a MonoBehaviour with a GraphicRaycaster component
		PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
		eventDataCurrentPosition.position = pos;
		List<RaycastResult> results = new List<RaycastResult> ();
		EventSystem.current.RaycastAll (eventDataCurrentPosition, results);
		return results.Count > 0;
	}
	public void updateLevelInfos(){
		timeLeftText.text = "Next wave in: " + Mathf.Round(GameManager.instance.getTimeLeft());
		waveText.text = "Wave " + GameManager.instance.getWave();

	}
	int getPlayerRessource(string ressource){
		return GameManager.playerInstance.GetComponent<Player> ().getRessources () [ressource];
	}
		
	void checkUiCanBeDiplayed(){
		if (!deathCanvas.enabled && !shopCanvas.enabled && !levelEndCanvas.enabled && !helpCanvas.enabled) {
			uiCanvas.enabled = true;
			uiCanvas.gameObject.SetActive (true);
		} else {
			uiCanvas.enabled = false;
			uiCanvas.gameObject.SetActive (false);
		}
	}
	public void displayShop(){
		shopCanvas.enabled = !shopCanvas.enabled;
		checkUiCanBeDiplayed ();
		costsBar.updateDisplay ();
		if (Time.timeScale == 1f)
			Time.timeScale = 0f;
		else
			Time.timeScale = 1f;
	}
	public void displayHelp(){
		helpCanvas.enabled = !helpCanvas.enabled;
		checkUiCanBeDiplayed ();
		costsBar.updateDisplay ();
		if (Time.timeScale == 1f)
			Time.timeScale = 0f;
		else
			Time.timeScale = 1f;
	}
	public ItemSlot getItemSlot(){
		return itemSlot;
	}
	public BuildingsBar getBuildingsBar(){
		return buildingsBar;
	}
	public SelectionBar getSelectionBar(){
		return selectionBar;
	}
	public CostsBar getCostsBar(){
		return costsBar;
	}
	public RessourcesBar getRessourcesBar(){
		return ressourcesBar;
	}
	public LevelEndMenu getLevelEndMenu(){
		return levelEndMenu;
	}
	public DeathMenu getDeathMenu(){
		return deathMenu;
	}
	public HpIcon getHpIcon(){
		return hpIcon;
	}
		

}
