using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControl : MonoBehaviour {
	private Camera cam;

	// Use this for initialization
	void Start () {
		cam = GetComponent<Camera> ();
	}
	
	// Update is called once per frame
	void Update () {
		float cameraZoom = Input.GetAxisRaw ("camzoom");
//		print (cam.transform.position);
		if (cameraZoom!=0) {
//			print(cam.transform.forward);
			cam.transform.position = cam.transform.position+cam.transform.forward*cameraZoom;
		}
//		Vector3 rotVer = Vector3.up * x_rotate_ship;
//		Vector3 rotHor = Vector3.right * y_rotate_ship;
//		Vector3 rotZ = Vector3.forward * z_rotate_ship;
//		Vector3 rotation = (rotHor+rotVer+rotZ).normalized*rotation_speed;
//		Vector3 screenPos = cam.WorldToScreenPoint(target_tmp);
//		Debug.Log("target is " + screenPos.x + " pixels from the left"+screenPos.y+" dist "+screenPos.z);
		
	}
}
