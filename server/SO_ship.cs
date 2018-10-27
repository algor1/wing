using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SO_ship {


    public SO_shipData p; //ship properties	
	public GameObject host;
	
    public bool atack;
    private Quaternion rotationToTarget;

 	private ServerObject target;
	public ServerObject targetToMove;
	public ServerObject newtargetToMove;
//	private Quaternion oldRotation; //for roll calc

	private SO_ship targetToAtack;
	private SO_ship newtargetToAtack;
    public enum MoveType { move, warp, stop};
    public MoveType moveCommand;
    public enum ComandType { warpTo,goTo,landTo, none};
    public ComandType complexCommand;
    public enum ShipEvenentsType { spawn, warp, warmwarp, move, stop, land, hide, reveal, destroyed };
    public bool warpActivated;
    public bool warpCoroutineStarted;
    public bool landed;

    public List<SO_weapon> weapons;
	public List<SO_equipment> equipments;


	public SO_ship(SO_shipData shipData, GameObject _host){
		
		p=shipData;
		rotationToTarget = p.SO.rotation;
        moveCommand = MoveType.move;
		SendEvent(ShipEvenentsType.move);

        host = _host;
		Debug.Log (host);

		newtargetToMove = null;
		weapons= new List<SO_weapon>();
		for (int i = 0; i < shipData.weapons.Count; i++) {
			Debug.Log (p.SO.id+ " ---init  weapon  " + i+ "    "+ shipData.weapons[i].type+shipData.weapons.Count);
			SO_weapon newweappon = new SO_weapon (shipData.weapons [i], this);
			weapons.Add(newweappon);
		}
		equipments = new List<SO_equipment>();
		for (int i = 0; i < shipData.equipments.Count; i++)
		{
			Debug.Log(p.SO.id + " ---init  active equipment  " + i + "    " + shipData.equipments.Count);
			SO_equipment neweq = new SO_equipment(shipData.equipments[i], this);
			equipments.Add(neweq);
		}


	}
	
	public void SetTarget(ServerObject newtarget){
		Debug.Log ("new target  " + newtarget.visibleName);
		newtargetToMove = newtarget;
//		newtargetToAtack=null;
	}
	public void SetTarget(SO_ship newtarget){	
//		Debug.Log ("new target  " + newtarget.p.visibleName);
		newtargetToMove = newtarget.p.SO;
		newtargetToAtack=newtarget;
		Debug.Log (p.SO.visibleName +" p"+p.SO.position+ " set target to " + newtargetToAtack.p.SO.visibleName+" p "+newtargetToAtack.p.SO.position);
//		Debug.Log (newtargetToMove.position);

	}
	public void GoToTarget(){
		if (newtargetToMove != null) {
			
            complexCommand = ComandType.goTo;
			targetToMove = newtargetToMove;
//			oldRotation = p.SO.rotation;
        

		}

	}
	public void WarpToTarget(){
        if (newtargetToMove != null && Vector3.Distance(p.SO.position, newtargetToMove.position) > 100000)
        {
			Debug.Log ("warping");
			moveCommand = MoveType.warp;
            complexCommand = ComandType.warpTo;
			targetToMove = newtargetToMove;
//			oldRotation = p.SO.rotation;
            warpActivated = false;
        }
        else
        {
            Debug.Log("TO CLOSE TO WARP");
		}

	}
    public void StartEquipment()
    {
            for (int i = 0; i < equipments.Count; i++)
            {
                if (!equipments[i].activate)
                {
                    equipments[i].Use();
                }
                else
                {
                    equipments[i].Stop();
                }
            }
        
    }
	public void StopEquipment()
	{
		for (int i = 0; i < equipments.Count; i++)
		{
			equipments[i].Stop();

		}

	}
    
    public void LandToTarget()
    {
        if (newtargetToMove != null)
        {
            complexCommand = ComandType.landTo;
            targetToMove = newtargetToMove;
//            oldRotation = p.SO.rotation;
            //			Debug.Log (targetToMove.position);
        }
    }

	public bool Rotate(){
		if (targetToMove != null) {
//			Debug.Log ("id " + p.id + "targettomove " + targetToMove.position + "  p  " + p.SO.position);
			rotationToTarget = Quaternion.LookRotation (targetToMove.position - p.SO.position);
			Vector3 rt =  new Vector3 (rotationToTarget.eulerAngles.x, rotationToTarget.eulerAngles.y, p.SO.rotation.eulerAngles.z);
			rotationToTarget = Quaternion.Euler (rt);

			if (Mathf.Abs (p.SO.rotation.eulerAngles.x - rotationToTarget.eulerAngles.x) > 1 || Mathf.Abs (p.SO.rotation.eulerAngles.y - rotationToTarget.eulerAngles.y) > 1) {
//				Debug.Log ("id " + p.id + "  rotating " + p.rotation.eulerAngles + "  to  " + rotationToTarget.eulerAngles);
//				Debug.Log ("id " + p.id + "  position " + p.SO.position + "  target pos" + targetToMove.position);
//				Debug.Log("sin z " +  Mathf.Sin(Mathf.Deg2Rad*p.rotation.z)+ "  cos z " +  Mathf.Cos(Mathf.Deg2Rad*p.rotation.z));

				p.SO.rotation = Quaternion.RotateTowards (p.SO.rotation, rotationToTarget, p.rotation_speed * Time.deltaTime);
//				float difx = p.rotation.eulerAngles.x - oldRotation.eulerAngles.x;
//				float dify = p.rotation.eulerAngles.y - oldRotation.eulerAngles.y;
//				Debug.Log("dif x " +  (difx*Mathf.Sin(Mathf.Deg2Rad*p.rotation.z)) + "  dif y " + (dify*Mathf.Cos(Mathf.Deg2Rad*p.rotation.z)) );
//				Debug.Log("roll" +(difx*Mathf.Sin(Mathf.Deg2Rad*p.rotation.z) + dify*Mathf.Cos(Mathf.Deg2Rad*p.rotation.z)) );
				return false;
			} else {
				return true;
			}
		}
		return false;
	}
		void Accelerate(){
		if (p.newSpeed != p.SO.speed) {
			if (p.newSpeed > p.SO.speed) {
				p.SO.speed += Time.deltaTime * p.acceleration_max;
				if (p.newSpeed < p.SO.speed) {
					p.SO.speed = p.newSpeed;
					}
				} else {
				p.SO.speed += -Time.deltaTime *  p.acceleration_max ;
				if (p.newSpeed > p.SO.speed) {
					p.SO.speed = p.newSpeed;
					}
				}
			}
		}

	private void Move(){
		if (moveCommand==MoveType.move) {
			Rotate();
			Accelerate ();
			p.SO.speed = Mathf.Lerp(p.SO.speed, p.max_speed, Time.deltaTime * p.acceleration_max);
			p.SO.position += p.SO.rotation * Vector3.forward * Time.deltaTime * p.SO.speed;		
		}
	}


	private void Stop(){
		if (moveCommand == MoveType.stop) {
//			SendEvent(ShipEvenentsType.stop);
			p.newSpeed = 0;
		}
	}

	public void Atack_target(int weaponnum){
//		Debug.Log ("atacking");
		if (newtargetToAtack != null) {
			targetToAtack = newtargetToAtack;
			Debug.Log (p.SO.visibleName + " atacking " + targetToAtack.p.SO.visibleName);

			atack = true;
			weapons[weaponnum].Atack_target(targetToAtack);

		}else{
			atack = false;
		}

	}
	public void StopFire(int weaponnum){
		Debug.Log (p.SO.visibleName + " stop fire ");
		atack = false;
		weapons [weaponnum].stop ();
		}




	public void Damage(float damage){
        if (p.shield - damage > 0)
        {
            p.shield += -damage;
		}
		else{
            if (p.shield + p.armor - damage > 0)
            {
                p.armor += p.shield - damage;
                p.shield = 0;
			}
			else{
                if (p.shield + p.armor + p.hull - damage > 0)
                {
                    p.hull += p.armor + p.shield - damage;
                    p.shield = 0;
                    p.armor = 0;
				}
				else{
					Destroyed();
				}
            }
        }
	}
	private void Destroyed(){
        p.destroyed = true;
		for (int i = 0; i < weapons.Count; i++) {
			StopFire (i);
		}
		StopEquipment ();
        SendEvent(ShipEvenentsType.destroyed);
	}

	private void RestoreTick(){
		if (p.hull<p.hull_full){
        	p.hull += p.hull_restore*Time.deltaTime;
		}else{p.hull = p.hull_full;}
		if (p.shield<p.shield_full)
		{p.shield += p.shield_restore * Time.deltaTime;}
		else {p.shield = p.shield_full;}
			
		if (p.armor<p.armor_full){ p.armor += p.armor_restore * Time.deltaTime;}
		else {p.armor=p.armor_full;}
		if (p.capasitor<p.capasitor_full) {p.capasitor += p.capasitor_restore * Time.deltaTime;}
		else{p.capasitor=p.capasitor_full;}
		
	}
	private void Agr(){
		if (p.mob&&!p.destroyed) {
//			Debug.Log ("mob "+p.id);
			if (newtargetToAtack != null&&!atack) {
//				Debug.Log ("mob "+p.id+" targ "+newtargetToAtack.p.id +" dist " + Vector3.Distance(p.SO.position, newtargetToAtack.p.SO.position)+" adist "+ p.agr_distance);
                if (Vector3.Distance(p.SO.position, newtargetToAtack.p.SO.position) < p.agr_distance)
                {
//					Debug.Log ("agr");
					SetTarget (newtargetToAtack);
					GoToTarget();
					for (int i = 0; i < weapons.Count; i++) {
						Atack_target (i);
					}
				} else {
					atack = false;
				}
			}
		}
	}
    public void SendEvent(ShipEvenentsType evnt)
    {
		if (host != null) {
//			Debug.Log (host.name);
			if (host.name == "ServerGo") {
				host.GetComponent<Server> ().Events(evnt,this);

			} else {
				host.GetComponent<ShipMotor> ().Events(evnt,this);
			}
//			Debug.Log (host.name);

		}
    }

	public IEnumerator Warpdrive(){
        warpCoroutineStarted = true;
		SendEvent(ShipEvenentsType.warmwarp);
		Debug.Log ("warming warp drive  " + p.warpDriveStartTime + " s");
		yield return new WaitForSeconds (p.warpDriveStartTime);


		float warpDistance = Vector3.Distance(p.SO.position, targetToMove.position);

        float warpTime = warpDistance / p.warpSpeed;
        SendEvent(ShipEvenentsType.hide);
        Hide();
        warpActivated = true;
		Debug.Log ("flying warp   " + warpTime+ " s");
        SendEvent(ShipEvenentsType.warp);
        yield return new WaitForSeconds(warpTime);

		Spawn(targetToMove.position-Vector3.forward*10);
        SendEvent(ShipEvenentsType.spawn);
        SendEvent(ShipEvenentsType.reveal);    
		Reveal();
        warpActivated = false;
        //warpToTargetFlag = false;
        warpCoroutineStarted = false;
		p.SO.speed = p.max_speed * 0.1f;
		moveCommand = MoveType.stop;
		SendEvent(ShipEvenentsType.stop);

		complexCommand = ComandType.none;


	}

	private void Spawn(Vector3 _position) {
		p.SO.position=_position;	
		Debug.Log (host);
		if (host!=null){
//			host.GetComponent<ShipMotor>().Spawn(p.SO.id);
		}
	}
    private void Hide(){
        p.hidden = true;
    }
    private void Reveal()
    {
        p.hidden = false;
    }


	private void CommandManager()
	{
		if (complexCommand==ComandType.goTo){

			if (Vector3.Distance(p.SO.position,targetToMove.position)> 10*p.SO.speed/p.acceleration_max)
			{
//				Debug.Log ("command goto");
				moveCommand = MoveType.move;
				SendEvent(ShipEvenentsType.move);

			}
			else
			{
//				Debug.Log(Vector3.Distance (p.SO.position, targetToMove.position));

				moveCommand=MoveType.stop;
				SendEvent(ShipEvenentsType.stop);

				complexCommand = ComandType.none;
			}
		}
		if (complexCommand == ComandType.warpTo) {
			moveCommand = MoveType.warp;
		}
		if (complexCommand == ComandType.landTo) {
			if (Vector3.Distance (p.SO.position, targetToMove.position) > 10 * p.max_speed / p.acceleration_max) {
				moveCommand = MoveType.move;
				SendEvent(ShipEvenentsType.move);

			} else {
				
				moveCommand = MoveType.stop;
				SendEvent(ShipEvenentsType.stop);


                SendEvent(ShipEvenentsType.land); 

			}
		}
	
	}

			
		

	public void Tick(){
		Agr();
		RestoreTick ();
		CommandManager ();
		Move ();
		Stop ();
	}

//	public ServerObject ConvertToSO(){
//		ServerObject SO= new ServerObject();
//		SO.id=p.SO.id;
//		SO.position=p.SO.position;
//		SO.rotation=p.SO.rotation;
//		SO.speed=p.SO.speed;
//		SO.prefab=p.SO.prefab;
//		SO.visibleName = p.SO.visibleName;
//		return SO;
//	}
		
}
