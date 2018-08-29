using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOParametres : MonoBehaviour {

	public ServerObject thisServerObject;
	[SerializeField]
	private GameObject dataLocal;

	public void Init (ServerObject thisSO_, GameObject _datalocal)
		{
		thisServerObject= new ServerObject(thisSO_);
		dataLocal = _datalocal;

	}

	void Update(){
		transform.position = thisServerObject.position-dataLocal.GetComponent<ShowEnv>().GetZeroPoint();

	}
}
