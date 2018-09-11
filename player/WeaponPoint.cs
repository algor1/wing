using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPoint : MonoBehaviour {
    public SO_weaponData.WeaponType type;
    public Component weapon;

	void Start()
	{
        switch (type)
        {
            case SO_weaponData.WeaponType.laser:
                weapon = gameObject.AddComponent<LaserBeam>();
                break;
            case SO_weaponData.WeaponType.missile:
                weapon = gameObject.AddComponent<MissileLauncher>();
                break;
        }

	}


	public void StartFire (SO_ship target)
	{
		gameObject.GetComponent<LaserBeam>().StartFire(target);
//		Debug.Log ("+++++pew to " + target.p.SO.visibleName);
	}
	public void StopFire ()
	{
//		Debug.Log ("++++stop pew  ");

		gameObject.GetComponent<LaserBeam>().StopFire();
	}
}
