using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ShowEnv : MonoBehaviour {

    //public GameObject station;
    private Vector3 zeroPoint;
//	[SerializeField]
	public Waypoints wp;
	[SerializeField]
	private GameObject wpprefab;
	public List<GameObject> wpList;
	public GameObject player;
	private int player_id;
	private SO_ship player_SO;

//	public List<FlyObject> allFlyObject;
	public List<SO_ship> serverShipslist;
	public Dictionary<int,FlyObject> nearestFlyObject;
	public Dictionary<int,GameObject> nearestShips;
	public List<ServerObject> serverSOlist;
	public Dictionary<int,GameObject> nearestSOs;
	Coroutine shipcoroutine;
	Coroutine socoroutine;
	private IEnumerator coroutineSH;
	private IEnumerator coroutineSO;
	[SerializeField]
	private GameObject serverObj;
	[SerializeField]
	private GameObject serverSOObj;
    [SerializeField]
	private GameObject serverLandObj;
	[SerializeField]
	private GameObject canvasobj;



	void Awake()
	{
		nearestFlyObject = new Dictionary<int,FlyObject> ();
		nearestShips =new Dictionary<int,GameObject> ();
		nearestSOs =new Dictionary<int,GameObject> ();

	}
	void Start () {

		player_id=0;
		serverObj = GameObject.Find ("ServerGo");
		player_SO=serverObj.GetComponent<Server>().GetPlayer(player_id);//player id=0 
		//init player
		player.GetComponent<ShipMotor>().Init(player_SO,serverObj,this.gameObject);
		zeroPoint = player_SO.p.SO.position;
		WaypointsCreate ();
//		var player_transform = player.GetComponent<Transform> ();
//		StartCoroutine (FlyObjectsListUpdate ());
//		 shipcoroutine= StartCoroutine (ShipsListUpdate ());
//		 socoroutine= StartCoroutine (SOListUpdate ());
		coroutineSH=ShipsListUpdate();
		coroutineSO = SOListUpdate ();
		StartCoroutine (coroutineSH);
		StartCoroutine (coroutineSO);
	}

	void Update(){
        //player_SO=serverObj.GetComponent<Server>().GetPlayer(player_id);
		ChangePlayer();
//		StartCoroutine (ShipsListUpdate ());

	}

	public void SetZeroPoint(Vector3 newZeroPoint)
	{
		zeroPoint = newZeroPoint;
		WaypointsUpdate ();
	}

	public Vector3 GetZeroPoint()
	{
        return zeroPoint;
	}


	public void ClearAll(){
		Debug.Log ("coroutin stopped");

		StopCoroutine (coroutineSH);
		StopCoroutine (coroutineSO);

		Dictionary<int,int> shipsForDeletionList = new Dictionary<int,int> ();
		foreach (int key in nearestShips.Keys) { 
			shipsForDeletionList.Add (key, key);
		}
		foreach (int key in shipsForDeletionList.Keys) {
			Debug.Log ("ship id " + key + " deleted");
			DeleteShip (key);
		}
		Dictionary<int,int> SOsForDeletionList = new Dictionary<int,int> ();
		foreach (int key in nearestSOs.Keys) { 
			SOsForDeletionList.Add (key, key);
			//				Debug.Log (shipsForDeletionList);
			//				print (key);
		}
		foreach (int key in SOsForDeletionList.Keys) { 
			Debug.Log ("so id " + key + " deleted");
			DeleteSO (key);
		}


	

//		foreach (int key in nearestSOs.Keys) { 
//			DeleteSO (key);
//		}
//		foreach (int key in nearestShips.Keys) { 
//			DeleteShip (key);
//		}
//		nearestSOs.Clear ();
//		nearestShips.Clear ();
	}
	public void UpdateAll(){
		StopCoroutine (coroutineSH);
		StopCoroutine (coroutineSO);
		StartCoroutine (coroutineSH);
		StartCoroutine (coroutineSO);

//		StopCoroutine (shipcoroutine);
//		StartCoroutine (shipcoroutine);
//		StopCoroutine (socoroutine);
//		StartCoroutine (socoroutine);
//		StartCoroutine (ShipsListUpdate ());
//		StopCoroutine (SOListUpdate ());
//		StartCoroutine (SOListUpdate ());

	}

		

	void WaypointsCreate(){
		Debug.Log ("start waypointscreate");

		wpList = new List<GameObject> ();
		Debug.Log ("wplist " +wpList);
		Debug.Log ("wp  "+wp.wayPoints);

		for (int i = 0; i < wp.wayPoints.Count; i++)
			//		foreach (WayPoint wp_value in wp.wayPoints)
		{
			GameObject wp_obj= (GameObject)Instantiate(Resources.Load(wp.wayPoints[i].prefab, typeof(GameObject)),wp.wayPoints[i].position-zeroPoint,Quaternion.Euler(Vector3.zero));
			////////////
			Debug.Log("wp_obj "+wp_obj);
			wp_obj.GetComponent<SOParametres>().Init(wp.wayPoints [i],this.gameObject);
//			wp_obj.GetComponent<PointParametres> ().thisServerObject = wp.wayPoints [i];//???
			wp_obj.name=wp.wayPoints [i].visibleName+"_ind";
			wpList.Add(wp_obj);
		}
		Debug.Log ("stop waypointscreate");

	}

	void WaypointsUpdate(){

		for (int i = 0; i < wpList.Count; i++)
		{
			wpList [i].transform.position = wp.wayPoints [i].position - zeroPoint;

		}
	}

//	---------------------  WP END ---------------------------------

//	---------------------  SO ---------------------------------

	void AddSO(ServerObject so){
		Debug.Log ("prefub " + so.prefab);

		GameObject SObj = (GameObject)Instantiate(Resources.Load(so.prefab, typeof(GameObject)), so.position -zeroPoint, so.rotation);
		SObj.GetComponent<SOParametres> ().Init (so,this.gameObject);
//		Debug.Log (so.visibleName);
		nearestSOs.Add(so.id, SObj);
		Debug.Log ("add SO id " + so.id);
		canvasobj.GetComponent<Indicators> ().AddIndicator_so (SObj);

	}

	void DeleteSO(int so_id){
		canvasobj.GetComponent<Indicators> ().DeleteIndicator_so (so_id);
		if (nearestSOs.ContainsKey (so_id)) {
			Destroy (nearestSOs [so_id]);
			nearestSOs.Remove (so_id);
		}
	}

	void UpdateSO(ServerObject so){

		nearestSOs[so.id].transform.position = so.position - zeroPoint;
		nearestSOs[so.id].transform.rotation = so.rotation;

	}

	void UpdateSOFromServer(){
		serverSOlist	= serverObj.GetComponent<ServerSO>().Nearest(player_id);//player id=0 
	}




	IEnumerator SOListUpdate()
	{
		while (true) {
			Debug.Log(" забираем инфу с сервера ???");	
			UpdateSOFromServer ();
			//		print ("1");
			//Создаем копию dict nearestShips чтобы узнать какие нужно удалить
			Dictionary<int,int> SOsForDeletionList = new Dictionary<int,int> ();
			foreach (int key in nearestSOs.Keys) { 
				SOsForDeletionList.Add (key, key);
				//				Debug.Log (shipsForDeletionList);
				//				print (key);
			}	

			//		Debug.Log (serverShipslist.Count);
			for (int i = 0; i < serverSOlist.Count; i++) {
				//print(serverShipslist[i].p.id);
				if (SOsForDeletionList.Remove (serverSOlist [i].id)) {
					UpdateSO (serverSOlist [i]);
					//print ("4");
				} else {
					Debug.Log ("add SO "+serverSOlist[i].visibleName);
					AddSO	(serverSOlist [i]);
					UpdateSO (serverSOlist [i]);			
				}	

			}
			//удаляем корабли которых небыло в списке
			foreach (int key in SOsForDeletionList.Keys) { 
				DeleteSO (key);


			}
			yield return new WaitForSeconds (10f);
			Debug.Log ("!!!!!!!!!!!!!!!! ended !!!!!!!!!!!!!!!!");
		}
	}

//	---------------------  SO END ---------------------------------

//	---------------------  SHIPS ---------------------------------
	void AddShip(SO_ship ship){
		Debug.Log ("prefub " + ship.p.SO.prefab);
		GameObject gObj = (GameObject)Instantiate(Resources.Load(ship.p.SO.prefab, typeof(GameObject)) , ship.p.SO.position -zeroPoint, ship.p.SO.rotation );// подставить ship.p.SO.prefab
		gObj.GetComponent<ShipMotor> ().Init ( ship,serverObj,this.gameObject);
		gObj.GetComponent<ShipMotor> ().thisShip.SetTarget (player.GetComponent<ShipMotor> ().thisShip);
		Debug.Log (ship.p.SO.visibleName);
		nearestShips.Add(ship.p.SO.id, gObj);
		canvasobj.GetComponent<Indicators> ().AddIndicator_sh (gObj);

	}

	void DeleteShip(int ship_id){
		canvasobj.GetComponent<Indicators> ().DeleteIndicator_sh (ship_id);
		if (nearestShips.ContainsKey (ship_id)) {
			//			if (nearestShips[ship_id].exist)
			Destroy (nearestShips [ship_id]);
			nearestShips.Remove (ship_id);
		}

	}

	void UpdateShip(SO_ship ship){
//        var control = nearestShips[ship.p.id].GetComponent<ShipMotor>();
//		control.selectedTarget=nearestShips [ship.target_id];
//		control.action=ship.action;
		nearestShips[ship.p.SO.id].GetComponent<ShipMotor>().thisShip.p.SO.position = ship.p.SO.position;
		nearestShips[ship.p.SO.id].GetComponent<ShipMotor>().thisShip.p.SO.rotation = ship.p.SO.rotation;

	}

	void UpdateShipsFromServer(){
		serverShipslist	= serverObj.GetComponent<Server>().Nearest(0);//player id=0 
	}
	void ChangePlayer(){
		player_SO=serverObj.GetComponent<Server>().GetPlayer(player_id);//player id=0 		
		player.GetComponent<ShipMotor>().thisShip.p.SO.position=player_SO.p.SO.position;
		player.GetComponent<ShipMotor>().thisShip.p.SO.rotation=player_SO.p.SO.rotation;
        if (player_SO.landed)
        {
			int station_id = serverObj.GetComponent<LandingServer>().FindShipLocation(player_SO.p.SO.id);
			SceneManager.LoadScene ("station");
        }

	}



	IEnumerator ShipsListUpdate()
	{
		while (true) {
//		Debug.Log(" забираем инфу с сервера ???");	
			UpdateShipsFromServer ();
//		print ("1");
			//Создаем копию dict nearestShips чтобы узнать какие нужно удалить
			Dictionary<int,int> shipsForDeletionList = new Dictionary<int,int> ();
			foreach (int key in nearestShips.Keys) { 
				shipsForDeletionList.Add (key, key);
//				Debug.Log (shipsForDeletionList);
//				print (key);
			}	

//		Debug.Log (serverShipslist.Count);
			for (int i = 0; i < serverShipslist.Count; i++) {
				//print(serverShipslist[i].p.id);
				if (shipsForDeletionList.Remove (serverShipslist [i].p.SO.id)) {
					UpdateShip (serverShipslist [i]);
					//print ("4");
				} else {
					print ("add ship");
					AddShip	(serverShipslist [i]);
					UpdateShip (serverShipslist [i]);			
				}	

			}
			//удаляем корабли которых небыло в списке
			foreach (int key in shipsForDeletionList.Keys) { 
				DeleteShip (key);


			}
			ChangePlayer ();
			yield return new WaitForSeconds (10f);
//		Debug.Log ("!!!!!!!!!!!!!!!! ended !!!!!!!!!!!!!!!!");
		}
	}
	//	---------------------  SHIPS END  ---------------------------------

}
