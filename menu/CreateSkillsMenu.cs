﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateSkillsMenu : MonoBehaviour {

//	[SerializeField]
	private GameObject server;
	private Dictionary<int,Dictionary<int, long>> playerSkills;
	[SerializeField]
	private List<Skill> skillsDB;
	[SerializeField]
	private GameObject panel1;
    [SerializeField]
    private GameObject panel2;
	[SerializeField]
	private GameObject panel3;
    [SerializeField]
    private GameObject buttonMenuPrefab;
    [SerializeField]
    private GameObject buttonTechMenuPrefab;
	[SerializeField]
	private GameObject panelTechInfoPopup;
	[SerializeField]
	private GameObject PopupCancelButton;
	[SerializeField]
	private GameObject PopupButton;
	[SerializeField]
	private GameObject PopupTechButton;
    [SerializeField]
    private GameObject panelQueue;
    [SerializeField]
    private GameObject buttonQueueMenuPrefab;

//	void Awake()
//	{
//		DontDestroyOnLoad(transform.gameObject);
//		gameObject.SetActive (false);
//	}

	void Start () {
		server = GameObject.Find ("ServerGo");
		Debug.Log (server.name);
        skillsDB = server.GetComponent<SkillsDB>().GetAllSkills();
        BuildMenu1();
		BuildQueueMenu ();
	}

    private void BuildMenu1 ()
    {
        foreach (Transform child in panel1.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
		int j = 0;
        for (int i = 0; i < skillsDB.Count; i++)
        {
            if (skillsDB[i].rootSkill)
            {
                MenuAdd(skillsDB[i],1,j);
				j++;	

            }
        }
    }
    private void BuildMenu2 (Skill _rootSkill)
	{
        //clear panel
        foreach (Transform child in panel2.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
		int j = 0;
		for (int i = 0; i < skillsDB.Count; i++) {
			if (skillsDB[i].depend_id ==_rootSkill.id) {
				MenuAdd (skillsDB[i],2,j);
				j++;	
			}
		}
	}
    private void BuildTechMenu(Skill _skill) {
        foreach (Transform child in panel3.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        for (int tech=1;tech>0;tech++){
            long points = server.GetComponent<PlayerSkillsServer>().SkillLearned(0, _skill.id, tech); //0-  player_id
			if (points > 0) {
				float level = SkillLevelCalc (tech, _skill.difficulty, points);
				TechMenuAdd (_skill, tech, points, level);
			} else {
                if (tech == 1) TechMenuAdd(_skill, tech, 0, 0);
				break;
			}
			if (tech > 10)
				break;
        }
        
    }
    private void BuildQueueMenu() {
        
        foreach (Transform child in panelQueue.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
		List<SkillQueue> queue = server.GetComponent<PlayerSkillsServer>().PlayerSkillQueue(0);
		Debug.Log("queue "+queue.Count);

        for (int i = 0; i < queue.Count; i++)
        {
			long points = server.GetComponent<PlayerSkillsServer>().SkillLearned(0, queue[i].skill_id, queue[i].tech); //0-  player_id
			//			Debug.Log(points+" "+_skill.id);
			Skill _skill=server.GetComponent<PlayerSkillsServer>().SkillFind(queue[i].skill_id);
			float _level = SkillLevelCalc (queue[i].tech, _skill.difficulty, points);
			QueueMenuAdd (_skill, queue[i].tech, points, _level,queue[i].queue);
        }
        }
	private void QueueMenuAdd(Skill skill, int tech,long points,float level,int pos)
	{
		GameObject menu_button = (GameObject)Instantiate(buttonQueueMenuPrefab);
		menu_button.transform.SetParent(panelQueue.transform, false);
		menu_button.GetComponent<RectTransform>().anchoredPosition = Vector3.right * ((pos-1) * +503);
		menu_button.GetComponent<Button>().onClick.AddListener(() => { DeleteFromQueuePopup(skill,tech,points,Mathf.FloorToInt( level)); });
		menu_button.GetComponentInChildren<Text>().text = "Tech: "+tech.ToString()+" Level: "+level.ToString()+" P: "+points.ToString();
		menu_button.GetComponentInChildren<Slider>().minValue =SkillPointsCalc(tech,skill.difficulty,Mathf.FloorToInt( level));
		menu_button.GetComponentInChildren<Slider>().maxValue =SkillPointsCalc(tech,skill.difficulty,Mathf.FloorToInt( level)+1);
		menu_button.GetComponentInChildren<Slider>().value =points;
	}
    private void TechMenuAdd(Skill skill, int tech,long points,float level)
    {
        GameObject menu_button = (GameObject)Instantiate(buttonTechMenuPrefab);
        menu_button.transform.SetParent(panel3.transform, false);
		menu_button.GetComponent<RectTransform>().anchoredPosition = Vector3.up * ((tech-1) * -80);
		Debug.Log (menu_button.GetComponent<RectTransform>().position);
		menu_button.GetComponent<Button>().onClick.AddListener(() => { AddToQueuePopup(skill,tech,points,Mathf.FloorToInt( level)); });
		menu_button.GetComponentInChildren<Text>().text = "Tech: "+tech.ToString()+" Level: "+level.ToString()+" P: "+points.ToString();

		menu_button.GetComponentInChildren<Slider>().maxValue =5;
		menu_button.GetComponentInChildren<Slider>().value =level;


        // TODO ON click show description
 
    }
	private void AddToQueuePopup(Skill skill, int tech,long points,int level)
	{
        if (server.GetComponent<PlayerSkillsServer>().PlayerSkillQueue(0).Count < 3)
        {
			panelTechInfoPopup.GetComponentInChildren<Text>().text = "Add to queue?";
			PopupButton.GetComponentInChildren<Text>().text = "Add to queue";
            PopupButton.GetComponent<Button>().onClick.AddListener(() => 
				{ 
					server.GetComponent<PlayerSkillsServer>().AddSkillToQueue(0, skill.id, tech, level + 1); 
					panelTechInfoPopup.SetActive (false);
					BuildQueueMenu();
				});
        }
        else
        {
			PopupButton.GetComponent<Button>().onClick.AddListener(() => 
				{ 
					Debug.Log ("cant add to queue  "+server.GetComponent<PlayerSkillsServer>().PlayerSkillQueue(0).Count);
					panelTechInfoPopup.SetActive (false);
				});
			// PopupLevelButton.GetComponent<Button>().onClick.AddListener(() =>   warinig message "cant add to queue" 
        }
//        if (SkillLevelCalc(tech, skill.difficulty, server.GetComponent<PlayerSkillsServer>().SkillLearned(0, skill.id, tech)) >= 5)
//        {
//            PopupTechButton.GetComponent<Button>().onClick.AddListener(() =>
//            {
//                server.GetComponent<PlayerSkillsServer>().AddSkillToQueue(0, skill.id, tech + 1, 1);
//				panelTechInfoPopup.SetActive (false);
//				BuildQueueMenu();
//            });
//            PopupTechButton.SetActive(true);
//        }
//        else
//        {
//            PopupTechButton.SetActive(false);
//            // PopupLevelButton.GetComponent<Button>().onClick.AddListener(() =>   warinig message "cant add to queue" 
//        }
		panelTechInfoPopup.SetActive (true);
	}
	private void DeleteFromQueuePopup(Skill skill, int tech,long points,int level)
	{
		panelTechInfoPopup.GetComponentInChildren<Text>().text = "Delete from queue?";
		PopupButton.GetComponentInChildren<Text>().text = "Delete";
		PopupButton.GetComponent<Button>().onClick.AddListener(() => 
				{ 
				server.GetComponent<PlayerSkillsServer>().DeleteSkillFromQueue(0, skill.id, tech, level + 1); 
					panelTechInfoPopup.SetActive (false);
					BuildQueueMenu();
				});
		
		panelTechInfoPopup.SetActive (true);
	}

	private void MenuAdd(Skill skill,int menulev,int buttonCount)
	{
        if (menulev == 1)
        {
            GameObject menu_button = (GameObject)Instantiate(buttonMenuPrefab);
			menu_button.transform.SetParent (panel1.transform, false);
//			menu_button.GetComponent<RectTransform>().
			menu_button.GetComponent<RectTransform>().anchoredPosition = Vector3.up * (buttonCount * -120);
			Debug.Log (" " + buttonCount + "   " + menu_button.GetComponent<RectTransform> ().localPosition);
            menu_button.GetComponent<Button>().onClick.AddListener(() => { BuildMenu2(skill); });
			menu_button.GetComponentInChildren<Text>().text = skill.skill;
        }
        if (menulev == 2) 
        {
            GameObject menu_button = (GameObject)Instantiate(buttonMenuPrefab);
			menu_button.transform.SetParent(panel2.transform, false);
			menu_button.GetComponent<RectTransform>().anchoredPosition = Vector3.up* (buttonCount * -120  );
			menu_button.GetComponentInChildren<Text>().text = skill.skill;
            menu_button.GetComponent<Button>().onClick.AddListener(() => { BuildTechMenu(skill); });
        }
	}
    public float SkillLevelCalc(int _tech, int _difficulty, long _points)
    {
		if (_points == 0) {
			return 0L;
		} else {
			float level = Mathf.Log (_points, _difficulty * 3 + _tech - 1);
			return level;
		}
    }
	public long SkillPointsCalc(int _tech, int _difficulty, int _level)
	{
		long points =Mathf.FloorToInt( Mathf.Pow ( _difficulty * 3 + _tech - 1,_level));
		return points;
	}
	public void CloseMenu(){
		gameObject.GetComponentInParent<ShowMenus> ().skillsOpened = false;
		Destroy (gameObject);
	}
}
