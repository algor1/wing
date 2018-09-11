using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]

public class MissileLauncher : MonoBehaviour {

	[SerializeField]
	GameObject player;
	//laser
	LineRenderer laser_shot;
	public Material laserMaterial;
	private bool laser_enabled;
	private SO_ship target;
	// Use this for initialization
	void Start () {
		initLaser ();
		stoplaser ();

	}

	// Update is called once per frame
	void Update () {
		if (laser_shot.enabled && target!=null) {
//			Debug.Log("from "+ transform.position+ "!!!!!!!!pew to " + target.p.SO.visibleName+" p "+ target.host.transform.position);
			laser_shot.SetPosition (0,transform.position);
			laser_shot.SetPosition(1, target.host.GetComponent<Transform>().position);

		}
		if (target == null) {
//
//			// TODO разобраться с выключением
//			bool u= (target==null);
////			Debug.Log ("target nulll " + target+ u);
//			stoplaser ();// почемуто сразу выключает лазер
		}
//		yield return new WaitForSeconds(1);

	}


	void initLaser()
	{
		laser_shot = GetComponent<LineRenderer>();
		laser_shot.positionCount=2;
		laser_shot.GetComponent<Renderer>().material = laserMaterial;
		laser_shot.startWidth = 0.1f;
		laser_shot.endWidth=0.03f;
	}




	void stoplaser(){
		laser_shot.enabled = false;
	}


	public void StartFire (SO_ship _target)
	{
		target = _target;
//		Debug.Log ("====laser pew to " + target.p.SO.visibleName);
//		Debug.Log( laser_shot.GetPosition(0)+ "    "  +  laser_shot.GetPosition(1));
		laser_shot.enabled = true;

	}
	public void StopFire ()
	{
		laser_shot.enabled = false;
//		Debug.Log ("====laser stop " );

	}
}