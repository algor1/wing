using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraslink : MonoBehaviour {
	[SerializeField]
	private GameObject dataLocal;
	[SerializeField]
	private ParticleSystem warpPartical;
    private bool warpFlag;
    private Vector3 warpTarget;
    private float warpSpeed;

	public Camera mainCamera;
	private ParticleSystem.EmissionModule warpParticlesEmission;

	// Use this for initialization
	void Start () {
		warpParticlesEmission = warpPartical.GetComponent<ParticleSystem>().emission;
        Vector3 zp = dataLocal.GetComponent<ShowEnv>().GetZeroPoint();
        transform.localPosition =zp / 1000;
        warpFlag = false;
		warpParticlesEmission.enabled=false;
        warpTarget = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {

        if (!warpFlag)
        {
            NormalFly();
        }
        else
        {
            WarpFly();
        }

	}
    void NormalFly()
    {
//		print (transform.localPosition);
        Vector3 zp = dataLocal.GetComponent<ShowEnv>().GetZeroPoint();
        transform.localPosition = (mainCamera.transform.position + zp) / 1000;
        transform.rotation = mainCamera.transform.rotation;

    }
    void WarpFly()
    {
//		print(Vector3.Distance (warpTarget, transform.localPosition));
//		print(warpTarget);
//		print(transform.localPosition);
//		print( (warpTarget- transform.localPosition));
//		transform.Translate ((warpTarget - transform.localPosition).normalized * warpSpeed * Time.deltaTime);
		warpPartical.transform.localPosition = Vector3.MoveTowards (transform.localPosition, warpTarget, 20);
		transform.localPosition=Vector3.MoveTowards(transform.localPosition, warpTarget, warpSpeed * Time.deltaTime);
		transform.rotation = mainCamera.transform.rotation;
		warpPartical.transform.localPosition = Vector3.MoveTowards (transform.localPosition, warpTarget, 5);
		warpPartical.transform.LookAt (transform.position);

    }
    public bool WarpTo(Vector3 target,float speed)
    {
		if (!warpFlag) {
			warpFlag = true;
			warpTarget = target  / 1000;
			warpSpeed = speed / 1000;
			warpParticlesEmission.enabled = true;

		}

		return (Vector3.Distance (warpTarget, transform.localPosition) > 100);
    }
    public void WarpStop()
    {
		warpParticlesEmission.enabled=false;
		warpFlag = false;
    }
}
