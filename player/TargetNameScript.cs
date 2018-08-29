using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TargetNameScript : MonoBehaviour {
	private Text tname;
	public GameObject player;

	// Use this for initialization
	void Start () {
		tname= GetComponent<Text> ();
		tname.text = "no target";
	}
	
	// Update is called once per frame
	void Update () {
		if (player.GetComponent<ShipMotor> ().thisShip.newtargetToMove!=null) {
			tname.text = player.GetComponent<ShipMotor> ().thisShip.newtargetToMove.visibleName;
//			print (player.GetComponent<ShipMotor> ().thisShip.newtargetToMove.visibleName);
		} else {
			tname.text = "no target";
		}
	}
}