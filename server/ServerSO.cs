using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerSO : MonoBehaviour {

	public bool started;

    //[SerializeField]
    //private SOList_scrob SODB;
	private List<ServerObject> serverObjects;
//	public enum Command { MoveTo,WarpTo,Atack,SetTarget};





	void Start()
	{
        DontDestroyOnLoad(transform.gameObject);
		LoadserverObjects();
		started = true;

	}

	public List<ServerObject> Nearest (int player_id){
		SO_ship pl = GetComponent<Server>().GetPlayer (player_id);
		List<ServerObject> resultSOList = new List<ServerObject> ();
		for (int i = 0; i < serverObjects.Count; i++){
			float dist = Vector3.Distance (pl.p.SO.position, serverObjects [i].position);
//			Debug.Log (dist + "   " + serverObjects [i].type);
			if (dist<pl.p.vision_distance||serverObjects[i].type==ServerObject.typeSO.waypoint){ 
				resultSOList.Add (serverObjects [i]);
			}

		}
		return resultSOList;
	}


	private void LoadserverObjects()
	{
        List<ServerObject> SOList = GetComponent<ServerDB>().GetAllSO();
		serverObjects = new List<ServerObject> ();
		Debug.Log ("all SO count" + SOList.Count);
		for (int i = 0; i < SOList.Count; i++)
		{
            if (SOList[i].type != ServerObject.typeSO.ship)
            {
				Debug.Log ("SO " + i);

                ServerObject s = new ServerObject(SOList[i]);
                serverObjects.Add(s);
            }
		}
	}


	public ServerObject GetSO(int SO_id){
		for (int i = 0; i < serverObjects.Count; i++)
			if (serverObjects [i].id == SO_id) {
				return serverObjects [i];
			}
		return null;
	}

    public void AddContainer(int ship_id)
    {
		int containerItemId = 4;// TODO  брать из базы
		ServerObject destroyedShip = GetComponent<ServerDB>().GetServerObject(ship_id);
		Debug.Log (destroyedShip.position);
		
		ServerObject newContainer = GetComponent<ServerDB>().AddNewSO(destroyedShip.position, destroyedShip.rotation, containerItemId);
		AddSO (newContainer);
		GetComponent<InventoryServer>().ContainerFromShip(newContainer,destroyedShip);
    }

	public void DestroyContainer(int container_id){
//		int containerItemId = 4;// TODO  брать из базы
		if (GetSO (container_id).type == ServerObject.typeSO.container) {
			if (GetComponent<InventoryServer> ().ObjectInventory (container_id).Count == 0) {
				DestroySO (container_id);
			}
		}

	}

	private void AddSO(ServerObject newSO){
		serverObjects.Add (newSO);
	}
	private void DestroySO(int SO_id){
		for (int i = 0; i < serverObjects.Count; i++) {
			
			if (serverObjects [i].id == SO_id) {
				GetComponent<ServerDB> ().DeleteServerObject (SO_id);
				serverObjects.RemoveAt (i);
			}
		}
	}

//	private void Tick(){
//		for (int i = 0; i < serverObjects.Count; i++){
//			serverObjects[i].Tick();
//		}
//	}
}
