

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
// Подключаем необходимые пространства имен
using System.Data;

using Mono.Data.Sqlite;
using System.IO;
using System.Collections.Generic;

public class SkillsDB : MonoBehaviour
	
{
	[SerializeField]
	private Text debugtext;
    private IDbConnection dbSkillCon;
    private IDataReader reader;

    void Start()
    {
        InitDB();
    }
    
    private void InitDB()
    {
		string p="skill.db";
		string filepath = Application.persistentDataPath + "/" + p;
		if (!File.Exists (filepath)) 
        {
			WWW loadDB = new WWW ( "jar:file://" + Application.dataPath + "!/assets/"+ p);  // this is the path to your StreamingAssets in android
			while (!loadDB.isDone) 
            {
				Debug.Log ("LOADING..."+ loadDB.uploadProgress);
			}
			File.WriteAllBytes (filepath, loadDB.bytes);
		}
		string connectionString = "URI=file:"+filepath ;
        dbSkillCon = (IDbConnection)new SqliteConnection(connectionString);
        dbSkillCon.Open();
    }

    public List<Skill> GetAllSkills()
    { 
        List<Skill> returnList=new List<Skill>();
		string qwery = "select id,skill,sprite_path,difficulty,depend_id,depend_tech,rootSkill from skills";
		GetReader (qwery);
		while (reader.Read()){

			Skill _skill= new Skill();
            _skill.id=reader.GetInt32( 0);
            _skill.skill=reader.GetString(1);
            _skill.sprite_path=reader.GetString(2);
            _skill.difficulty=reader.GetInt32( 3);
            _skill.depend_id=reader.GetInt32( 4);//skill_id
            _skill.depend_tech=reader.GetInt32( 5);
            _skill.rootSkill=reader.GetBoolean( 6);
            
            returnList.Add(_skill);
            
            //const string frmt = "Name: {0}; Cost: {1}; ";
            //debugtext.text = string.Format(frmt,
            //    reader.GetInt32( 0),
            //    reader.GetString(1)
//				//							reader.GetBoolean(2),
//				//							reader.GetInt32(3)
            //);
//			GetComponent<Text> ().text = debugtext;

		
        }

		return returnList; 
    }
  
    public List<Skill> GetPlayerSkills(int _player_id)
    {
            string qwery = "select skills.id,skills.skill,skills.sprite_path,skills.difficulty,skills.depend_id,skills.depend_tech,skills.rootSkill,tech,points from player_skills inner join skills where skill_id=skills.id and player_id=0";
   
            List<Skill> returnList = new List<Skill>();

            GetReader(qwery);
            while (reader.Read())
            {

                Skill _skill = new Skill();
                _skill.id = reader.GetInt32(0);
                _skill.skill = reader.GetString(1);
                _skill.sprite_path = reader.GetString(2);
                _skill.difficulty = reader.GetInt32(3);
                _skill.depend_id = reader.GetInt32(4);//skill_id
                _skill.depend_tech = reader.GetInt32(5);
                _skill.rootSkill = reader.GetBoolean(6); ;
                _skill.tech = reader.GetInt32(7);
                _skill.points = reader.GetInt32(8);
                returnList.Add(_skill);

                //const string frmt = "Name: {0}; Cost: {1}; ";
                //debugtext.text = string.Format(frmt,
                //    reader.GetInt32( 0),
                //    reader.GetString(1)
                //				//							reader.GetBoolean(2),
                //				//							reader.GetInt32(3)
                //);
                //			GetComponent<Text> ().text = debugtext;


            }

            return returnList; 


    }
   
    public long SkillLearnedPoints(int _player_id,int _skill_id,int _tech)
    {
        string qwery = "select points from player_skills where skill_id="+_skill_id+" and player_id="+_player_id+" and tech=" + _tech;
   
        long returnLong =0;

        GetReader(qwery);
        while (reader.Read())
        {
            returnLong= reader.GetInt64(0);
        }
        return returnLong;
    }

    public Skill FindSkill(int skill_id)
    {
        Skill returnSkill=new Skill();
		string qwery = "select id,skill,sprite_path,difficulty,depend_id,depend_tech,rootSkill from skills where id="+skill_id;
		GetReader (qwery);
        while (reader.Read())
        {
            returnSkill.id = reader.GetInt32(0);
            returnSkill.skill = reader.GetString(1);
            returnSkill.sprite_path = reader.GetString(2);
            returnSkill.difficulty = reader.GetInt32(3);
            returnSkill.depend_id = reader.GetInt32(4);//skill_id
            returnSkill.depend_tech = reader.GetInt32(5);
            returnSkill.rootSkill = reader.GetBoolean(6);
        }
        return returnSkill; 
    }

    private bool GetReader(string dbselect)
    {

        //	var sql = "SELECT * FROM skills LIMIT 1";
        using (IDbCommand dbcmd = dbSkillCon.CreateCommand())
        {
            dbcmd.CommandText = dbselect;
            // Выполняем запрос
            using (IDataReader reader = dbcmd.ExecuteReader())
            {
                return true;
            }


        }
        return false;
    }

}