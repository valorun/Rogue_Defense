using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualTurret : Turret {
	Transform firePoint1;
	// Use this for initialization
	protected override void Awake () {
		base.Awake ();
		firePoint = transform.Find ("FirePoint");
		firePoint1 = transform.Find ("FirePoint1");
	}
	
	// Update is called once per frame
	void Update () {
		time=GameManager.instance.getTime();

		target = findClosestEnemy();
		if (target != null && power>=consumedPower) {
			rotate ();
			attack ();
		}
	}
	protected override void attack(){
		if (nextShot <= time) {
			Debug.DrawRay (spriteTransform.position, target.transform.position, Color.red);
			Vector3 rot = spriteTransform.rotation.eulerAngles;
			rot = new Vector3 (rot.x, rot.y, rot.z + 90);// new rotation for the projectile to be shot in front.
			Instantiate (projectile, new Vector3(firePoint.transform.position.x, firePoint.transform.position.y, projectile.transform.position.z), Quaternion.Euler (rot));
			Instantiate (shotParticles, firePoint.position,  Quaternion.Euler (rot));
			Instantiate (projectile, new Vector3(firePoint1.transform.position.x, firePoint1.transform.position.y, projectile.transform.position.z), Quaternion.Euler (rot));
			Instantiate (shotParticles, firePoint1.position,  Quaternion.Euler (rot));
			power-=consumedPower;
			nextShot = time + attackSpeed;
		}
	}
}
