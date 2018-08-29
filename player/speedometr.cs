using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class speedometr : MonoBehaviour {
	[SerializeField]
	private GameObject player;
	float speed;
	private Text speedometerText;

	// Use this for initialization
	void Start () {
		speedometerText = GetComponent<Text> ();
		
//		speed = player.GetComponent<ShipMotor> ().thisShip.p.speed;

	}
	
	// Update is called once per frame
	void Update () {
		SO_ship playerSO = player.GetComponent<ShipMotor> ().thisShip;
		speedometerText.text = "s "+ playerSO.p.shield.ToString().Substring(0,3)+"\n"+"c "+playerSO.p.capasitor.ToString().Substring(0,3) ;

	}
}
