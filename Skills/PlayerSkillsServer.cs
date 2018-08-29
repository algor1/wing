using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


//три таблицы 
//skillsDB - список всех скилов
//playerSkills - список изученных скилов
//playerSkillQueue - список изучаемых  сейчас скилов очередь до 3-х дней, мах 10 скилов

    
class PlayerSkillsServer: MonoBehaviour
{
    private Dictionary<int, Dictionary<int,Dictionary<int, long>>> playersSkillsBase; //StationLandingBase[player_id][skill_id][tech]points
  
//    [SerializeField]
//    private SkillsDatabase skillsDB;
    private Dictionary<int, List<Skill>> playerSkillQueue;


        
    void Awake(){
        DontDestroyOnLoad(transform.gameObject);
//        playersSkillsBase = new Dictionary<int, Dictionary<int,Dictionary<int, long>>>();
//        InitPlayer(0);

    }
//    public void InitPlayer(int player_id)
//    {
//        if (!playersSkillsBase.ContainsKey(player_id))
//        {
//            playersSkillsBase.Add(player_id, new Dictionary<int,Dictionary<int, long>>());
//                
//            for (int i = 0; i < skillsDB.allskills.Count; i++)
//            {
//                if (skillsDB.allskills[i].rootSkill)
//                {
//                    //if (!playersSkillsBase[player_id].ContainsKey(player_id))
//                    playersSkillsBase[player_id].Add(skillsDB.allskills[i].id, new Dictionary<int, long>{{1,0}});
//                        
//                }
//            }
//        }
//    }

    public long SkillLearned(int player_id,int skill_id,int _tech){
	long p = GetComponent<SkillsDB> ().SkillLearnedPoints (player_id, skill_id, _tech);
        return p;
        }

    public Skill SkillFind(int skill_id){
        Skill _skill = GetComponent<SkillsDB>().FindSkill(skill_id);
        return _skill;
    }
	public List<SkillQueue> PlayerSkillQueue(int player_id)
    {
        return GetComponent<SkillsDB>().PlayerQueue(player_id);
    }
	public bool AddSkillToQueue(int player_id, int skill_id, int _tech,int level)
    {
        List<SkillQueue> _PSQ = PlayerSkillQueue(player_id);
        for (int i = 0; i < _PSQ.Count; i++)
        {
			if (_PSQ[i].skill_id == skill_id && _PSQ[i].tech == _tech && _PSQ[i].level == level)
            {
                return false;
            }
        }
        GetComponent<SkillsDB>().AddToQueue(player_id, skill_id, _tech, level);
        return true;
    }
	public bool DleleteSkillFromQueue(int player_id, int skill_id, int _tech, int level)
    {
        List<SkillQueue> _PSQ = PlayerSkillQueue(player_id);
        for (int i = 0; i < _PSQ.Count; i++)
        {
			if (_PSQ[i].skill_id == skill_id && _PSQ[i].tech == _tech &&_PSQ[i].level==level)
            {
				GetComponent<SkillsDB>().DeleteFromQueue(player_id, skill_id, _tech,level);
                return true;
            }
        }
        return false;
    }




    public Dictionary<int, Dictionary<int, long>> AllPlayerSkills(int player_id)
	{
		return playersSkillsBase [player_id];
	}

    public int SkillLevelCalc(int _tech, int _difficulty, long _points)
    {
        int level= Mathf.FloorToInt(Mathf.Log(_points, _difficulty*3+_tech-1));
        return level;
    }

    
	


    private void CheckDepSkill(int player_id,int skill_id,int _tech)
    {

    }

	private void UpdatePlayerSkill(int player_id,SkillQueue skillq, int pointsAdd)
    {
		Skill _skill = SkillFind (skillq.skill_id);
		long _points = SkillLearned (player_id, skillq.skill_id, skillq.tech);


			GetComponent<SkillsDB>().AddPointsToSkill(0, _skill.id, _skill.tech, pointsAdd);
		
		if (skillq.level<=SkillLevelCalc(skillq.tech,_skill.difficulty,_points)){
			GetComponent<SkillsDB> ().DeleteFromQueue (0, skillq.skill_id, skillq.tech, skillq.level);
		}
    }

    private IEnumerator UpdateSkills()
	{
		while (true) {
            //foreach player_id
                int playerLearningSpeed = 1;

                List<SkillQueue> _PSQ = PlayerSkillQueue(0); //пока только один плеер
                for (int i = 0; i < _PSQ.Count; i++)
				{
				if (_PSQ[i].queue==1) UpdatePlayerSkill (0,_PSQ[i],playerLearningSpeed);

                }
			yield return new WaitForSeconds (10f);
	
		}


        
    }
}
