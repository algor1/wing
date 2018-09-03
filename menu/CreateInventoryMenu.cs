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
    private GameObject panelBottom;
	[SerializeField]
	private GameObject panelPopup;
    [SerializeField]
    private GameObject buttonMenuPrefab;
    [SerializeField]
    private GameObject PopupMoveButton;
    public int leftHolder_id;
    public int rightHolder_id;
    private int player_id;



	// Use this for initialization
	void Start () {
		server = GameObject.Find ("ServerGo");
		Debug.Log (server.name);
        leftHolder_id =200;
        rightHolder_id=0;
        player_id=0;
//		playerSkills =server.GetComponent<PlayerSkillsServer> ().AllPlayerSkills (0);
        //skillsDB = server.GetComponent<SkillsDB>().GetAllSkills();
		BuildMenu(panelLeft,0,leftHolder_id);
		BuildMenu (panelRight, 0, rightHolder_id);
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

    
	private void InfoPopup(Item item,InventoryItem invItem,bool directionToMove)
	{
        int fromHolder;
        int toHolder;
        string arrow;
        if (directionToMove)
        {
            fromHolder = leftHolder_id;
            toHolder = rightHolder_id;
        }
        else
        {
            fromHolder = rightHolder_id;
            toHolder = leftHolder_id;
        }
        PopupMoveButton.GetComponentInChildren<Text>().text = "Move to : " + toHolder.ToString();
        PopupMoveButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                server.GetComponent<InventoryServer>().MoveItem(player_id,fromHolder,player_id,toHolder,invItem.item_id,invItem.tech,invItem.quantity);
                panelPopup.SetActive(false);
            });
        panelPopup.SetActive (true);
	}

	private void MenuAdd(Item item, InventoryItem invItem,GameObject panel,int buttonCount)
	{
		
        GameObject menu_button = (GameObject)Instantiate(buttonMenuPrefab);
		menu_button.transform.SetParent (panel.transform, false);
		menu_button.GetComponent<RectTransform>().localPosition = Vector3.up * (buttonCount * -50);
        bool directionToMove = (panel == panelRight);
		menu_button.GetComponent<Button>().onClick.AddListener(() => { InfoPopup(item,invItem,directionToMove); });
		menu_button.GetComponentInChildren<Text>().text = "id : "+ invItem.item_id.ToString()+"  quantity: "+invItem.quantity.ToString();
	}


    
}
