using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Server : MonoBehaviour {
   
    [SerializeField]
	private GameObject serverLand;

    [SerializeField]
	private ShipList_scrob shipsDB;

	private List<SO_ship> ships;

    public enum Command { MoveTo,WarpTo,Atack,SetTarget,LandTo,Equipment};

//	[SerializeField]
//	private Vector3 p1;
//	[SerializeField]
//	private Vector3 p2;
    public enum ShipEvenentsType{spawn,warp,warmwarp,move,stop,land,hide,reveal};



	void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);

//        LoadShips();
		ships = new List<SO_ship> ();
		Debug.Log ("all ship" + shipsDB.shipList.Count);
		for (int i = 0; i < shipsDB.shipList.Count; i++)
		{
			SO_ship s = new SO_ship (shipsDB.shipList [i], this.gameObject);
			ships.Add(s);
			//            ships = shipsDB.shipList;
		}
    }

	void Start(){
		ships [1].SetTarget (ships [0]);
		ships [1].GoToTarget();
	
    }


	void FixedUpdate(){
		Tick();	
//		p1 = ships [0].p.SO.position;
//		p2 = ships [1].p.SO.position;

	}

		


	public List<SO_ship> Nearest (int player_id){
		SO_ship pl = GetPlayer (player_id);
		List<SO_ship> resultShipList = new List<SO_ship> ();
		for (int i = 0; i < ships.Count; i++){
			if (ships[i].p.SO.id == player_id || ships[i].p.hidden) continue;//remove player from list

			float dist = Vector3.Distance (pl.p.SO.position, ships [i].p.SO.position);
//			print (dist);
			if (dist<pl.p.vision_distance){ 
				resultShipList.Add (ships [i]);
			}
			if (dist<ships[i].p.vision_distance&&!ships[i].atack){ 
				ships [i].SetTarget(pl);//playerid=0
			}
		}
//		if (resultShipList.Count>0)
//		Debug.Log ("nearest "+resultShipList.Count);

		return resultShipList;
	}

	public SO_ship GetPlayer(int player_id){
		for (int i = 0; i < ships.Count; i++){
			if (ships [i].p.SO.id== player_id) return ships[i];
		}
		return ships [0];

	}

    private void LoadShips()
    {
		ships = new List<SO_ship> ();
		Debug.Log ("all ship" + shipsDB.shipList.Count);
        for (int i = 0; i < shipsDB.shipList.Count; i++)
        {
			SO_ship s = new SO_ship (shipsDB.shipList [i], this.gameObject);
            ships.Add(s);
//            ships = shipsDB.shipList;
        }
    }

    public int RegisterPlayer(){
        return 0;
    }

  


	public void PlayerControl(int player_id,Command player_command,int weapon_equip_num ) {
		SO_ship player = GetPlayer(player_id);
        switch(player_command){
            case Command.MoveTo:
			    player.GoToTarget();
                break;
		    case Command.WarpTo:		
			    player.WarpToTarget();
                break;
            case Command.Atack:
			player.Atack_target(weapon_equip_num);
                break;
            case Command.LandTo:
                player.LandToTarget();
                break;
            case Command.Equipment:
                player.StartEquipment();
                break;
        }
	}

	public void PlayerControlSetTargetShip(int player_id,Command player_command ,SO_ship target) {
		SO_ship player = GetPlayer(player_id);
		SO_ship tg = GetPlayer(target.p.SO.id);
		if (player_command==Command.SetTarget){
			player.SetTarget(tg);
		}

	}
	public void PlayerControlSetTarget(int player_id, Command player_command, ServerObject target)
	{
		SO_ship player = GetPlayer(player_id);
		if (player_command == Command.SetTarget)
		{
			player.SetTarget(target);
		}

	}


    private void Warp(SO_ship ship)
    {
        if (ship.moveCommand == SO_ship.MoveType.warp)
        {
            if (ship.Rotate())
            {
                if (!ship.warpCoroutineStarted)
                {
					StartCoroutine(ship.Warpdrive());
                }
                //else
                //{
                //    ship.p.SO.position = Vector3.MoveTowards (ship.p.SO.position,ship.p. Time.deltaTime * ship.p.warpSpeed;
                //}
            }
        }
    }
    public void Events(SO_ship.ShipEvenentsType shipEvent,SO_ship ship)
    {
        switch (shipEvent)
        {
            case SO_ship.ShipEvenentsType.stop:
//                Debug.Log("server" + ship.p.SO.visibleName + " ship stopped");
                
                break;
            case SO_ship.ShipEvenentsType.move:
                Debug.Log("server" + ship.p.SO.visibleName + " ship accelerated");
                break;
            case SO_ship.ShipEvenentsType.warmwarp:
                Debug.Log("server" + ship.p.SO.visibleName + " ship preparing to warp");
                break;
            case SO_ship.ShipEvenentsType.warp:
                Debug.Log("server" + ship.p.SO.visibleName + " ship warping....");
                
                break;
            case SO_ship.ShipEvenentsType.spawn:
                Debug.Log("server" + ship.p.SO.visibleName + " ship spawn");
                break;
		case SO_ship.ShipEvenentsType.land:
//			Debug.Log ("server" + ship.p.SO.visibleName + " ship landing " + ship.targetToMove.id);
//			GetComponent<SkillsDB> ().GetAllSkills ();
			GetComponent<LandingServer>().Landing( ship.targetToMove.id,ship.p.SO.id);
                ship.landed=true;



                break;
        }
    }

    private void Atack(SO_ship ship)
    {
        if (ship.atack)
        {
            for (int i = 0; i < ship.weapons.Count; i++)
            {
                if (ship.weapons[i].fire && !ship.weapons[i].p.active)
					
                {
//					Debug.Log (ship.weapons [i].GetHashCode ());
                    StartCoroutine(ship.weapons[i].Attack());
                }

            }
        }
    }
    private void Equipment(SO_ship ship){
        for (int i = 0; i < ship.equipments.Count; i++){
            if (ship.equipments[i].activate&&!ship.equipments[i].coroutineStarted)
            {
                StartCoroutine(ship.equipments[i].UseEq());
            }
        }
    }
	private void Tick(){
		for (int i = 0; i < ships.Count; i++){
			ships[i].Tick();
            Warp(ships[i]);
            Atack(ships[i]);
            Equipment(ships[i]);
        }
	}

  //private void RemoveDestroyed(){
    //    for (int i = 0; i < ships.Count; i++)
    //    {
    //        if (ships[i].destroyed)
    //        {
    //            if (FO_ships.ContainsKey(ships[i].id))
    //            {
    //                FO_ships.Remove(ships[i].id);
    //            }
    //            ships.RemoveAt(i);
    //        }
    //    }

    //}

    //public void DamageShip(int SO_id, float damage)

    //{
    //    ships[SO_id].Damage(damage);
    //}
}
