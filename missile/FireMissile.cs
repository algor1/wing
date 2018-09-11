using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMissile : MonoBehaviour {
	
	public GameObject missilePrefab;
	public GameObject target;

	public void Fire(){
//		GameObject target = GameObject.Find ("Cube");
		GameObject missile=(GameObject)Instantiate(missilePrefab,transform.position,transform.rotation);
		missile.GetComponent<Missile>().target=target;
	}
}