using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ServerObject {
	public int id;
	public string visibleName;
	public enum typeSO {asteroid,ship,station,waypoint,container};
    public typeSO type;
	public Vector3 position;
	public Quaternion rotation;
	public float speed;
	public string prefab;

	public ServerObject( ServerObject so_){
		id = so_.id;
		visibleName = so_.visibleName;
		position = so_.position;
		rotation = so_.rotation;
		speed = so_.speed;
		prefab = so_.prefab;
		type = so_.type;
	}
	public ServerObject(){
	}
}
