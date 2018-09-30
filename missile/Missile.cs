﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour {
    private IEnumerator coroutineLife;
	float speed;
	float lifetime;
	float rotationSpeed;
	public GameObject target;
	private bool destroed;


	// Use this for initialization
	void Start () {
		speed = 10;
		lifetime = 40;
		rotationSpeed = 180;
        coroutineLife = LifeTimeCount();
		destroed = false;
        StartCoroutine(coroutineLife);
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 target_vector = target.GetComponent<Transform> ().position - transform.position;
		var q = Quaternion.LookRotation(target_vector);
		transform.rotation = Quaternion.RotateTowards(transform.rotation, q, rotationSpeed * Time.deltaTime);
		transform.position += transform.forward * Time.deltaTime * speed;
		if (Vector3.SqrMagnitude (target_vector) < 1 && !destroed) {
			Debug.Log ("magnotude " + Vector3.SqrMagnitude (target_vector));
			speed = 0;
            StopCoroutine(coroutineLife);
			Explode ();
		}
		
	}
	private void Explode(){
		Debug.Log ("boom");
        
		GetComponent<Detonator> ().Explode ();
		destroed = true;
//		GetComponent<MeshRenderer> ().enabled = false;

		foreach (Transform child in transform)
		{
			if (child.name == "fire" || child.name == "smoke" || child.name.Substring (0, 3) == "Mis") {	
				Debug.Log (child.name);
				GameObject.Destroy (child.gameObject);
			}
		}
	}
    private IEnumerator LifeTimeCount()
    {
        yield return new WaitForSeconds(lifetime);
        Explode();
    }
}