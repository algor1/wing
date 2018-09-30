﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using System.Collections.Generic;


public class ServerDB : MonoBehaviour {
    private IDbConnection dbSkillCon;
    private IDataReader reader;
    private IDbCommand dbcmd;
	public bool started;

	void Start(){
		started = true;
	}

    void OnApplicationQuit()
    {
        if (dbSkillCon != null) dbSkillCon.Close();
    }

    private void InitDB()
    {
        string p = "wing_srv.db";
        string filepath = Application.persistentDataPath + "/" + p;
        Debug.Log(filepath);
        if (!File.Exists(filepath))
        {
            WWW loadDB = new WWW("jar:file://" + Application.dataPath + "!/assets/" + p);  // this is the path to your StreamingAssets in android
            while (!loadDB.isDone)
            {
                Debug.Log("LOADING..." + loadDB.uploadProgress);
            }
            File.WriteAllBytes(filepath, loadDB.bytes);
        }
        string connectionString = "URI=file:" + filepath;
        dbSkillCon = (IDbConnection)new SqliteConnection(connectionString);
        dbSkillCon.Open();
    }

    private void GetReader(string dbselect)
    {
        if (dbSkillCon == null)
        {
            InitDB();
        }
        //		Debug.Log (dbSkillCon.Database);
        dbcmd = dbSkillCon.CreateCommand();
        //		Debug.Log (dbcmd.CommandText);
        dbcmd.CommandText = dbselect;
        // Выполняем запрос
        reader = dbcmd.ExecuteReader();
        //		Debug.Log (reader.Read());
    }

    public ServerObject GetServerObject(int SO_id)
    {
        ServerObject returnSO=new ServerObject();
        int _type = 0;
		string qwery =@"SELECT id,
                               type,
                               visibleName,
                               position_x,
                               position_y,
                               position_z,
                               rotation_x,
                               rotation_y,
                               rotation_z,
                               rotation_w,
                               speed,
                               prefab_path
                          FROM server_objects
                          where id="+SO_id.ToString();
		GetReader (qwery);
        while (reader.Read())
        {
            if (!reader.IsDBNull(0))returnSO.id = reader.GetInt32(0);
            if (!reader.IsDBNull(1)){ _type = reader.GetInt32(1);}
            if (!reader.IsDBNull(2))returnSO.visibleName=reader.GetString(2);
            if (!reader.IsDBNull(3)&&!reader.IsDBNull(4)&&!reader.IsDBNull(5)) returnSO.position=new Vector3 (reader.GetFloat(3),reader.GetFloat(4),reader.GetFloat(5));
			if (!reader.IsDBNull(6)&&!reader.IsDBNull(7)&&!reader.IsDBNull(8)&&!reader.IsDBNull(9)) returnSO.rotation=new Quaternion (reader.GetFloat(6),reader.GetFloat(7),reader.GetFloat(8),reader.GetFloat(9));
            if (!reader.IsDBNull(0))returnSO.speed=reader.GetFloat(10);
            //if (!reader.IsDBNull(0))returnSO.prefab_path=reader.GetString(11);
            switch (_type)
            {
                case 1:
                    returnSO.type = ServerObject.typeSO.ship;
                    break;
                case 2:
                    returnSO.type = ServerObject.typeSO.station;
                    break;
                case 3:
                    returnSO.type = ServerObject.typeSO.asteroid;
                    break;
            }
        }

        return returnSO; 
    }

