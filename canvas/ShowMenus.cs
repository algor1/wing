using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ShowMenus : MonoBehaviour {
	int station_id=200;
	[SerializeField]
	private GameObject inventoryPrefab;
	public bool inventoryOpened = false;
	private GameObject inventoryMenu;
	[SerializeField]
	private GameObject skillsPrefab;
	public bool skillsOpened = false;
	private GameObject skillsMenu;

	public void ShowInventory (int player_id, int container_id) {
		if (!inventoryOpened){
			Debug.Log (" open inventory " + player_id + "  " + container_id);
			inventoryMenu = (GameObject)Instantiate(inventoryPrefab, gameObject.transform);
	//		someMenu.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
	//		someMenu.transform.SetParent(gameObject.transform, false);
			inventoryMenu.GetComponent<CreateInventoryMenu>().leftHolder_id=player_id;
			inventoryMenu.GetComponent<CreateInventoryMenu>().rightHolder_id=container_id;
	//		return true;
			inventoryOpened=true;
		}
	}
	public void ShowStationInventory (int player_id) {
		ShowInventory (player_id, station_id);
	}
	public void ShowSkills (int player_id) {
		if (!skillsOpened){
			Debug.Log (" open skills " + player_id );
			skillsMenu = (GameObject)Instantiate(skillsPrefab, gameObject.transform);
			skillsOpened=true;
		}
	}
	public void  takeOffStation(){
		Debug.Log ("Taking off station " + station_id);
		GameObject serverObj = GameObject.Find ("ServerGo");
		serverObj.GetComponent<Server> ().PlayerControl (0, Server.Command.TakeOff, 0);
		serverObj.GetComponent<LandingServer> ().TakeOff (station_id, 0);
		SceneManager.LoadScene ("1");


	}


}
