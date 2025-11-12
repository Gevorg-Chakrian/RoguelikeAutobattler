using UnityEngine;

[CreateAssetMenu(fileName = "New Unit", menuName = "Roguelike Autobattler/Unit Data")]
public class UnitData : ScriptableObject
{
    [Header("Basic Info")]
    public string unitName;
    public UnitClass unitClass;
    public Element element;
    public Rarity rarity = Rarity.Common;

    [Header("Stats")]
    public int baseHealth = 100;
    public int baseDamage = 10;
    public float attackSpeed = 1f;
    public float range = 2f;
    public float moveSpeed = 3f;

    [Header("Visuals")]
    public GameObject unitPrefab;
    public Sprite unitIcon;

    [Header("Skills")]
    public string skillDescription;
    public string evolutionDescription;

    [Header("Evolution")]
    public UnitData evolvedForm;

    [Header("Shop")]
    public int shopCost = 3;

    public string GetDisplayName()
    {
        return $"{unitName} ({element} {unitClass})";
    }
}