    public List<ServerObject> GetAllSO()
    {
        List<ServerObject> returnSOList =new List<ServerObject>();
        string qwery = @"SELECT id,
                               type,
                               visibleName,
                               position_x,
                               position_y,
                               position_z,
                               rotation_x,
                               rotation_y,
                               rotation_z,
                               rotation_w,
                               speed,
                               prefab_path
                          FROM server_objects";
        GetReader(qwery);
        while (reader.Read())
        {
            ServerObject _SO = new ServerObject();
            int _type = 0;
            if (!reader.IsDBNull(0)) _SO.id = reader.GetInt32(0);
            if (!reader.IsDBNull(1)) { _type = reader.GetInt32(1); }
            if (!reader.IsDBNull(2)) _SO.visibleName = reader.GetString(2);
            if (!reader.IsDBNull(3) && !reader.IsDBNull(4) && !reader.IsDBNull(5)) _SO.position = new Vector3(reader.GetFloat(3), reader.GetFloat(4), reader.GetFloat(5));
            if (!reader.IsDBNull(6) && !reader.IsDBNull(7) && !reader.IsDBNull(8) && !reader.IsDBNull(9)) _SO.rotation = new Quaternion(reader.GetFloat(6), reader.GetFloat(7), reader.GetFloat(8), reader.GetFloat(9));
            if (!reader.IsDBNull(10)) _SO.speed = reader.GetFloat(10);
            //if (!reader.IsDBNull(11))returnSO.prefab_path=reader.GetString(11);
            switch (_type)
            {
                case 1:
                    _SO.type = ServerObject.typeSO.ship;
                    break;
                case 2:
                    _SO.type = ServerObject.typeSO.station;
                    break;
                case 3:
                    _SO.type = ServerObject.typeSO.asteroid;
                    break;
            }
            returnSOList.Add(_SO);
        }

        return returnSOList;
    }

    public SO_shipData GetSOShipData(int SO_id){
        SO_shipData retShipData= new SO_shipData();
        string qwery = @"SELECT 
                       SO_id,
                       max_speed,
                       rotation_speed,
                       acceleration_max,
                       newSpeed,
                       hull_full,
                       armor_full,
                       shield_full,
                       capasitor_full,
                       hull,
                       armor,
                       shield,
                       capasitor,
                       hull_restore,
                       armor_restore,
                       shield_restore,
                       capasitor_restore,
                       agr_distance,
                       vision_distance,
                       destroyed,
                       hidden,
                       mob,
                       warpDriveStartTime,
                       warpSpeed
                FROM SO_shipdata
                WHERE SO_id=" + SO_id.ToString();



        GetReader (qwery);
        while (reader.Read())
        {

//            if (!reader.IsDBNull(0)) { int _id = reader.GetInt32(0); };
            if (!reader.IsDBNull(1)) retShipData.max_speed = reader.GetFloat(1);
            if (!reader.IsDBNull(2)) retShipData.rotation_speed = reader.GetFloat(2);
            if (!reader.IsDBNull(3)) retShipData.acceleration_max = reader.GetFloat(3);
            if (!reader.IsDBNull(4)) retShipData.newSpeed = reader.GetFloat(4);
            if (!reader.IsDBNull(5)) retShipData.hull_full = reader.GetFloat(5);
            if (!reader.IsDBNull(6)) retShipData.armor_full = reader.GetFloat(6);
            if (!reader.IsDBNull(7)) retShipData.shield_full = reader.GetFloat(7);
            if (!reader.IsDBNull(8)) retShipData.capasitor_full = reader.GetFloat(8);
            if (!reader.IsDBNull(9)) retShipData.hull = reader.GetFloat(9);
            if (!reader.IsDBNull(10)) retShipData.armor = reader.GetFloat(10);
            if (!reader.IsDBNull(11)) retShipData.shield = reader.GetFloat(11);
            if (!reader.IsDBNull(12)) retShipData.capasitor = reader.GetFloat(12);
            if (!reader.IsDBNull(13)) retShipData.hull_restore = reader.GetFloat(13);
            if (!reader.IsDBNull(14)) retShipData.armor_restore = reader.GetFloat(14);
            if (!reader.IsDBNull(15)) retShipData.shield_restore = reader.GetFloat(15);
            if (!reader.IsDBNull(16)) retShipData.capasitor_restore = reader.GetFloat(16);
            if (!reader.IsDBNull(17)) retShipData.agr_distance = reader.GetFloat(17);
            if (!reader.IsDBNull(18)) retShipData.vision_distance = reader.GetFloat(18);
            if (!reader.IsDBNull(19)) retShipData.destroyed = (reader.GetInt32(19) == 1);
            if (!reader.IsDBNull(20)) retShipData.hidden = (reader.GetInt32(20) == 1);
            if (!reader.IsDBNull(21)) retShipData.mob = (reader.GetInt32(21) == 1);
            if (!reader.IsDBNull(22)) retShipData.warpDriveStartTime = reader.GetFloat(22);
            if (!reader.IsDBNull(23)) retShipData.warpSpeed = reader.GetFloat(23);
        }
        retShipData.SO= GetServerObject(SO_id);

        return retShipData;
    }

