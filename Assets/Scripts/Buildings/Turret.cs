using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : PoweredBuilding {
	public Transform spriteTransform;
	protected GameObject target;
	public GameObject projectile;
	public GameObject shotParticles;
	public float attackSpeed = 1.5f;
	public float rotationSpeed = 100f;
	protected Transform firePoint;

	protected float nextShot;
	protected float time;
	// Use this for initialization
	protected override void Awake () {
		base.Awake ();
		if (spriteTransform == null)
			spriteTransform = transform;
		firePoint = spriteTransform.Find ("FirePoint");
	}

	// Update is called once per frame
	void Update () {
		//timeCalculation();
		time=GameManager.instance.getTime();
		target = findClosestEnemy();
		if (target != null && power>=consumedPower) {
			rotate ();
			attack ();
		}
	}

	protected virtual void attack(){
		if (nextShot <= time) {
			Debug.DrawRay (spriteTransform.position, target.transform.position, Color.red);
			Vector3 rot = spriteTransform.rotation.eulerAngles;
			rot = new Vector3 (rot.x, rot.y, rot.z +90);// new rotation for the projectile to be shot in front.
			GameObject proj=Instantiate (projectile, new Vector3(firePoint.transform.position.x, firePoint.transform.position.y, projectile.transform.position.z), Quaternion.Euler (rot));
			if (proj.GetComponent<Projectile> ().getVelocity () <= 0f) {
				proj.transform.SetParent (firePoint.transform);
				proj.transform.localPosition = new Vector3(0,0,0);
			}

			if(shotParticles!=null)Instantiate (shotParticles, firePoint.position,  Quaternion.Euler (rot));
			power-=consumedPower;
			nextShot = time + attackSpeed;
		}
	}
	protected void rotate(){
		Vector3 vectorToTarget = target.transform.position - spriteTransform.position;
		float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
		Quaternion q = Quaternion.AngleAxis(angle-90, Vector3.forward);
		spriteTransform.rotation = Quaternion.RotateTowards(spriteTransform.rotation, q, Time.deltaTime * rotationSpeed);
	}
	protected GameObject findClosestEnemy(){
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		GameObject closest = null;
		float distance = Mathf.Infinity;
		Vector2 position = spriteTransform.position;
		foreach (GameObject go in enemies){
			Vector2 diff = (Vector2)go.transform.position - position;
			float curDistance = diff.sqrMagnitude;
			if (curDistance < distance){
				closest = go;
				distance = curDistance;
			}
		}
		return closest;
	}
		
}
