using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using System.Collections.Generic;

public class ItemDB : MonoBehaviour
{
    private IDbConnection dbSkillCon;
    private IDataReader reader;
    private IDbCommand dbcmd;



    private void InitDB()
    {
        string p = "inventory.db";
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


//    public Item GetItem (int item_id)
//    {
//        Skill returnItem = new Item();
//        string qwery = "select id,skill,sprite_path,difficulty,depend_id,depend_tech,rootSkill from skills where id=" + skill_id.ToString();
//        GetReader(qwery);
//        while (reader.Read())
//        {
//            returnItem.id = reader.GetInt32(0);
//            returnItem.skill = reader.GetString(1);
//            returnItem.sprite_path = reader.GetString(2);
//            returnItem.difficulty = reader.GetInt32(3);
//            returnItem.depend_id = reader.GetInt32(4);//skill_id
//            returnItem.depend_tech = reader.GetInt32(5);
//            returnItem.rootSkill = reader.GetBoolean(6);
//        }
//        return returnItem;
//    }


}