    public List<SO_shipData> GetAllShips()
    {
        List<SO_shipData> retListShip = new List<SO_shipData>();

        string qwery = @"SELECT 
                            server_objects.id,
                            server_objects.type,
                            server_objects.visibleName,
                            server_objects.position_x,
                            server_objects.position_y,
                            server_objects.position_z,
                            server_objects.rotation_x,
                            server_objects.rotation_y,
                            server_objects.rotation_z,
                            server_objects.rotation_w,
                            server_objects.speed,
                            server_objects.prefab_path,
                            SO_shipdata.SO_id,
                            SO_shipdata.max_speed,
                            SO_shipdata.rotation_speed,
                            SO_shipdata.acceleration_max,
                            SO_shipdata.newSpeed,
                            SO_shipdata.hull_full,
                            SO_shipdata.armor_full,
                            SO_shipdata.shield_full,
                            SO_shipdata.capasitor_full,
                            SO_shipdata.hull,
                            SO_shipdata.armor,
                            SO_shipdata.shield,
                            SO_shipdata.capasitor,
                            SO_shipdata.hull_restore,
                            SO_shipdata.armor_restore,
                            SO_shipdata.shield_restore,
                            SO_shipdata.capasitor_restore,
                            SO_shipdata.agr_distance,
                            SO_shipdata.vision_distance,
                            SO_shipdata.destroyed,
                            SO_shipdata.hidden,
                            SO_shipdata.mob,
                            SO_shipdata.warpDriveStartTime,
                            SO_shipdata.warpSpeed
                        FROM SO_shipdata INNER JOIN server_objects
                        ON SO_shipdata.SO_id=server_objects.id";
        GetReader(qwery);
        while (reader.Read())
        {
            SO_shipData retShipData = new SO_shipData();
            int _type = 0;

            if (!reader.IsDBNull(0)) retShipData.SO.id = reader.GetInt32(0);
            if (!reader.IsDBNull(1)) { _type = reader.GetInt32(1); }
            if (!reader.IsDBNull(2)) retShipData.SO.visibleName = reader.GetString(2);
            if (!reader.IsDBNull(3) && !reader.IsDBNull(4) && !reader.IsDBNull(5)) retShipData.SO.position = new Vector3(reader.GetFloat(3), reader.GetFloat(4), reader.GetFloat(5));
            if (!reader.IsDBNull(6) && !reader.IsDBNull(7) && !reader.IsDBNull(8) && !reader.IsDBNull(9)) retShipData.SO.rotation = new Quaternion(reader.GetFloat(6), reader.GetFloat(7), reader.GetFloat(8), reader.GetFloat(9));
            if (!reader.IsDBNull(10)) retShipData.SO.speed = reader.GetFloat(10);
            //if (!reader.IsDBNull(11))retShipData.SO.prefab_path=reader.GetString(11);

//            if (!reader.IsDBNull(12)) { int _id = reader.GetInt32(12); };
            if (!reader.IsDBNull(13)) retShipData.max_speed = reader.GetFloat(13);
            if (!reader.IsDBNull(14)) retShipData.rotation_speed = reader.GetFloat(14);
            if (!reader.IsDBNull(15)) retShipData.acceleration_max = reader.GetFloat(15);
            if (!reader.IsDBNull(16)) retShipData.newSpeed = reader.GetFloat(16);
            if (!reader.IsDBNull(17)) retShipData.hull_full = reader.GetFloat(17);
            if (!reader.IsDBNull(18)) retShipData.armor_full = reader.GetFloat(18);
            if (!reader.IsDBNull(19)) retShipData.shield_full = reader.GetFloat(19);
            if (!reader.IsDBNull(20)) retShipData.capasitor_full = reader.GetFloat(20);
            if (!reader.IsDBNull(21)) retShipData.hull = reader.GetFloat(21);
            if (!reader.IsDBNull(22)) retShipData.armor = reader.GetFloat(22);
            if (!reader.IsDBNull(23)) retShipData.shield = reader.GetFloat(23);
            if (!reader.IsDBNull(24)) retShipData.capasitor = reader.GetFloat(24);
            if (!reader.IsDBNull(25)) retShipData.hull_restore = reader.GetFloat(25);
            if (!reader.IsDBNull(26)) retShipData.armor_restore = reader.GetFloat(26);
            if (!reader.IsDBNull(27)) retShipData.shield_restore = reader.GetFloat(27);
            if (!reader.IsDBNull(28)) retShipData.capasitor_restore = reader.GetFloat(28);
            if (!reader.IsDBNull(29)) retShipData.agr_distance = reader.GetFloat(29);
            if (!reader.IsDBNull(30)) retShipData.vision_distance = reader.GetFloat(30);
            if (!reader.IsDBNull(31)) retShipData.destroyed = (reader.GetInt32(31) == 1);
            if (!reader.IsDBNull(32)) retShipData.hidden = (reader.GetInt32(32) == 1);
            if (!reader.IsDBNull(33)) retShipData.mob = (reader.GetInt32(33) == 1);
            if (!reader.IsDBNull(34)) retShipData.warpDriveStartTime = reader.GetFloat(34);
            if (!reader.IsDBNull(35)) retShipData.warpSpeed = reader.GetFloat(35);
            
            switch (_type)
            {
                case 1:
                    retShipData.SO.type = ServerObject.typeSO.ship;
                    break;
                case 2:
                    retShipData.SO.type = ServerObject.typeSO.station;
                    break;
                case 3:
                    retShipData.SO.type = ServerObject.typeSO.asteroid;
                    break;
            }
            retListShip.Add(retShipData);
        }
		for (int i = 0; i < retListShip.Count; i++) {
			retListShip [i].weapons=GetWeapons (retListShip [i].SO.id);
			retListShip[i].equipments=GetEquip(retListShip [i].SO.id);
		}
        return retListShip;
    }

