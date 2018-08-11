using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
	
	public float lifeTime = 3f;
	public GameObject hitParticles;
	public bool destroyOnCollision;
	public int damage = 2;
	public int targets;
	public float velocity = 300f;

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<Rigidbody2D>().AddForce(transform.right*velocity);
		Destroy(gameObject, lifeTime);
	}
		
	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "Enemy" && targets>0){
			col.gameObject.GetComponent<Enemy>().damage(damage);
			if(hitParticles!=null)Instantiate (hitParticles, col.transform.position,Quaternion.identity);
			if(destroyOnCollision)Destroy(gameObject);
			targets--;
		}
	}
	public float getVelocity(){
		return velocity;
	}

}