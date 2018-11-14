using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowMenus : MonoBehaviour {
	[SerializeField]
	private GameObject menuPrefab;
	public bool inventoryOpened = false;
	private GameObject inventoryMenu;

	public void ShowInventory (int player_id, int container_id) {
		if (!inventoryOpened){
			Debug.Log (" open inventory " + player_id + "  " + container_id);
			inventoryMenu = (GameObject)Instantiate(menuPrefab, gameObject.transform);
	//		someMenu.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
	//		someMenu.transform.SetParent(gameObject.transform, false);
			inventoryMenu.GetComponent<CreateInventoryMenu>().leftHolder_id=player_id;
			inventoryMenu.GetComponent<CreateInventoryMenu>().rightHolder_id=container_id;
	//		return true;
			inventoryOpened=true;
		}
	}
}
