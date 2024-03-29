﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]

public class ShipMotor : MonoBehaviour {

	[SerializeField]
	private GameObject dataLocal;
	[SerializeField]
	private GameObject Server_GO;
	public SO_ship thisShip;

	//	private Rigidbody rb;
//	private ParticleSystem ps;
//	private Vector3 _rotation;
//	private Quaternion _rotationQ;
//	private Quaternion rotationTarget;
//	private Quaternion oldTransformRotation;


	private bool _flagWarp;
	public float _warpPower;
	public float back_thrust;


	public float thrust;

	[SerializeField]
	private GameObject burner;	
	[SerializeField]
	private Camera mainCam;
	[SerializeField]
	private float rotationSpeed;
	private float rotationRoll;
	private float rotationTime;
	private float rotationStartedTime;
	private int leftOrRightTurn;
	private bool rotated;
	public GameObject selectedTarget; //temp for waypoints
	public bool _warpStarted;

	[SerializeField]
	private List<GameObject> weaponPoints;


	[SerializeField]
	private float maxWarpSpeed;

	public enum ShipEvenentsType{spawn,warp,warmwarp,move,stop,land,hide,reveal};



	void Start(){
		
//		thisShip=Server_GO.GetComponent<Server>().GetPlayer(0);
//		shipName = thisShip.p.visibleName;
//        ps=particalSys.GetComponent<ParticleSystem> ();
//		var em = ps.emission;
//		em.enabled = false;
//		SetSpeedometr ();
//		rotationTarget = transform.rotation;
		Server_GO = GameObject.Find ("ServerGo");


	}

	public void Init(SO_ship _ship, GameObject _server, GameObject _datalocal)
	{

		thisShip = new SO_ship(_ship.p,this.gameObject);
		Debug.Log (this);
		if (thisShip.weapons.Count<=weaponPoints.Count)
		{
			for (int i =0 ;i < thisShip.weapons.Count ;i++){
				thisShip.weapons[i].SetWeaponPoint(weaponPoints[i]);
				weaponPoints [i].GetComponent<WeaponPoint> ().Init (thisShip.weapons [i].p.type);
				Debug.Log (thisShip.p.SO.visibleName+ " init weapon point "+ thisShip.weapons [i].p.type + i);
		}
		}
		Server_GO = _server;
		dataLocal = _datalocal;
		Debug.Log ("----init   " + thisShip.p.SO.visibleName+"  id   " +thisShip.p.SO.id);
	}
	//удалить в продакшене
//	private void SetSpeedometr(){
//		speedometerText.text = thisShip.p.speed.ToString();
//	}


//	public bool Rotate (Quaternion rotationQ)
//	{
//		
//		if (rotationTarget != rotationQ) {
//
//			rotationTarget = rotationQ;
//			rotationTime = Quaternion.Angle(transform.rotation, rotationTarget) / rotationSpeed;
//			rotationStartedTime = Time.time;
//			leftOrRightTurn=( Mathf.Sin(Mathf.Deg2Rad*transform.InverseTransformVector(rotationTarget.eulerAngles).y)>0)? 1:-1;
////			print (leftOrRightTurn);
////			print( Mathf.Sin(Mathf.Deg2Rad*transform.InverseTransformVector(rotationTarget.eulerAngles).y));
////			print ((transform.rotation.eulerAngles.y - rotationTarget.eulerAngles.y));
//			rotated=false;
//
//		} 
//		return rotated;
//	}

	public void Thrust (float _thrust)
	{
		thisShip.p.newSpeed = thisShip.p.max_speed*_thrust;
	}

//	public void Warp (float warpPower)
//	{
//		_warpPower=warpPower;
//	}

	void FixedUpdate () {
		thisShip.Tick ();
		Warp();
		Atack();
        Equipment();
		PerformRotateQ ();
//		PerformAccelerate ();
		PerformMove ();
		//		PerformWarp ();
		//		PerformStop ();
//		ChangeParticlesBehaviour ();
		//		CutSpeed ();
	}

//	void Update(){
//		SetSpeedometr ();
//	}

	void PerformRotateQ(){
		transform.rotation = thisShip.p.SO.rotation;
//		if (Mathf.Abs (transform.rotation.eulerAngles.x - rotationTarget.eulerAngles.x) > 1 || Mathf.Abs (transform.rotation.eulerAngles.y - rotationTarget.eulerAngles.y) > 1) {
//			float roll = (rotationTime * 3 / 4 < (Time.time - rotationStartedTime)) ? rotationSpeed / 4 : -rotationSpeed * 3 / 4;
//			transform.rotation = Quaternion.RotateTowards (transform.rotation, rotationTarget, rotationSpeed * Time.deltaTime);
//			transform.RotateAround (transform.position, transform.forward, Time.deltaTime * roll * leftOrRightTurn);
//		} else {
//			rotated = true;
//		}

	}

