using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Destructible : MonoBehaviour {
	public GameObject damageParticles;
	public int hp;
	public abstract void damage (int loss);
}
