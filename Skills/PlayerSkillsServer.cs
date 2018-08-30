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
  
        [SerializeField]
        private SkillsDatabase skillsDB;
        private Dictionary<int, List<Skill>> playerSkillQueue;


        
        void Awake(){
            DontDestroyOnLoad(transform.gameObject);
            playersSkillsBase = new Dictionary<int, Dictionary<int,Dictionary<int, long>>>();
            InitPlayer(0);

        }
        public void InitPlayer(int player_id)
        {
            if (!playersSkillsBase.ContainsKey(player_id))
            {
                playersSkillsBase.Add(player_id, new Dictionary<int,Dictionary<int, long>>());
                
                for (int i = 0; i < skillsDB.allskills.Count; i++)
                {
                    if (skillsDB.allskills[i].rootSkill)
                    {
                        //if (!playersSkillsBase[player_id].ContainsKey(player_id))
                        playersSkillsBase[player_id].Add(skillsDB.allskills[i].id, new Dictionary<int, long>{{1,0}});
                        
                    }
                }
            }
        }

        public long SkillLearned(int player_id,int skill_id,int _tech){
            if (playersSkillsBase.ContainsKey(player_id))
            {
                if (playersSkillsBase[player_id].ContainsKey(skill_id))
                {
                    if (playersSkillsBase[player_id][skill_id].ContainsKey(_tech))
                    {
                        return playersSkillsBase[player_id][skill_id][_tech];
                    }
                }
            }
           return 0L;
        }

    public Skill SkillFind(int skill_id){
        Skill _skill = GetComponent<SkillsDB>().FindSkill(skill_id);
        return _skill;
    }

	public Dictionary<int,Dictionary<int, long>> AllPlayerSkills( int player_id)
	{
		return playersSkillsBase [player_id];
	}

    public bool AddSkillToQueue(int player_id,int skill_id,int _tech){
//        long points=SkillLearned(player_id,skill_id,_tech);
        if (!playerSkillQueue.ContainsKey(player_id))
        {
            ////?????? check if here


            playerSkillQueue.Add(player_id, new List<Skill>());
//            Skill newskill = new Skill(SkillFind(skill_id), _tech);
        }
        
        return false;
    }
    public int SkillLevelCalc(int _tech, int difficulty, long _points)
    {
        int level= Mathf.FloorToInt(Mathf.Log(_points, _difficulty*3+_tech-1));
        return level;
    }

        private void LoadData(){
        }

        private void SaveData(){

        }
	
   private float LearningTime(int dificulty,int tech,int level){
       float retTime = Mathf.Pow((3*dificulty+tech-1),level);
       return retTime;
   }

    private void CheckDepSkill(int player_id,int skill_id,int _tech)
    {
//        if 
    }

    private void UpdatePlayerSkill(int player_id,int skill_id,int _tech, int volume)
    {
         if (!playersSkillsBase.ContainsKey(player_id))
         {
             InitPlayer(player_id);
         }

    }

    private IEnumerator UpdateSkills()
	{
		while (true) {



				UpdatePlayerSkill (0,0,0,1);



			Debug.Log(" забираем инфу с сервера ???");	
//			UpdateSOFromServer ();
//			//		print ("1");
//			//Создаем копию dict nearestShips чтобы узнать какие нужно удалить
//			Dictionary<int,int> SOsForDeletionList = new Dictionary<int,int> ();
//			foreach (int key in nearestSOs.Keys) { 
//				SOsForDeletionList.Add (key, key);
//				//				Debug.Log (shipsForDeletionList);
//				//				print (key);
//			}	
//
//			//		Debug.Log (serverShipslist.Count);
//			for (int i = 0; i < serverSOlist.Count; i++) {
//				//print(serverShipslist[i].p.id);
//				if (SOsForDeletionList.Remove (serverSOlist [i].id)) {
//					UpdateSO (serverSOlist [i]);
//					//print ("4");
//				} else {
//					print ("add ship");
//					AddSO	(serverSOlist [i]);
//					UpdateSO (serverSOlist [i]);			
//				}	
//
//			}
//			//удаляем корабли которых небыло в списке
//			foreach (int key in SOsForDeletionList.Keys) { 
//				DeleteSO (key);


			
			yield return new WaitForSeconds (10f);
			Debug.Log ("!!!!!!!!!!!!!!!! ended !!!!!!!!!!!!!!!!");
		}


        
    }
}
