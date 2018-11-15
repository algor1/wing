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


//	void Awake()
//	{
//		DontDestroyOnLoad(transform.gameObject);
//		gameObject.SetActive (false);
//	}

	// Use this for initialization
	void Start () {
		server = GameObject.Find ("ServerGo");
		Debug.Log (server.name);
//        leftHolder_id =200;
//        rightHolder_id=0;
        player_id=0;
//		playerSkills =server.GetComponent<PlayerSkillsServer> ().AllPlayerSkills (0);
        //skillsDB = server.GetComponent<SkillsDB>().GetAllSkills();
		BuildMenu(panelLeft,player_id,leftHolder_id);
		BuildMenu (panelRight,player_id, rightHolder_id);
	}

	private void BuildMenu (GameObject panel,int player_id,int holder_id)
    {
        foreach (Transform child in panel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
		List<InventoryItem> panelInv = server.GetComponent<InventoryServer> ().PlayerInventory (player_id, holder_id);
		Debug.Log (holder_id + " hold " + panelInv.Count);
        int j = 0;
		for (int i = 0; i < panelInv.Count; i++)
        {
 
			Item item = server.GetComponent<InventoryServer> ().GetItem (panelInv [i].item_id);
			MenuAdd(item, panelInv[i], panel, j);
			j++;

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
		Debug.Log (invItem.item_id+" item  "+ menu_button.GetComponent<RectTransform> ().localPosition);
		menu_button.GetComponent<RectTransform>().anchoredPosition += Vector2.up*(buttonCount * -120);
		Debug.Log (invItem.item_id+" item  "+ menu_button.GetComponent<RectTransform> ().localPosition);
        bool directionToMove = (panel == panelRight);
		menu_button.GetComponent<Button>().onClick.AddListener(() => { InfoPopup(item,invItem,directionToMove); });
		menu_button.transform.Find ("TextName").GetComponent<Text> ().text = item.item.ToString ();
		menu_button.transform.Find ("TextProp").GetComponent<Text> ().text = "Tech : " + invItem.tech.ToString ();
		menu_button.transform.Find("TextQuant").GetComponent<Text>().text = invItem.quantity.ToString();


	}
	public void CloseMenu(){
		gameObject.GetComponentInParent<ShowMenus> ().inventoryOpened = false;
		Destroy (gameObject);
	}
	public void moveAll(int direction){
		int fromHolder_id = rightHolder_id;
		int toHolder_id = leftHolder_id;
		if (direction == 0) {
			fromHolder_id = leftHolder_id;
			toHolder_id = rightHolder_id;
		}
		List<InventoryItem> panelInv = server.GetComponent<InventoryServer> ().PlayerInventory (player_id, fromHolder_id);

		for (int i = 0; i < panelInv.Count; i++)
		{
			server.GetComponent<InventoryServer> ().MoveItem (player_id, fromHolder_id, player_id, toHolder_id, panelInv [i].item_id, panelInv [i].tech, panelInv [i].quantity);
		}
		BuildMenu(panelLeft,player_id,leftHolder_id);
		BuildMenu (panelRight,player_id, rightHolder_id);

		if (server.GetComponent<InventoryServer> ().PlayerInventory (player_id, fromHolder_id).Count==0) {
			CloseMenu ();
			server.GetComponent<ServerSO> ().DestroyContainer (fromHolder_id);
		}
	}
    
}
