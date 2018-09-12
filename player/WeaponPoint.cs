using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPoint : MonoBehaviour {
    public SO_weaponData.WeaponType type;
    public Component weapon;

	public void Init(SO_weaponData.WeaponType _type)
	{
		type = _type;
        switch (type)
        {
            case SO_weaponData.WeaponType.laser:
                weapon = gameObject.AddComponent<LaserBeam>();
                break;
			case SO_weaponData.WeaponType.missile:
				weapon = gameObject.AddComponent<MissileLauncher> ();
				break;
        }
	}


	public void StartFire (SO_ship target)
	{
        Debug.Log("StartFire  "+ type);
        switch (type)
        {
            case SO_weaponData.WeaponType.laser:
   
                gameObject.GetComponent<LaserBeam>().StartFire(target);
                break;
            case SO_weaponData.WeaponType.missile:
                gameObject.GetComponent<MissileLauncher>().Fire(target.host);
                break;
        }
	}

	public void StopFire ()
	{
        switch (type)
        {
            case SO_weaponData.WeaponType.laser:
                gameObject.GetComponent<LaserBeam>().StopFire();
                break;
            case SO_weaponData.WeaponType.missile:
                break;
        }
		
	}
}
