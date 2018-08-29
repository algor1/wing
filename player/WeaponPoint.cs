using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPoint : MonoBehaviour {

	void Start()
	{
		gameObject.AddComponent<LaserBeam>();
	}


	public void StartFire (SO_ship target)
	{
		gameObject.GetComponent<LaserBeam>().StartFire(target);
		Debug.Log ("+++++pew to " + target.p.SO.visibleName);
	}
	public void StopFire ()
	{
		Debug.Log ("++++stop pew  ");

		gameObject.GetComponent<LaserBeam>().StopFire();
	}
}
