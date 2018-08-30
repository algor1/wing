using UnityEngine;
using System.Collections;


public class BurnControl : MonoBehaviour {
    private ParticleSystem ps;
//    private ParticleSystem.EmissionModule ps_em;
//    private ParticleSystem.MainModule ps_main;
    float normal_duration;
	
    void Awake () {
        ps = GetComponent<ParticleSystem>();
//        ps_em = ps.emission;
//		ps_main = ps.main;
		var em=ps.emission;
//			em.enabled = false;
		EngineStop();
		normal_duration = ps.main.duration;


	}

    public void EngineStart()
    {
		var em=ps.emission;
		var psMain = ps.main;
		psMain.startLifetimeMultiplier = 0.5f;
		Debug.Log ("engine   start");

//		em.enabled = true;
    }

    public void EngineStop()
    {
		var psMain = ps.main;
		psMain.startLifetimeMultiplier = 0.05f;
		Debug.Log ("engine STOP");
//		var em=ps.emission;
//		em.enabled =false;
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