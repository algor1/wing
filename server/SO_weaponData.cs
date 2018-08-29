using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SO_weaponData {
	public enum WeaponType {laser,missile,projective};
    public WeaponType type;
	public bool active;	
	public float damage;
    public float reload;
	public float ammoSpeed;
    public float activeTime;//for laser
	public float sqrDistanse_max;
	public float capasitor_use;

	public SO_weaponData(){
	}
	public SO_weaponData(SO_weaponData val){
		active = val.active;
		damage = val.damage;
		reload = val.reload;
		ammoSpeed = val.ammoSpeed;
		sqrDistanse_max = val.sqrDistanse_max;
		capasitor_use = val.capasitor_use;
	}

}
