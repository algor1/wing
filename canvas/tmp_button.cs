using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tmp_button : MonoBehaviour {
	[SerializeField]
	private GameObject menuPrefab;

	public void PressedTmpButton () {
		GameObject someMenu = (GameObject)Instantiate(menuPrefab, gameObject.transform);
//		someMenu.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
//		someMenu.transform.SetParent(gameObject.transform, false);
		someMenu.GetComponent<CreateInventoryMenu>().leftHolder_id=0;
		someMenu.GetComponent<CreateInventoryMenu>().leftHolder_id=200;
//		return true;
	}
}
