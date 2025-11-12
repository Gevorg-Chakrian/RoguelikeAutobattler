using UnityEngine;

[CreateAssetMenu(fileName = "New Unit Modifier", menuName = "Roguelike Autobattler/Unit Modifier")]
public class UnitModifier : ScriptableObject
{
    [Header("Basic Info")]
    public string modifierName;
    public string description;

    [Header("Stat Modifiers")]
    public float healthMultiplier = 1f;
    public float damageMultiplier = 1f;
    public float speedMultiplier = 1f;
    public float attackSpeedMultiplier = 1f;

    [Header("Special Effects")]
    public bool grantsBonusSouls = false;
    public int soulsPerKill = 0;

    public virtual void ApplyModifier(UnitInstance unit)
    {
        Debug.Log($"Applying modifier {modifierName} to {unit.unitData.unitName}");
        // Base implementation - can be overridden for special modifiers
    }
}