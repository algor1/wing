using UnityEngine;
 
//[System.Serializable]
public class Item 
{
    public int itemID;
    public string itemName;
    public Sprite itemSprite;
    public enum Type_of_item { ship, weapon,equipment,material, other };
    public Type_of_item itemType;
    public float volume;


}