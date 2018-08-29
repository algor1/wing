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
			em.enabled = true;
		normal_duration = ps.main.duration;


	}

    public void EngineStart()
    {
		var em=ps.emission;
		em.enabled = true;
    }

    public void EngineStop()
    {
		var em=ps.emission;
		em.enabled =false;
    }

    public void WarpStart()
    {
		ps.Stop ();
		if (!ps.isPlaying) {
			var psm = ps.main;
			psm.duration = normal_duration * 5;
		}
		ps.Play ();
    }
    public void WarpStop()
    {
		ps.Stop ();
		if (!ps.isPlaying) {
			
			var psm = ps.main;
			psm.duration = normal_duration;
			ps.Play ();
		}
    }
    

}