using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateInventoryMenu : MonoBehaviour {

//	[SerializeField]
	private GameObject server;
	[SerializeField]
	private GameObject panelLeft;
    [SerializeField]
    private GameObject panelRight;
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
		BuildMenu(panelLeft,0,200);
		BuildMenu (panelRight, 0, 0);
	}

	private void BuildMenu (GameObject panel,int player_id,int holder_id)
    {
        foreach (Transform child in panel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
		List<InventoryItem> panelInv = server.GetComponent<InventoryServer> ().PlayerInventory (0, 200);

        int j = 0;
		for (int i = 0; i < panelInv.Count; i++)
        {
 
            j++;
			Item item = server.GetComponent<InventoryServer> ().GetItem (panelInv [i].item_id);
			MenuAdd(item, panelInv[i], panel, j);
        }
    }

    
	private void InfoPopup(Item item,InventoryItem invItem)
	{
//
//        if (server.GetComponent<PlayerSkillsServer>().PlayerSkillQueue(0).Count < 3)
//        {
//            PopupLevelButton.GetComponent<Button>().onClick.AddListener(() => { server.GetComponent<PlayerSkillsServer>().AddSkillToQueue(0, skill.id, tech, level + 1); });
//        }
//        else
//        {
//            // PopupLevelButton.GetComponent<Button>().onClick.AddListener(() =>   warinig message "cant add to queue" 
//        }
//
//        if (SkillLevelCalc(tech, skill.difficulty, server.GetComponent<PlayerSkillsServer>().SkillLearned(0, skill.id, tech)) >= 5)
//        {
//
//            PopupTechButton.GetComponent<Button>().onClick.AddListener(() =>
//            {
//                server.GetComponent<PlayerSkillsServer>().AddSkillToQueue(0, skill.id, tech + 1, 1);
//            });
//            PopupTechButton.SetActive(true);
//        }
//        else
//        {
//            PopupTechButton.SetActive(false);
//            // PopupLevelButton.GetComponent<Button>().onClick.AddListener(() =>   warinig message "cant add to queue" 
//        }
//
//		panelTechInfoPopup.SetActive (true);

	}

	private void MenuAdd(Item item, InventoryItem invItem,GameObject panel,int buttonCount)
	{
		
        GameObject menu_button = (GameObject)Instantiate(buttonMenuPrefab);
		menu_button.transform.SetParent (panel.transform, false);
		menu_button.GetComponent<RectTransform>().localPosition = Vector3.up * (buttonCount * -50);
		menu_button.GetComponent<Button>().onClick.AddListener(() => { InfoPopup(item,invItem); });
		menu_button.GetComponentInChildren<Text>().text = "id : "+ invItem.item_id.ToString()+"  quantity: "+invItem.quantity.ToString();
	}


    
}
