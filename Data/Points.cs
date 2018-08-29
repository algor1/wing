using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using WayPoint;

[CreateAssetMenu()]
public class Points : ScriptableObject {
	public List<WayPoint> wayPoints;


//	// Use this for initialization
//	void Start () {
//		Vector3 zero_point = GetComponent<ShowEnv> ().zeroPoint;
//        
//		WayPoint wp1= new WayPoint();
//		wp1.position= new Vector3(-3124000,0,0)+zero_point*1000;
//        wp1.name="Waypoint 1";
//		wayPoints.Add(wp1);
//		WayPoint wp2= new WayPoint();
//		wp2.position= new Vector3(1000000,0,0)+zero_point*1000;
//		wp2.name="Waypoint 2";
//		wayPoints.Add(wp2);
//		WayPoint wp0= new WayPoint();
//		wp0.name="Waypoint 0";
//		wp0.position= new Vector3(0,0,0)+zero_point*1000;
//		wayPoints.Add(wp0);
//
//
//
//	}
//	
//	// Update is called once per frame
//	void Update () {
//		
//	}
}
