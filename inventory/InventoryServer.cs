using UnityEngine;
using System.Collections.Generic;

class InventoryServer : MonoBehaviour
{

    public List<InventoryItem> PlayerInventory(int player_id, int holder_id)
    {
        return GetComponent<ItemDB>().GetInventory(player_id, holder_id);
    }

    public Item GetItem(int item_id)
    {
        return GetComponent<ItemDB>().GetItem(item_id);
    }
    public ShipItem GetShipItem(int item_id)
    {
        return GetComponent<ItemDB>().GetShipItem(item_id);
    }


    public void AddToInventory(int player_id, int holder_id, int item_id, int tech, int quantity)
    {
        GetComponent<ItemDB>().InventoryAdd(player_id, holder_id, item_id, tech, quantity);
    }

    public bool DeleteFromInventory(int player_id, int holder_id, int item_id, int tech, int quantity)
    {
        return GetComponent<ItemDB>().InventoryDelete(player_id, holder_id, item_id, tech, quantity);
    }

    public void DestroyInventory(int player_id, int holder_id)
    {

    }

    public void MoveItem(int fromPlayer_id, int fromHolder_id, int toPlayer_id, int toHolder_id, int item_id, int tech, int quantity)
    {
        if (DeleteFromInventory(fromPlayer_id, fromHolder_id, item_id, tech, quantity))
        {
            AddToInventory(toPlayer_id, toHolder_id, item_id, tech, quantity);
        }
    }
}
