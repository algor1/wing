using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]

public class MissileLauncher : MonoBehaviour {

	[SerializeField]
	GameObject player;
    public GameObject missilePrefab;
    private GameObject target;


    void Start()
    {
        missilePrefab = Resources.Load("prefabs/Missil_01") as GameObject;
    }

    public void Fire(GameObject _target)
    {
        target = _target;
        //		GameObject target = GameObject.Find ("Cube");
        GameObject missile = (GameObject)Instantiate(missilePrefab, transform.position, transform.rotation);
        missile.GetComponent<Missile>().target = target;
    }
}