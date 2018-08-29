using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceParticles : MonoBehaviour {
	[SerializeField]
	private GameObject player;
	float speed;
	ParticleSystem ps;


	void Start () {
		speed = player.GetComponent<ShipMotor> ().thisShip.p.SO.speed;
		ps=GetComponent<ParticleSystem> ();
		var em = ps.emission;
		em.enabled = false;
		
	}

	void ChangeParticlesBehaviour(){
//		var ps = GetComponent<ParticleSystem>();
		var main = ps.main;
		var em = ps.emission;

		if (speed ==0) {
			em.enabled = false;
		}
		else
		{
			em.enabled = true;
			main.startSpeedMultiplier = player.GetComponent<ShipMotor>().thisShip.p.SO.speed/3;
			Vector3 vel_vector = transform.forward;
			Vector3 pos = transform.position;
			ps.transform.position = transform.position+(transform.forward * 22);
			ps.transform.LookAt(transform.position);

		}

	}

}
