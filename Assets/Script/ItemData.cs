using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public bool stackable;
    public int maxStack;
    public Sprite icon;
}