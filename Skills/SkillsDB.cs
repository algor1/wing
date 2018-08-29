

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
	private IDbCommand dbcmd;

    void Start()
    {
//        InitDB();
    }
	void OnApplicationQuit()
	{
		if (dbSkillCon!=null) dbSkillCon.Close();
	}
    private void InitDB()
    {
		string p="skill.db";
		string filepath = Application.persistentDataPath + "/" + p;
		Debug.Log (filepath);
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
		string qwery = "select id,skill,sprite_path,difficulty,depend_id,depend_tech,rootskill from skills";
		GetReader (qwery);
//		Debug.Log (dbSkillCon.Database);
//		Debug.Log (dbcmd.CommandText);
//
//		Debug.Log (reader.Read ());

		while (reader.Read()){
            
			Skill _skill= new Skill();
			if (!reader.IsDBNull(0))_skill.id=reader.GetInt32( 0);
            Debug.Log("reading skill : "+_skill.id);
			if (!reader.IsDBNull(1)) _skill.skill=reader.GetString(1);
//			Debug.Log (_skill.skill);
			if (!reader.IsDBNull(2)) _skill.sprite_path=reader.GetString(2);
//			Debug.Log (_skill.sprite_path);
			if (!reader.IsDBNull(3))_skill.difficulty=reader.GetInt32( 3);
			if (!reader.IsDBNull(4))_skill.depend_id=reader.GetInt32( 4);//skill_id
			if (!reader.IsDBNull(5))_skill.depend_tech=reader.GetInt32( 5);
			if (!reader.IsDBNull(6))_skill.rootSkill= (reader.GetInt32(6)==1);
            
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
		Debug.Log (reader.Read ());
            while (reader.Read())
            {

                Skill _skill = new Skill();
                _skill.id = reader.GetInt32(0);
			if (!reader.IsDBNull(1)) _skill.skill = reader.GetString(1);
			if (!reader.IsDBNull(2))_skill.sprite_path = reader.GetString(2);
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

    public List<SkillQueue> PlayerQueue(int player_id)
    {
        List<SkillQueue> retQueue = new List<SkillQueue>();
        string qwery = "select skill_id,tech,queue from skill_queue where player_id="+player_id.ToString();
        GetReader(qwery);
        SkillQueue _skillQueue= new SkillQueue();
        while (reader.Read())
        {
			Debug.Log ("reader.read");
            if (!reader.IsDBNull(0))
            {
				Debug.Log ("reader0 "+reader.GetInt32(0));
                if (!reader.IsDBNull(0)) _skillQueue.skill = FindSkill(reader.GetInt32(0));
				Debug.Log ("reader1 "+reader.GetInt32(1));
                if (!reader.IsDBNull(1)) _skillQueue.skill.tech = reader.GetInt32(1);
				Debug.Log ("reader2 "+reader.GetInt32(2));
                if (!reader.IsDBNull(2)) _skillQueue.queue = reader.GetInt32(2);
                _skillQueue.skill.points = SkillLearnedPoints(player_id, _skillQueue.skill.id, _skillQueue.skill.tech);
                retQueue.Add(_skillQueue);
            }
        }
        return retQueue;
    }

    public void AddToQueue(int player_id,int skill_id,int tech, int level)
    {
		string qwery = "insert into skill_queue (player_id,skill_id,tech,level) values ("+player_id.ToString()+","+skill_id.ToString()+","+tech.ToString()+","+level.ToString()+")";
        GetReader(qwery);
    }
	public void DeleteFromQueue(int player_id,int skill_id,int tech,int level)
    {
		string qwery = "delete from skill_queue where (player_id = "+player_id.ToString()+" and skill_id="+skill_id.ToString()+" and tech="+tech.ToString()+"and level="+ level.ToString()+")" ;
        GetReader(qwery);
    }
	public void AddPointsToSkill(int player_id,int skill_id,int tech, int _points)
	{
		string qwery =  "select skill_id from player_skills where skill_id="+skill_id.ToString()+" and player_id="+player_id.ToString()+" and tech=" + tech.ToString();
		GetReader (qwery);
		if (reader.IsDBNull (0)) 
		{
			qwery = "insert into player_skills (player_id,skill_id,tech,points) values (" + player_id.ToString () + "," + skill_id.ToString () + "," + tech.ToString () + ",0)";
			GetReader (qwery);
		}
		qwery = "update player_skills set points= points+" + _points.ToString() + " where player_id=" + player_id.ToString () + " and skill_id=" + skill_id.ToString () + "  and tech=" + tech.ToString () + " )";
		GetReader (qwery);
	}

    public Skill FindSkill(int skill_id)
    {
        Skill returnSkill=new Skill();
		string qwery = "select id,skill,sprite_path,difficulty,depend_id,depend_tech,rootSkill from skills where id="+skill_id.ToString();
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
		if (dbSkillCon==null){
			InitDB();
		}
//		Debug.Log (dbSkillCon.Database);
		dbcmd = dbSkillCon.CreateCommand ();
//		Debug.Log (dbcmd.CommandText);
        dbcmd.CommandText = dbselect;
            // Выполняем запрос
		reader = dbcmd.ExecuteReader();
//		Debug.Log (reader.Read());
         
                return true;
        


      
//        return false;
    }

}