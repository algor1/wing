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

    public ServerObject AddNewSO(Vector3 position, Quaternion rotation, float speed, int item_id)
    {
        Item _item= GetComponent<InventoryServer>().GetItem(item_id);
        if (_item.itemType==Item.Type_of_item.ship){
            ShipItem _shipItem=GetComponent<InventoryServer>().GetShipItem(item_id);
            string qwery = "insert into.... ";
            GetReader(qwery);
            lastId();
        }

        
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
