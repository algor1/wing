using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effects : MonoBehaviour {

    [SerializeField]
    private Camera secondCamera;

    public void PlayerWarp(Vector3 _target,float _speed) {
		GetComponent<ShowEnv> ().ClearAll ();
        secondCamera.GetComponent<cameraslink>().WarpTo(_target,_speed);


    }
	public void PlayerWarpStop() {
		secondCamera.GetComponent<cameraslink>().WarpStop();
		GetComponent<ShowEnv> ().UpdateAll ();
	}
}
