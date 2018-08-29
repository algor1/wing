using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerSO : MonoBehaviour {



	[SerializeField]
	private SOList_scrob SODB;
	private List<ServerObject> serverObjects;
//	public enum Command { MoveTo,WarpTo,Atack,SetTarget};





	void Awake()
	{
        DontDestroyOnLoad(transform.gameObject);
		LoadserverObjects();

	}

//
//	void Update(){
//		Tick();	
//
//	}



	public List<ServerObject> Nearest (int player_id){
		SO_ship pl = GetComponent<Server>().GetPlayer (player_id);
		List<ServerObject> resultSOList = new List<ServerObject> ();
		for (int i = 0; i < serverObjects.Count; i++){
			float dist = Vector3.Distance (pl.p.SO.position, serverObjects [i].position);
			if (dist<pl.p.vision_distance){ 
				resultSOList.Add (serverObjects [i]);
			}

		}
		return resultSOList;
	}


	private void LoadserverObjects()
	{
		serverObjects = new List<ServerObject> ();
		Debug.Log ("all SO count" + SODB.SOList.Count);
		for (int i = 0; i < SODB.SOList.Count; i++)
		{
			ServerObject s = new ServerObject (SODB.SOList [i]);
			serverObjects.Add(s);
		}
	}


	public ServerObject GetSO(int SO_id){
		for (int i = 0; i < serverObjects.Count; i++)
			if (serverObjects [i].id == SO_id) {
				return serverObjects [i];
			}
		return null;
	}

//	private void Tick(){
//		for (int i = 0; i < serverObjects.Count; i++){
//			serverObjects[i].Tick();
//		}
//	}
}