	private List<SO_equipmentData> GetEquip(int Ship_id){
		List<SO_equipmentData> retList = new List<SO_equipmentData> ();
		return retList;
	}

	private List<SO_weaponData> GetWeapons (int ship_id){
		List<SO_weaponData> retList = new List<SO_weaponData> ();
		SO_weaponData _weapon;
		string qwery = @"SELECT 
					SO_id ,
 					WeaponType, 
					active, 
					damage, 
					reload, 
					ammoSpeed, 
					activeTime, 
					sqrDistanse_max, 
					capasitor_use
					FROM SO_weapondata
					WHERE SO_id=" + ship_id.ToString();
		GetReader(qwery);
		while (reader.Read ()) {
			int _type = 0;
			_weapon = new SO_weaponData ();

//			if (!reader.IsDBNull(0)) _weapon. = reader.GetInt32(0);
			if (!reader.IsDBNull (1))_type = reader.GetInt32 (1);
			if (!reader.IsDBNull (2))_weapon.active = (reader.GetInt32(2) == 1);
			if (!reader.IsDBNull (3))_weapon.damage = reader.GetFloat (3);
			if (!reader.IsDBNull (4))_weapon.reload = reader.GetFloat (4);
			if (!reader.IsDBNull (5))_weapon.ammoSpeed = reader.GetFloat (5);
			if (!reader.IsDBNull (6))_weapon.activeTime = reader.GetFloat (6);
			if (!reader.IsDBNull (7))_weapon.sqrDistanse_max = reader.GetFloat (7);
			if (!reader.IsDBNull (8))_weapon.capasitor_use = reader.GetFloat (8);
			switch (_type)
			{
			case 1:
				_weapon.type = SO_weaponData.WeaponType.laser;
				break;
			case 2:
				_weapon.type = SO_weaponData.WeaponType.missile;
				break;
			case 3:
				_weapon.type = SO_weaponData.WeaponType.projective;
				break;
			}
			retList.Add (_weapon);
		}
		return retList;

		
	}

