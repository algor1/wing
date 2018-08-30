using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateInventoryMenu : MonoBehaviour {

//	[SerializeField]
	private GameObject server;
	[SerializeField]
	private GameObject panel1;
    [SerializeField]
    private GameObject panel2;
	[SerializeField]
	private GameObject panel3;
    [SerializeField]
    private GameObject buttonMenuPrefab;




	// Use this for initialization
	void Start () {
		server = GameObject.Find ("ServerGo");
		Debug.Log (server.name);
//		playerSkills =server.GetComponent<PlayerSkillsServer> ().AllPlayerSkills (0);
        //skillsDB = server.GetComponent<SkillsDB>().GetAllSkills();
        BuildMenu1();
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
				j++;	
                MenuAdd(skillsDB[i],1,j);
            }
        }
    }
    
	private void InfoPopup(Skill skill, int tech,long points,int level)
	{

        if (server.GetComponent<PlayerSkillsServer>().PlayerSkillQueue(0).Count < 3)
        {
            PopupLevelButton.GetComponent<Button>().onClick.AddListener(() => { server.GetComponent<PlayerSkillsServer>().AddSkillToQueue(0, skill.id, tech, level + 1); });
        }
        else
        {
            // PopupLevelButton.GetComponent<Button>().onClick.AddListener(() =>   warinig message "cant add to queue" 
        }

        if (SkillLevelCalc(tech, skill.difficulty, server.GetComponent<PlayerSkillsServer>().SkillLearned(0, skill.id, tech)) >= 5)
        {

            PopupTechButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                server.GetComponent<PlayerSkillsServer>().AddSkillToQueue(0, skill.id, tech + 1, 1);
            });
            PopupTechButton.SetActive(true);
        }
        else
        {
            PopupTechButton.SetActive(false);
            // PopupLevelButton.GetComponent<Button>().onClick.AddListener(() =>   warinig message "cant add to queue" 
        }

		panelTechInfoPopup.SetActive (true);

	}

	private void MenuAdd(Skill skill,int menulev,int buttonCount)
	{
        if (menulev == 1)
        {
            GameObject menu_button = (GameObject)Instantiate(buttonMenuPrefab);
			menu_button.transform.SetParent (panel1.transform, false);
			menu_button.GetComponent<RectTransform>().localPosition = Vector3.up * (buttonCount * -50);
            menu_button.GetComponent<Button>().onClick.AddListener(() => { BuildMenu2(skill); });
			menu_button.GetComponentInChildren<Text>().text = skill.skill;
        }
        if (menulev == 2) 
        {
            GameObject menu_button = (GameObject)Instantiate(buttonMenuPrefab);
			menu_button.transform.SetParent(panel2.transform, false);
            menu_button.GetComponent<RectTransform>().localPosition = Vector3.up* (buttonCount * -50  );
			menu_button.GetComponentInChildren<Text>().text = skill.skill;
            menu_button.GetComponent<Button>().onClick.AddListener(() => { BuildTechMenu(skill); });


        }
	}


    
}
