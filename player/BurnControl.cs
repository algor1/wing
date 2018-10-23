using UnityEngine;
using System.Collections;


public class BurnControl : MonoBehaviour {

	private ParticleSystem ps;

    void Awake () {
        ps = GetComponent<ParticleSystem>();
		EngineStop();
	}

    public void EngineStart()
    {
		var psMain = ps.main;
		psMain.startLifetimeMultiplier = 0.5f;
//		Debug.Log ("engine   start");
    }

    public void EngineStop()
    {
		var psMain = ps.main;
		psMain.startLifetimeMultiplier = 0.05f;
//		Debug.Log ("engine STOP");
    }

    public void WarpStart()
    {
		var psMain = ps.main;
		psMain.startLifetimeMultiplier = 2f;
    }
    public void WarpStop()
    {
		var psMain = ps.main;
		psMain.startLifetimeMultiplier = 0.05f;
    }
}