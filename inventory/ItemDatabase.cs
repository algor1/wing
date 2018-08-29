using UnityEngine;
[CreateAssetMenu]
public class ItemDatabase : ScriptableObject
{
    public Item[] items;
 
    public Item GetItem(int itemID)
    {
        foreach (var item in items)
        {
            if (item != null && item.itemID == itemID) return item;
        }
        return null;
    }
}