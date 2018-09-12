using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour {
    private IEnumerator coroutineLife;
	float speed;
	float lifetime;
	float rotationSpeed;
	public GameObject target;


	// Use this for initialization
	void Start () {
		speed = 10;
		lifetime = 4;
		rotationSpeed = 180;
        coroutineLife = LifeTimeCount();
        StartCoroutine(coroutineLife);
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 target_vector = target.GetComponent<Transform> ().position - transform.position;
		var q = Quaternion.LookRotation(target_vector);
		transform.rotation = Quaternion.RotateTowards(transform.rotation, q, rotationSpeed * Time.deltaTime);
		transform.position += transform.forward * Time.deltaTime * speed;
		if (Vector3.SqrMagnitude (target_vector) < 1 && GetComponent<MeshRenderer>().enabled) {
			speed = 0;
            StopCoroutine(coroutineLife);
			Explode ();
		}
		
	}
	private void Explode(){
		Debug.Log ("boom");
        
		GetComponent<Detonator> ().Explode ();
		GetComponent<MeshRenderer> ().enabled = false;

		foreach (Transform child in transform)
		{
			if (child.name=="fire"|| child.name== "smoke")
			GameObject.Destroy(child.gameObject);
		}
	}
    private IEnumerator LifeTimeCount()
    {
        yield return new WaitForSeconds(lifetime);
        Explode();
    }
}
