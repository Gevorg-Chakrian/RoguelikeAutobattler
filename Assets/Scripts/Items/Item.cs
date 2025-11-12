using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Roguelike Autobattler/Item")]
public class Item : ScriptableObject
{
    [Header("Basic Info")]
    public string itemName;
    public string description;
    public Sprite icon;
    public int cost = 1;

    [Header("Effects")]
    public int healthBonus = 0;
    public int damageBonus = 0;
    public float speedBonus = 0f;
    public float attackSpeedBonus = 0f;

    [Header("Item Type")]
    public ItemType itemType = ItemType.Consumable;

    public enum ItemType
    {
        Consumable,
        Equipment,
        Scroll,
        Modifier
    }

    public virtual void UseItem()
    {
        Debug.Log($"Using item: {itemName}");
        // Base implementation - override in specific item types
    }
}