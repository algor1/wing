using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ServerStarter : MonoBehaviour {

	
	// Update is called once per frame
	void Update () {

		if (
			GetComponent<Server> ().started &&
			GetComponent<ServerDB> ().started &&
			GetComponent<ServerSO> ().started &&
			GetComponent<PlayerSkillsServer> ().started &&
			GetComponent<InventoryServer> ().started &&
			GetComponent<LandingServer> ().started) {
				Debug.Log ("All servers loaded");
				SceneManager.LoadScene ("1");
				this.enabled = false;
		}
	}
}
