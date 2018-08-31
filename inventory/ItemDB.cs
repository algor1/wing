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
        // ��������� ������
        reader = dbcmd.ExecuteReader();
        //		Debug.Log (reader.Read());
    }


    public Item GetItem(int item_id)
    {
        Item returnItem = new Item();
        string qwery = "SELECT id,item,item_type.type FROM items inner join item_type  where items.id=" + item_id.ToString()+" and items.type_id=item_type.id";
        
        GetReader(qwery);
        while (reader.Read())
        {
            returnItem.id = reader.GetInt32(0);
            returnItem.item = reader.GetString(1);
			returnItem.itemType = (Item.Type_of_item) System.Enum.Parse(typeof( Item.Type_of_item),reader.GetString(2));
        }
        return returnItem;
    }
    public ShipItem GetShipItem(int item_id)
    {
        ShipItem returnShipItem = new ShipItem();
        string qwery = "SELECT item_id,max_speed,rotation_speed, acceleration_max,hull_full,armor_full,shield_full,capasitor_full,hull_restore,armor_restore,shield_restore,capasitor_restore,agr_distance,vision_distance,mob,warpDriveStartTime,warpSpeed  FROM ship_item  where item_id=" + item_id.ToString();

        GetReader(qwery);
        while (reader.Read())
        {
       if (!reader.IsDBNull(0))returnShipItem.item_id = reader.GetInt32(0);
			if (!reader.IsDBNull(1))returnShipItem.max_speed = reader.GetFloat(1);
       if (!reader.IsDBNull(2))returnShipItem.rotation_speed = reader.GetFloat(1);
       if (!reader.IsDBNull(3))returnShipItem.acceleration_max = reader.GetFloat(1);
       if (!reader.IsDBNull(4))returnShipItem.hull_full = reader.GetFloat(1);
       if (!reader.IsDBNull(5))returnShipItem.armor_full = reader.GetFloat(1);
       if (!reader.IsDBNull(6))returnShipItem.shield_full = reader.GetFloat(1);
       if (!reader.IsDBNull(7))returnShipItem.capasitor_full = reader.GetFloat(1);
       if (!reader.IsDBNull(8))returnShipItem.hull_restore = reader.GetFloat(1);
       if (!reader.IsDBNull(9))returnShipItem.armor_restore = reader.GetFloat(1);
       if (!reader.IsDBNull(10))returnShipItem.shield_restore = reader.GetFloat(1);
       if (!reader.IsDBNull(11))returnShipItem.capasitor_restore = reader.GetFloat(1);
       if (!reader.IsDBNull(12))returnShipItem.agr_distance = reader.GetFloat(1);
       if (!reader.IsDBNull(13))returnShipItem.vision_distance = reader.GetFloat(1);
		if (!reader.IsDBNull(14))returnShipItem.mob = (reader.GetInt32(1)==1);
       if (!reader.IsDBNull(15))returnShipItem.warpDriveStartTime = reader.GetFloat(1);
       if (!reader.IsDBNull(16)) returnShipItem.warpSpeed = reader.GetFloat(1);
        }
        return returnShipItem;
    }
    public List<InventoryItem> GetInventory(int player_id, int holder_id)
    {
        List<InventoryItem> returnInventoryItemList = new List<InventoryItem>();
        string qwery = "SELECT item_id,tech,quantity FROM inventory where player_id=" + player_id.ToString() + " and inventory_holder_id = " + holder_id.ToString();
        GetReader(qwery);
        
        while (reader.Read())
        {
            InventoryItem _inventoryItem = new InventoryItem();
            if (!reader.IsDBNull(0)) _inventoryItem.item_id = reader.GetInt32(0);
            if (!reader.IsDBNull(1)) _inventoryItem.tech = reader.GetInt32(1);
            if (!reader.IsDBNull(2)) _inventoryItem.quantity = reader.GetInt32(2);
            returnInventoryItemList.Add(_inventoryItem);
        }
        return returnInventoryItemList;
    }
    

    public void InventoryAdd(int player_id, int holder_id, int item_id,int tech, int quantity)
    {
        string qwery=qwery = "insert into inventory (player_id,inventory_holder_id,item_id,tech,quantity) values (" + player_id.ToString () + "," + holder_id.ToString() +"," +item_id.ToString () + "," + tech.ToString () + ","+quantity.ToString()+ ")";
			
        List<InventoryItem> inventoryItemList= GetInventory(player_id,holder_id);
        
        for (int i=0 ; i<inventoryItemList.Count;i++){
            if ( inventoryItemList[i].item_id==item_id && inventoryItemList[i].tech==tech){
                qwery = "update inventory set quantity = quantity +" + quantity.ToString() + " where player_id=" + player_id.ToString () + " and item_id=" + item_id.ToString () + " and inventory_holder_id=" + holder_id.ToString () + "  and tech=" + tech.ToString () + " )";
		    
                break;
            }
        }
        GetReader (qwery);
    }

    public void InventoryDelete(int player_id, int holder_id, int item_id, int tech, int quantity)
    {
        //string qwery = qwery = "delete from inventory where (player_id=" + player_id.ToString() + " and item_id=" + item_id.ToString() + " and inventory_holder_id=" + holder_id.ToString() + "  and tech=" + tech.ToString() + " )";
        //List<InventoryItem> inventoryItemList = GetInventory(player_id, holder_id);

        //for (int i = 0; i < inventoryItemList.Count; i++)
        //{
        //    if (inventoryItemList[i].item_id == item_id && inventoryItemList[i].tech == tech)
        //    {
        //        qwery = "update inventory set quantity = quantity +" + quantity.ToString() + " where player_id=" + player_id.ToString() + " and item_id=" + item_id.ToString() + " and inventory_holder_id=" + holder_id.ToString() + "  and tech=" + tech.ToString() + " )";

        //        break;
        //    }
        //}
        //GetReader(qwery);
    }
}   