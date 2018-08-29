using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipEquipmentUI : MonoBehaviour {

	[SerializeField]
	private GameObject player;
	private List< SO_weapon> weaponList;
	private List<SO_equipment> eqList;
	[SerializeField]
	private GameObject eqPrefab;



	 



	void Start ()
	{
		weaponList = player.GetComponent<ShipMotor> ().thisShip.weapons;
		eqList = player.GetComponent<ShipMotor> ().thisShip.equipments;
		WeaponButtonInit ();
		EqButtonInit ();
	}

	private void WeaponButtonInit(){
		for (int i = 0; i < weaponList.Count; i++)
		
				{
					GameObject we_button = (GameObject)Instantiate(eqPrefab);
					we_button.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
			we_button.GetComponent<RectTransform>().position+=Vector3.right*(i*-50-120);
//			var ship_obj = player.GetComponent<ShipMotor>().AtackTarget(i) ;
			int _i=i;
			we_button.GetComponent<Button>().onClick.AddListener( ()=> { player.GetComponent<ShipMotor>().AtackTarget(_i) ;});
		
				}
	}
	private void EqButtonInit(){
		for (int i = 0; i < eqList.Count; i++)

		{
			GameObject eq_button = (GameObject)Instantiate(eqPrefab);
			eq_button.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
			eq_button.GetComponent<RectTransform>().position+=Vector3.right*(i*-50-300);
			//			var ship_obj = player.GetComponent<ShipMotor>().AtackTarget(i) ;
			int _i=i;
			eq_button.GetComponent<Button>().onClick.AddListener( ()=> { player.GetComponent<ShipMotor>().StartEquipment(_i) ;});

		}
	}

}






	//	public List<Item> Eq= new List<Item>();
//	public ItemDatabase DB;
//	public GameObject eqPrefab;
//	public List<GameObject> eq_button_objs =new List<GameObject>();
//	public List<GameObject> weaponPointsObj_objs;


	// Use this for initialization
//	void Start () {
//		Eq.Add (DB.GetItem (10));
//		Eq.Add(DB.GetItem(11));
////		initLaser ();
//
//		
//		for (int i = 0; i < Eq.Count; i++)
//
//		{
//			GameObject eq_button = (GameObject)Instantiate(eqPrefab);
//			eq_button.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
//			eq_button.GetComponent<RectTransform>().position+=Vector3.right*i*-100;
////			print (Eq [i].itemID);
//			int _id = Eq [i].itemID;
//			var wp = weaponPointsObj_objs [i].GetComponent<LaserBeam>();
////			eq_button.GetComponent<Button>().onClick.AddListener( ()=> { wp.fireLaser();});
//				
//
//
//			eq_button.GetComponent<Image>().sprite= Eq[i].itemSprite;
//
//
//
//			eq_button_objs.Add (eq_button);
//
//
//		}
//						
//
//	}
	
	// Update is called once per frame
//	void Update () {
//		stoplaser ();
		
//	}

//	void UseEq(int id){
//		print(id);
//		fireLaser(
//	}
//	void UseEq(){
//		print("id");
//	}

//	void initLaser()
//		{
//		laser_shot = GetComponent<LineRenderer>();
//		laser_shot.SetVertexCount(2);
//		laser_shot.GetComponent<Renderer>().material = laserMaterial;
//		laser_shot.SetWidth(0.1f, 0.03f);
//		}
//		
//	void fireLaser (GameObject weaponPoint)
//	{
//		GameObject target = GetComponent<ShipControl> ().selectedTarget;
////		laser_shot = GetComponent<LineRenderer>();
//		laser_shot.enabled = true;
//		laser_shot.SetPosition(0, weaponPoint.transform.position);
//		laser_shot.SetPosition(1, target.transform.position);
//
//
//	}
//
//	void stoplaser(){
//		laser_shot.enabled = false;
//	}
//	public class LaserBeam : MonoBehaviour 
//	{
//		Vector2 mouse;
//		RaycastHit hit;
//		float range = 100.0f;
//		LineRenderer line;
//		public Material lineMaterial;
//
//		void Start()
//		{
//			line = GetComponent<LineRenderer>();
//			line.SetVertexCount(2);
//			line.GetComponent<Renderer>().material = lineMaterial;
//			line.SetWidth(0.1f, 0.03f);
//		}
//
//		void Update()
//		{
//			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//			if(Physics.Raycast(ray, out hit, range))
//			{
//				if(Input.GetMouseButton(0))
//				{
//					line.enabled = true;
//					line.SetPosition(0, transform.position);
//					line.SetPosition(1, hit.point + hit.normal);
//				}
//				else
//					line.enabled = false;
//			}
//
//		}
//	}