    public ServerObject AddNewSO(Vector3 position, Quaternion rotation, int item_id)
    {
		int new_id = 0;
        Item _item= GetComponent<InventoryServer>().GetItem(item_id);
        if (_item.itemType==Item.Type_of_item.ship){
            string qwery = "insert into server_objects (type,visibleName,position_x,position_y,position_z,rotation_x,rotation_y,rotation_z,rotation_w,speed) values (1,"+
                _item.item+", "+
                position.x + ", " +
                position.y + ", " +
                position.z + ", " +
                rotation.x + ", " +
                rotation.y + ", " +
                rotation.z+ ", " +
                rotation.w + ", " +
                "0, " +
                ") ";

            GetReader(qwery);
            new_id=lastId("server_objects");
           
        }
        return GetServerObject(new_id);
    }
   
    public SO_shipData AddNewSOShip(Vector3 position, Quaternion rotation, int item_id )
    {
        ServerObject SO = AddNewSO(position, rotation, item_id);
        SO_shipData retSOShipData;
        if (SO.type== ServerObject.typeSO.ship)
        {
            ShipItem _shipItem = GetComponent<InventoryServer>().GetShipItem(item_id);

            string qwery = @"insert into SO_shipdata (
                   SO_id,
                   max_speed,
                   rotation_speed,
                   acceleration_max,
                   newSpeed,
                   hull_full,
                   armor_full,
                   shield_full,
                   capasitor_full,
                   hull,
                   armor,
                   shield,
                   capasitor,
                   hull_restore,
                   armor_restore,
                   shield_restore,
                   capasitor_restore,
                   agr_distance,
                   vision_distance,
                   mob,
                   warpDriveStartTime,
                   warpSpeed"+
                ") values ("+
                   SO.id.ToString()+", "+
                   _shipItem.max_speed.ToString()+", "+
                   _shipItem.rotation_speed.ToString()+", "+
                   _shipItem.acceleration_max.ToString()+", "+
                   _shipItem.newSpeed.ToString()+", "+
                   _shipItem.hull_full.ToString()+", "+
                   _shipItem.armor_full.ToString()+", "+
                   _shipItem.shield_full.ToString()+", "+
                   _shipItem.capasitor_full.ToString()+", "+
                   _shipItem.hull.ToString()+", "+
                   _shipItem.armor.ToString()+", "+
                   _shipItem.shield.ToString()+", "+
                   _shipItem.capasitor.ToString()+", "+
                   _shipItem.hull_restore.ToString()+", "+
                   _shipItem.armor_restore.ToString()+", "+
                   _shipItem.shield_restore.ToString()+", "+
                   _shipItem.capasitor_restore.ToString()+", "+
                   _shipItem.agr_distance.ToString()+", "+
                   _shipItem.vision_distance.ToString()+", "+
                   _shipItem.mob.ToString()+", "+
                   _shipItem.warpDriveStartTime.ToString()+", "+
                   _shipItem.warpSpeed.ToString()+")";
        
            GetReader(qwery);
        }
		retSOShipData=GetSOShipData(SO.id);
        return retSOShipData;
    }

    private void DeleteServerObject(int SO_id)
    {
        string qwery = "delete FROM server_objects where id="+SO_id.ToString(); 
        GetReader(qwery);
    }

    public void DeleteSOShipData(int SO_id)
    {
        string qwery = "delete FROM SO_shipdata where id=" + SO_id.ToString();
        GetReader(qwery);
        DeleteServerObject(SO_id);
    }

    private int lastId(string dbName)
    {
        string qwery = "SELECT id FROM "+ dbName + " WHERE rowid=last_insert_rowid()";
        GetReader(qwery);
        int ret_id = 0;
        while (reader.Read()){
			if (!reader.IsDBNull(0)) ret_id=reader.GetInt32( 0);
        }
        return ret_id;
    }

}