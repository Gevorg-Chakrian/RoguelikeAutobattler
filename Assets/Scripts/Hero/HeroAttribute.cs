using UnityEngine;

[CreateAssetMenu(fileName = "New Hero Attribute", menuName = "Roguelike Autobattler/Hero Attribute")]
public class HeroAttribute : ScriptableObject
{
    [Header("Basic Info")]
    public string attributeName;
    public AttributeType type;
    public string description;
    public Sprite icon;

    [Header("Effects")]
    public float healthBonus = 0f;
    public float damageBonus = 0f;
    public float speedBonus = 0f;
    public float cooldownReduction = 0f;

    public virtual void ApplyEffect(HeroController hero)
    {
        // Base implementation
        Debug.Log($"Applying attribute effect: {attributeName}");
    }
}