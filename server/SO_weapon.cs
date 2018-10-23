using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SO_weapon {
    public SO_weaponData p; //weapon properties

	private SO_ship currentTarget;
	private SO_ship host;
	private GameObject weaponPoint;
    public bool fire;

	public SO_weapon (SO_weaponData _data, SO_ship _host){
		p = new SO_weaponData(_data);
		host = _host;
		p.active = false;
		fire = false;
	}
	public void SetWeaponPoint(GameObject _weaponPoint){
		weaponPoint = _weaponPoint;
	}
	public  void Atack_target(SO_ship target){
        
        currentTarget = target;
        fire = true;
//		Debug.Log (host.p.SO.visibleName + " qqqqqqq " + target.p.SO.visibleName+  fire+ p.active);
	}
	public void stop(){
        currentTarget = null;
        fire = false;
	}

    public IEnumerator Attack()
    {
        p.active = true;
        while (fire)
        {
            if (!currentTarget.p.destroyed)
            {
				if (host.p.capasitor >= p.capasitor_use) {
					host.p.capasitor += -p.capasitor_use;
				} else {
					stop ();
				}

                float sqrDistance = (currentTarget.p.SO.position - host.p.SO.position).sqrMagnitude;
                if (sqrDistance > p.sqrDistanse_max * 4)
                {
                    stop();
                }
                else
                {
                    yield return new WaitForSeconds(p.reload);

                    if (sqrDistance < p.sqrDistanse_max)
                    {
						if (weaponPoint!=null){
//							Debug.Log(host.p.SO.visibleName +  " weaponpoint not null");
//							Debug.Log (weaponPoint.GetComponent<WeaponPoint> ());

							weaponPoint.GetComponent<WeaponPoint>().StartFire (currentTarget);}
						yield return new WaitForSeconds(2);

                        if (p.type == SO_weaponData.WeaponType.laser)
                        {
                            yield return new WaitForSeconds(p.activeTime);
                        }
                        else
                        {
                            yield return new WaitForSeconds(Mathf.Sqrt(sqrDistance) / p.ammoSpeed);
                        }    
                        if(host.host != null) {
//							Debug.Log (host.host.name + "  " + host.p.SO.visibleName + "----pew----  to " + currentTarget.p.SO.visibleName); 
						} else{
//							Debug.Log ("server  " + host.p.SO.visibleName + "----pew----  to " + currentTarget.p.SO.visibleName); 
						}
//							Debug.Log (weaponPoint.GetComponent<WeaponPoint> ());
						if (weaponPoint != null) {
							weaponPoint.GetComponent<WeaponPoint> ().StopFire ();
						}
						currentTarget.Damage(p.damage);
                    }
                }
            }
            else
            {
                stop();
            }
        }
        p.active = false;
    }

}
