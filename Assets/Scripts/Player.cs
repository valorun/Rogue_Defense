using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Player : Destructible {

	public event System.Action OnRessourcesChangeEvent;
	public event System.Action OnCollectItemEvent;
	public event System.Action OnPlaceBuildingEvent;
	public event System.Action OnSelectBuildingEvent;
	public event System.Action OnDeselectBuildingEvent;
	public event System.Action OnDiedEvent;
	public event System.Action OnHPChangeEvent;

	public int digDamagesToRocks;

	//public int digDamage = 1;
	public float speed = 3f;
	public float maxInteractionDistance = 0.5f; // maximum distance at which the player can destroy an object
	public float maxConstructionDistance = 0.5f; // maximum distance at which the player can destroy an object

	Dictionary<string, int> ressources;
	Dictionary<string, int> digDamages;
	private GameObject buildingToPlace; //for construction
	private GameObject selectedTile;
	bool placingObject;

	Vector2 playerGridPos; //player position in the grid
	Camera playerCamera;
	//private Vector2 destination; //move destination

	MapManager mapManager;
	AudioSource audioSource;
	public AudioClip collectSound;
	public AudioClip digSound;
	public GameObject colorMask;
	protected Transform selectionTiles;
	private Transform sprite;

	private Vector2 startPosition; // vars for mobile inputs
	private Vector2 endPosition; //
	private bool moving; //

	GameObject itemSlot;

	void Awake () {
		buildingToPlace = null;
		placingObject = false;
		audioSource = gameObject.GetComponent<AudioSource>();
		mapManager=MapManager.instance;

		ressources = RessourcesManager.getRessourcesDictionnary ();
		ressources ["gold"] = PlayerPrefs.GetInt ("goldAmount");
		ressources ["copper"] = PlayerPrefs.GetInt ("copperAmount");
		ressources ["iron"] = PlayerPrefs.GetInt ("ironAmount");
		ressources ["coal"] = PlayerPrefs.GetInt ("coalAmount");
		ressources ["uranium"] = PlayerPrefs.GetInt ("uraniumAmount");
		digDamages = RessourcesManager.getRessourcesDictionnary (1);

		playerCamera = transform.Find("Camera").GetComponent<Camera>();
		selectionTiles = transform.Find ("SelectionTiles");
		sprite = transform.Find ("Sprite");
	}
		
	// Update is called once per frame
	void Update () {
		if (!isDead()) {
			if (Application.platform == RuntimePlatform.Android)mobileInputs ();
			else pcInputs ();
			/*if (placingObject) {
				destination = transform.position;
			} else {
				transform.position = Vector3.MoveTowards (transform.position, destination, speed * Time.deltaTime);
			}*/
		}
	}

	//permet la rotation du joueur vers la position ciblée
	void rotateToTarget(Vector2 targetPos){
		Vector3 targetDir = new Vector3 (targetPos.x, targetPos.y, 0) - transform.position;
		float angle = Mathf.Atan2 (targetDir.y, targetDir.x) * Mathf.Rad2Deg;
		sprite.transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);
	}

	//inputs pour les joueurs PC
	void pcInputs(){
		if (Input.GetKeyDown (KeyCode.Mouse1))endPlaceBuilding ();
		if (Input.GetKeyDown (KeyCode.Mouse0)) {
			if(!UIManager.instance.isPointerOverUI(new Vector2(Input.mousePosition.x, Input.mousePosition.y))) //prevent from clicking through UI
				interract (playerCamera.ScreenToWorldPoint (Input.mousePosition));
		}
		move(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
		if(Input.GetKeyDown(KeyCode.Escape))SceneManager.LoadScene("Menu");
	}

	//inputs pour les joueurs mobile
	void mobileInputs(){
		if (Input.touchCount == 1)
		{
			var touch = Input.touches[0];

			switch (touch.phase)
			{
				case TouchPhase.Began:
					startPosition = touch.position;
					if(!UIManager.instance.isPointerOverUI(new Vector2(Input.GetTouch (0).position.x, Input.GetTouch (0).position.y))) //prevent from clicking through UI
						interract (playerCamera.ScreenToWorldPoint(startPosition));
					break;
				case TouchPhase.Ended:
					endPosition = touch.position;
					if(Vector2.Distance(startPosition, endPosition) > 50 && !moving){
						Vector2 movVect = new Vector2 (endPosition.x-startPosition.x, endPosition.y-startPosition.y);
						StartCoroutine ("moveToward", movVect);
					}
					break;
			}
		}
		if(Input.GetKeyDown(KeyCode.Escape))SceneManager.LoadScene("Menu");
	}

	//déselection du batiment selectionné
	public void deselection(){
		foreach (Transform child in selectionTiles){
			Destroy(child.gameObject);
		}
		if(selectedTile!=null )selectedTile.GetComponent<Building> ().deselection ();
		selectedTile = null;
		if (OnDeselectBuildingEvent != null)
		{
			OnDeselectBuildingEvent();
		}
		//UIManager.instance.getCostsBar().updateDisplay ();
	}

	//déplacement vers l'endroit indiqué, utilisé par les inputs PC
	void move(Vector2 direction){
		transform.position += new Vector3(direction.x, direction.y,0) * speed * Time.deltaTime;
		if (direction.x == 0f && direction.y == 0f) {
			sprite.GetComponent<Animator> ().SetBool ("moving", false);
			return;
		}
		sprite.GetComponent<Animator> ().SetBool ("moving", true);
		rotateToTarget (new Vector2(direction.x+transform.position.x, direction.y+transform.position.y));
		endPlaceBuilding ();
	}

	//déplacement tout droit dans la direction indiquée, utilisé par les inputs mobile
	IEnumerator moveToward(Vector2 direction){
		moving = true;
		sprite.GetComponent<Animator> ().SetBool ("moving", true);
		Vector2 targetPos = new Vector2(0f,0f);
		if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) { //look for the movement axe
			if (direction.x>=0)
				targetPos = new Vector2 (1 + Mathf.Round (transform.position.x), Mathf.Round (transform.position.y));
			else targetPos = new Vector2 (-1 + Mathf.Round (transform.position.x), Mathf.Round (transform.position.y));
		} else {
			if (direction.y>=0)
				targetPos = new Vector2 (Mathf.Round (transform.position.x), 1 + Mathf.Round (transform.position.y));
			else targetPos = new Vector2 (Mathf.Round (transform.position.x), -1 + Mathf.Round (transform.position.y));
		}
		GameObject targetGo = MapManager.instance.getObjectAt ((int)targetPos.x, (int)targetPos.y);
		if ( (targetGo == null && MapManager.instance.positionIsInsideMap((int)targetPos.x, (int)targetPos.y) ) 
			|| ( targetGo!=null && (targetGo.tag!="Destructible" && targetGo.tag!="Building")) ) { //check if destination is empty
			while (Vector2.Distance (transform.position, targetPos) > .0001) { //while the destination isn't reached
				Vector3 newPos=Vector3.MoveTowards (transform.position, targetPos, speed * Time.deltaTime);
				rotateToTarget(new Vector2(newPos.x, newPos.y));
				transform.position = newPos;
				endPlaceBuilding ();
				yield return null;
			}
		}
		sprite.GetComponent<Animator> ().SetBool ("moving", false);
		moving = false;
		yield return null;
	}

	//interraction avec l'objet sur lequel l'utilisateur clique
	void interract(Vector2 screenPosition){
		RaycastHit2D hit = Physics2D.Raycast(screenPosition, Vector2.zero);
		if(hit.transform!=null && hit.transform.gameObject.layer==5) return; //use to prevent from clickink through UI
		StopCoroutine ("move");
		deselection ();
		//rotateToTarget(screenPosition);
		if (hit.collider != null) {
			playerGridPos = new Vector2 (Mathf.Round (transform.position.x), Mathf.Round (transform.position.y));
			Vector2 hitGridPos = new Vector2 (Mathf.Round(hit.collider.transform.position.x), Mathf.Round(hit.collider.transform.position.y));

			if (hit.collider.tag == "Destructible"//check if a destructible is hit
			    && Vector2.Distance (hit.collider.transform.position, gameObject.transform.position) <= maxInteractionDistance) { //check if the distance between the play and the object is correct
				if (hit.collider.gameObject.GetComponent<OreVein> () != null) {
					OreVein oreVein = hit.collider.gameObject.GetComponent<OreVein> ();
					oreVein.damage (digDamages [oreVein.getType ()]);
				} else
					hit.collider.gameObject.GetComponent<Destructible> ().damage (digDamagesToRocks);
				rotateToTarget (hit.collider.transform.position);
				audioSource.PlayOneShot (digSound);
				endPlaceBuilding ();
			} else if ((hit.collider.tag == "Floor" && placingObject)//building construction
			           && Vector2.Distance (hit.collider.transform.position, gameObject.transform.position) <= maxConstructionDistance//check if the distance between the play and the object is correct
			           && (Mathf.Abs (playerGridPos.x - hitGridPos.x) > 0.01f || Mathf.Abs (playerGridPos.y - hitGridPos.y) > 0.01f)) { //check if the selected case is not player's case
				if (enoughRessources (buildingToPlace.GetComponent<Building>().getCosts()))
					mapManager.placeBuilding (buildingToPlace, hit.collider.transform.position);
				else PopupMessage.instance.ShowMessage("Not enough ressources");
				//endPlaceBuilding ();
			} 
			else if ((hit.collider.tag == "Item") //item collection
			       && Vector2.Distance (hit.collider.transform.position, gameObject.transform.position) <= maxInteractionDistance) { //check if the distance between the play and the object is correct
				if(hit.collider.gameObject.GetComponent<Item> ().collect ())
				audioSource.PlayOneShot (collectSound);
			} 
			else if (hit.collider.tag == "Building" && !placingObject){ //building selection
				selectedTile = hit.collider.gameObject;
				buildingToPlace = null;
				if (itemSlot != null && itemSlot.GetComponent<UsableItem> () != null
				   && selectedTile.GetComponent<Building> ().hasUpgrade (itemSlot.GetComponent<UsableItem> ().type)) {
					buildingToPlace = selectedTile;
				}
				//UIManager.instance.getCostsBar().updateDisplay ();
				selectedTile.GetComponent<Building> ().selection ();
				if (OnSelectBuildingEvent != null)
				{
					OnSelectBuildingEvent();
				}
			}
			else
				Debug.Log ("Rien touché");
			if(placingObject)endPlaceBuilding ();
		}
	}
	public Dictionary<string, int> getRessources(){
		return ressources;
	}

	public Dictionary<string, int> getDigDamages(){
		return digDamages;
	}

	public int getDigDamagesToRocks(){
		return digDamagesToRocks;
	}

	public GameObject getSelectedTile(){
		return selectedTile;
	}

	public GameObject getBuildingToPlace(){
		return buildingToPlace;
	}

	public void setBuildingToPlace(GameObject go){
		buildingToPlace = go;
	}

	public void placeBuilding(GameObject building){
		StopCoroutine ("move");
		if (buildingToPlace!=null && building.GetInstanceID () == buildingToPlace.GetInstanceID ())
			endPlaceBuilding ();
		else {
			if(selectedTile!=null )selectedTile.GetComponent<Building> ().deselection ();
			deselection ();
			placingObject = true;
			buildingToPlace = building;
			//UIManager.instance.getCostsBar().updateDisplay ();
			for (int x = -Mathf.RoundToInt(maxConstructionDistance); x <= Mathf.RoundToInt(maxConstructionDistance); x++) {
				for (int y = -Mathf.RoundToInt(maxConstructionDistance); y <= Mathf.RoundToInt(maxConstructionDistance); y++) {
					int checkX = Mathf.RoundToInt(transform.position.x) + x;
					int checkY = Mathf.RoundToInt(transform.position.y) + y;
					Vector2 checkPosition = new Vector2 (checkX, checkY);
					if (x == 0 && y == 0)
						continue;
					GameObject goAtPosition = mapManager.getObjectAt (checkX, checkY);
					if (Vector2.Distance (checkPosition, transform.position) <= maxConstructionDistance 
						&& mapManager.positionIsInsideMap(checkX, checkY) && goAtPosition==null ) {
						GameObject mask = Instantiate (colorMask, new Vector3 (checkX, checkY, -1), Quaternion.identity) as GameObject;
						mask.transform.SetParent (selectionTiles);
					}
				}
			}

		}
		if (OnPlaceBuildingEvent != null)
		{
			OnPlaceBuildingEvent();
		}

	}
	public void endPlaceBuilding(){
		buildingToPlace = null;
		placingObject = false;
		deselection ();
	}

	public bool enoughRessources(RessourcesManager.SerializedRessource[] costs){
		foreach (RessourcesManager.SerializedRessource r in costs) {
			if (r.value > ressources [r.name]) {
				Debug.Log ("not enough ressources");
				//StartCoroutine (PopupMessage.instance.ShowMessage("Not enough ressources", 2f));
				return false;
			}
		}
		return true;
	}

	public bool enoughRessource(string type, int cost){
		if (cost > ressources [type]) {
			Debug.Log ("not enough ressources");
			//StartCoroutine (PopupMessage.instance.ShowMessage("Not enough ressources", 2f));
			return false;
		}
		return true;
	}

	public override void damage(int loss){
		hp -= loss;
		Instantiate (damageParticles, transform.position, Quaternion.identity);
		if (hp <= 0){
			hp = 0;
			if (OnDiedEvent != null)
			{
				OnDiedEvent();
			}
		}
		if (OnHPChangeEvent != null)
		{
			OnHPChangeEvent();
		}
	}
		
	public bool isDead(){
		return hp <= 0;
	}

	public int getHp(){
		return hp;
	}

	public Camera getCamera(){
		return playerCamera;
	}
	private void OnCollisionStay2D(Collision2D collision){
		//destination = transform.position;
	}
	public GameObject takeItem(GameObject item){
		if (item.GetComponent<UsableItem> () != null && itemSlot == null) {
			itemSlot = item;
			UIManager.instance.getItemSlot().setIcon (item);
			return null;
		} else if (item.GetComponent<UsableItem> () != null) { //if player already have an object, replace him and return the old object
			GameObject tempSlot = itemSlot;
			itemSlot = item;
			UIManager.instance.getItemSlot().setIcon (item);
			return tempSlot; //return old object to change his position
		}
		return itemSlot;
	}
	public GameObject getItemInSlot(){
		return itemSlot;
	}
	public void setItemInSlot(GameObject item){
		itemSlot=item;
	}

	public void gainRessource(string type, int value){
		ressources [type] += value;
		//call the event (Used for UI)
		if (OnRessourcesChangeEvent != null)
		{
			OnRessourcesChangeEvent();
		}
	}

	public void loseRessource(string type, int value){
		ressources [type] -= value;
		//call the event (Used for UI)
		if (OnRessourcesChangeEvent != null)
		{
			OnRessourcesChangeEvent();
		}
	}

	public void upgradeDigDamage(string type, int value){
		digDamages [type] += value;

	}


}
