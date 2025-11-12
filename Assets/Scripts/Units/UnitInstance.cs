using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitInstance
{
    public UnitData unitData;
    public int currentHealth;
    public int level = 1;
    public List<UnitModifier> modifiers = new List<UnitModifier>();
    public bool isEvolved = false;

    // Battle state (not serialized)
    [System.NonSerialized] public Vector3 battlePosition;
    [System.NonSerialized] public bool isAlive = true;

    public UnitInstance(UnitData data)
    {
        unitData = data;
        currentHealth = GetMaxHealth();
    }

    public int GetMaxHealth()
    {
        int health = unitData.baseHealth;
        foreach (var modifier in modifiers)
        {
            health = Mathf.RoundToInt(health * modifier.healthMultiplier);
        }
        return health;
    }

    public int GetDamage()
    {
        int damage = unitData.baseDamage;
        foreach (var modifier in modifiers)
        {
            damage = Mathf.RoundToInt(damage * modifier.damageMultiplier);
        }
        return damage;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isAlive = false;
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > GetMaxHealth())
            currentHealth = GetMaxHealth();
    }

    public void AddModifier(UnitModifier modifier)
    {
        modifiers.Add(modifier);
        modifier.ApplyModifier(this);
    }
}