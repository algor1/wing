using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FlyObject {
	public int flyObjectId;
	public Vector3 position;
	public Vector3 direction;
	public float speed;
	public GameObject prefab;
	public GameObject sceneObject;

	public FlyObject(int flyObjectId_, Vector3 position_,Vector3 direction_,float speed_){
		flyObjectId = flyObjectId_;
		position = position_;
		direction = direction_;
		speed = speed_;
	}


}