	void PerformMove(){
		transform.position = thisShip.p.SO.position-dataLocal.GetComponent<ShowEnv>().GetZeroPoint();
	}




	public void SetTarget(ServerObject tg)
	{
		//SO_ship tg = Server_GO.GetComponent<Server> ().GetPlayer (1);
		//		thisShip.SetTarget (tg);

		thisShip.SetTarget(tg);
		Server_GO.GetComponent<Server>().PlayerControlSetTarget(thisShip.p.SO.id, Server.Command.SetTarget, tg);

	}
//	public void SetTarget(SO_ship tg)
//	{
//		//SO_ship tg = Server_GO.GetComponent<Server> ().GetPlayer (1);
//		//		thisShip.SetTarget (tg);
//
//		thisShip.SetTarget(tg);
//		Server_GO.GetComponent<Server>().PlayerControlSetTargetShip(thisShip.p.SO.id, Server.Command.SetTarget, tg);
//
//	}

	public void GoTotarget()
    {
//		SO_ship tg = Server_GO.GetComponent<Server> ().GetPlayer (1);
//		thisShip.SetTarget (tg);
//		Server_GO.GetComponent<Server> ().PlayerControl(0,Server.Command.SetTarget,tg);

		thisShip.GoToTarget ();
		Server_GO.GetComponent<Server> ().PlayerControl(0,Server.Command.MoveTo,0);

//        se
//        _goToTarget = true;
    }
    public void WarpTotarget()

    {
//		SO_ship tg = Server_GO.GetComponent<Server> ().GetPlayer (1);
//		thisShip.SetTarget (tg);
//		Server_GO.GetComponent<Server> ().PlayerControl(0,Server.Command.SetTarget,tg);

		thisShip.WarpToTarget ();
		Server_GO.GetComponent<Server> ().PlayerControl(0,Server.Command.WarpTo,0);
//		Spawn ();
//        _warpToTarget = true;
    }

    public void StartEquipment(int weaponnum)
    {
        thisShip.StartEquipment();
		Server_GO.GetComponent<Server>().PlayerControl(0, Server.Command.Equipment,weaponnum);
    }
	public void AtackTarget(int weaponnum)
	{
		thisShip.Atack_target(weaponnum);
		Server_GO.GetComponent<Server>().PlayerControl(0, Server.Command.Atack,weaponnum);
	}

    public void LandTotarget()
    {
        //		SO_ship tg = Server_GO.GetComponent<Server> ().GetPlayer (1);
        //		thisShip.SetTarget (tg);
        //		Server_GO.GetComponent<Server> ().PlayerControl(0,Server.Command.SetTarget,tg);

		Server_GO.GetComponent<Server> ().PlayerControl (0, Server.Command.LandTo,0);
//        {
//            Debug.Log("landed");
//        }
        //		Spawn ();
        //        _warpToTarget = true;
    }
	public void OpenTarget()
	{
		//		SO_ship tg = Server_GO.GetComponent<Server> ().GetPlayer (1);
		//		thisShip.SetTarget (tg);
		//		Server_GO.GetComponent<Server> ().PlayerControl(0,Server.Command.SetTarget,tg);

		Server_GO.GetComponent<Server> ().PlayerControl (0, Server.Command.Open,0);
		thisShip.OpenTarget ();
		//        {
		//            Debug.Log("landed");
		//        }
		//		Spawn ();
		//        _warpToTarget = true;
	}

	private void Spawn()//backlink from SO_ship
	{
		//		set zero
		Debug.Log("set zeropoint");
		Debug.Log("zeropoint was  " +dataLocal.GetComponent<ShowEnv>().GetZeroPoint());
		dataLocal.GetComponent<ShowEnv>().SetZeroPoint(thisShip.p.SO.position);
		Debug.Log("zeropoint now  " +dataLocal.GetComponent<ShowEnv>().GetZeroPoint());
		Debug.Log ("move ship To 0,0,0");

		transform.position = transform.position * 0;

	}

	private void Warp()
	{
		if ( thisShip.moveCommand == SO_ship.MoveType.warp)
		{
			if (thisShip.Rotate())
			{
				if (!thisShip.warpCoroutineStarted)
				{
					StartCoroutine(thisShip.Warpdrive());
				}
				//else
				//{
				//    ship.p.SO.position = Vector3.MoveTowards (ship.p.SO.position,ship.p. Time.deltaTime * ship.p.warpSpeed;
				//}
			}
		}
	}

