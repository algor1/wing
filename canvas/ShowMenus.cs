using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowMenus : MonoBehaviour {
	[SerializeField]
	private GameObject menuPrefab;

	public void ShowInventory (int player_id, int container_id) {
		Debug.Log (" open inventory " + player_id + "  " + container_id);
		GameObject someMenu = (GameObject)Instantiate(menuPrefab, gameObject.transform);
//		someMenu.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
//		someMenu.transform.SetParent(gameObject.transform, false);
		someMenu.GetComponent<CreateInventoryMenu>().leftHolder_id=player_id;
		someMenu.GetComponent<CreateInventoryMenu>().rightHolder_id=container_id;
//		return true;
	}
}
