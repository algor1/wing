using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using System.Collections.Generic;


public class DB : MonoBehaviour {
    private IDbConnection dbSkillCon;
    private IDataReader reader;
    private IDbCommand dbcmd;


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
            if (!reader.IsDBNull(1)){ int _type = reader.GetInt32(1);}
            if (!reader.IsDBNull(2))returnSO.visibleName=reader.GetString(2);
            if (!reader.IsDBNull(3)&&!reader.IsDBNull(4)&&!reader.IsDBNull(5)) returnSO.position=new Vector3 (reader.GetFloat(3),reader.GetFloat(4),reader.GetFloat(5));
			if (!reader.IsDBNull(6)&&!reader.IsDBNull(7)&&!reader.IsDBNull(8)&&!reader.IsDBNull(9)) returnSO.rotation=new Quaternion (reader.GetFloat(6),reader.GetFloat(7),reader.GetFloat(8),reader.GetFloat(9));
            if (!reader.IsDBNull(0))returnSO.speed=reader.GetFloat(10);
            //if (!reader.IsDBNull(0))returnSO.prefab_path=reader.GetString(11);
        }
        return returnSO; 
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
            if (!reader.IsDBNull(0)){ int _id = reader.GetInt32(0);};
            if (!reader.IsDBNull(2))retShipData.max_speed=reader.GetFloat(1);
            if (!reader.IsDBNull(2))retShipData.rotation_speed=reader.GetFloat(2);
            if (!reader.IsDBNull(2))retShipData.acceleration_max=reader.GetFloat(3);
            if (!reader.IsDBNull(2))retShipData.newSpeed=reader.GetFloat(4);
            if (!reader.IsDBNull(2))retShipData.hull_full=reader.GetFloat(5);
            if (!reader.IsDBNull(2))retShipData.armor_full=reader.GetFloat(6);
            if (!reader.IsDBNull(2))retShipData.shield_full=reader.GetFloat(7);
            if (!reader.IsDBNull(2))retShipData.capasitor_full=reader.GetFloat(8);
            if (!reader.IsDBNull(2))retShipData.hull=reader.GetFloat(9);
            if (!reader.IsDBNull(2))retShipData.armor=reader.GetFloat(10);
            if (!reader.IsDBNull(2))retShipData.shield=reader.GetFloat(11);
            if (!reader.IsDBNull(2))retShipData.capasitor=reader.GetFloat(12);
            if (!reader.IsDBNull(2))retShipData.hull_restore=reader.GetFloat(13);
            if (!reader.IsDBNull(2))retShipData.armor_restore=reader.GetFloat(14);
            if (!reader.IsDBNull(2))retShipData.shield_restore=reader.GetFloat(15);
            if (!reader.IsDBNull(2))retShipData.capasitor_restore=reader.GetFloat(16);
            if (!reader.IsDBNull(2))retShipData.agr_distance=reader.GetFloat(17);
            if (!reader.IsDBNull(2))retShipData.vision_distance=reader.GetFloat(18);
			if (!reader.IsDBNull(2))retShipData.destroyed=(reader.GetInt32(19)==1);
			if (!reader.IsDBNull(2))retShipData.hidden=(reader.GetInt32(20)==1);
			if (!reader.IsDBNull(2))retShipData.mob=(reader.GetInt32(21)==1);
            if (!reader.IsDBNull(2))retShipData.warpDriveStartTime=reader.GetFloat(22);
            if (!reader.IsDBNull(2))retShipData.warpSpeed=reader.GetFloat(23);

        }
        retShipData.SO= GetServerObject(SO_id);

        return retShipData;
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