	private void Atack()
	{
//		if (thisShip.atack)
//		{
			
			for (int i = 0; i < thisShip.weapons.Count; i++)
			{
//				Debug.Log ("atack  " + thisShip.p.id + "  weap  " +i + " fire " + thisShip.weapons[i].fire + "  "+ thisShip.weapons[i].p.active);
//				Debug.Log ("M" +thisShip.weapons [i]);

			if (thisShip.weapons[i].fire && !thisShip.weapons[i].activated)
				{
					thisShip.weapons[i].atack_co= StartCoroutine(thisShip.weapons[i].Attack());
				Debug.Log ("*************      atack_co    " + thisShip.weapons [i].atack_co.GetHashCode());

				}
			}
//		}
	}
	private void StopAtacking()
	{
		for (int i = 0; i < thisShip.weapons.Count; i++)
		{
			if (thisShip.weapons[i].activated)
			{
				Debug.Log ("stopping atack couroutine id " + thisShip.weapons [i].atack_co.GetHashCode ());
				StopCoroutine (thisShip.weapons [i].atack_co);
			}
		}
	}

    private void Equipment()
    {
        for (int i = 0; i < thisShip.equipments.Count; i++)
        {
            if (thisShip.equipments[i].activate && !thisShip.equipments[i].coroutineStarted)
            {
				thisShip.equipments [i].use_co=StartCoroutine(thisShip.equipments[i].UseEq());
            }
        }
    }
	private void StopEquipments()
	{
		for (int i = 0; i < thisShip.equipments.Count; i++)
		{
			if (thisShip.equipments[i].coroutineStarted)
			{
				StopCoroutine (thisShip.equipments [i].use_co);
			}
		}
	}


    public void Events(SO_ship.ShipEvenentsType shipEvent, SO_ship ship)
	{
		if (ship == thisShip) {
			switch (shipEvent) {
			case SO_ship.ShipEvenentsType.stop:
				Debug.Log ("motor " + thisShip.p.SO.visibleName + " ship stopped");
				burner.GetComponent<BurnControl> ().EngineStop ();
				break;
			case SO_ship.ShipEvenentsType.move:
				Debug.Log ("motor " + thisShip.p.SO.visibleName + " ship accelerated");
				burner.GetComponent<BurnControl> ().EngineStart ();
				break;
			case SO_ship.ShipEvenentsType.warmwarp:
				Debug.Log ("motor " + thisShip.p.SO.visibleName + " ship preparing to warp");
				burner.GetComponent<BurnControl> ().WarpStart ();
				break;
			case SO_ship.ShipEvenentsType.warp:
			
				Debug.Log ("motor " + thisShip.p.SO.visibleName + " ship warping....");
//			if player
				dataLocal.GetComponent<Effects> ().PlayerWarp (thisShip.targetToMove.position, thisShip.p.warpSpeed);
				break;
			case SO_ship.ShipEvenentsType.spawn:
				Debug.Log ("motor " + thisShip.p.SO.visibleName + " ship spawn");
				dataLocal.GetComponent<Effects> ().PlayerWarpStop ();
				burner.GetComponent<BurnControl> ().WarpStop ();
				Spawn ();
				break;
			case SO_ship.ShipEvenentsType.land:
				Debug.Log ("motor " + thisShip.p.SO.visibleName + " ship landing");
				Server_GO.GetComponent<LandingServer> ().Landing (thisShip.p.SO.id, thisShip.targetToMove.id);
				thisShip.landed = true;
				break;
			case SO_ship.ShipEvenentsType.destroyed:
				Debug.Log ("motor " + thisShip.p.SO.visibleName + " ship destroyed");
				StopAtacking ();
				StopEquipments ();
//			thisShip = null;
				//var detonator = gameObject.AddComponent<Detonator> ();
				//detonator.duration = 4;
				//detonator.destroyTime = 4;
				//detonator.size = 40;
				//detonator.autoCreateSmoke = false;
				//detonator.autoCreateHeatwave = false;
				//detonator.color = Color.white;
				//detonator.destroyObj = false;
				//detonator.Explode ();
				thisShip.p.destroyed = true;
				Debug.Log ("********************       explosion");
				dataLocal.GetComponent<ShowEnv> ().DestroyShip (thisShip.p.SO.id);
				break;
			case SO_ship.ShipEvenentsType.open:
				GameObject canvasObj = GameObject.Find ("Canvas");
				canvasObj.GetComponent<ShowMenus> ().ShowInventory (thisShip.p.SO.id, thisShip.targetToMove.id);
				break;
			}
		}
	}

}	