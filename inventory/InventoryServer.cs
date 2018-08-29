using UnityEngine;
using System.Collections.Generic;

class InventoryServer : MonoBehaviour
{
//    public Item GetItem(int item_id)
//    {
//        Item retItem;
//        
//
//        return retItem;
//
//    }
//    public List<InventoryItem> GetInventory (int player_id,int holder_id)
//    {
//
//    }
    public void AddToInventory(int player_id, int holder_id, int item_id, int quantity)
    {

    }
    public void DeleteFromInventory(int player_id, int holder_id, int item_id, int quantity)
    {

    }
    public void DestroyInventory(int player_id, int holder_id)
    {

    }



















}
    //    private Dictionary<int,Dictionary<int,Dictionary<int,InventoryItem>>> playerSOInventoryBase; //playerStationItems[ player_id,[Inventoy_holder_id,list_of_items_in inventory]]
        
    //    void Awake(){
       
    //        playerSOInventoryBase = new Dictionary<int, Dictionary<int, Dictionary<int, InventoryItem>>>();
    //    }

    //    private void LoadData(){
    //    }

    //    private void SaveData(int player_id,int inventory_id){

    //    }

    //    public Dictionary<int, InventoryItem> GetInventory(int player_id, int SO_id)
    //    {
    //         return playerSOInventoryBase[player_id][SO_id];
    //    }

    //    public void PlayerTakeOff(int player_id,int station_id){
    //        SaveData(player_id, station_id);
    //        //send info about ship to ServerGO
    //    }
    //    public void AddItemToInventory(int player_id,int station_id, int item_id,int quantity ){
    //        if (playerSOInventoryBase.ContainsKey(player_id))
    //        {
    //            if (playerSOInventoryBase[player_id].ContainsKey(station_id))
    //            {
    //                if (playerSOInventoryBase[player_id][station_id].ContainsKey(item_id))
    //                {
    //                    playerSOInventoryBase[player_id][station_id][item_id].quantity += quantity;
    //                }
    //                else
    //                {
    //                    InventoryItem newItem = new InventoryItem() { itemID = item_id, quantity = quantity };
    //                    playerSOInventoryBase[player_id][station_id].Add(item_id, newItem);
    //                }
    //            }
    //        }
    //    }

    //    public bool DeleteItemFromInventory(int player_id,int station_id, int item_id,int quantity){
    //        if (playerSOInventoryBase.ContainsKey(player_id))
    //        {
    //            if (playerSOInventoryBase[player_id].ContainsKey(station_id))
    //            {
    //                if (playerSOInventoryBase[player_id][station_id].ContainsKey(item_id))
    //                {
    //                    if (playerSOInventoryBase[player_id][station_id][item_id].quantity > quantity)
    //                   {
    //                       playerSOInventoryBase[player_id][station_id][item_id].quantity += -quantity;
    //                       return true;
    //                   }
    //                    else if (playerSOInventoryBase[player_id][station_id][item_id].quantity == quantity)
    //                   {
    //                       playerSOInventoryBase[player_id][station_id].Remove(item_id);
    //                        return true;
    //                   }
                       
                        
    //                }
                    
    //            }
    //        }
    //        return false;
    //    }
        
        

    //}
