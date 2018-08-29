using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointParametres : MonoBehaviour {

	public ServerObject thisServerObject;

	public void Init (ServerObject thisSO_){
		thisServerObject= new ServerObject(thisSO_);
	}
}
