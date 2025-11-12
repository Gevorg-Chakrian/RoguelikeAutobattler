using UnityEngine;
using System.Collections.Generic;

public class HeroController : MonoBehaviour
{
    [Header("Hero Stats")]
    public string heroName = "Captain Aldric";
    public int level = 0;
    public int currentXP = 0;
    public int xpToNextLevel = 100;

    [Header("Attributes")]
    public int strength = 10;
    public int dexterity = 10;
    public int intelligence = 10;

    [Header("Abilities")]
    public List<HeroAttribute> chosenAttributes = new List<HeroAttribute>();

    [Header("Combat")]
    public int currentHealth = 100;
    public int maxHealth = 100;
    public int damage = 20;

    public void GainXP(int amount)
    {
        currentXP += amount;
        Debug.Log($"Hero gained {amount} XP. Total: {currentXP}/{xpToNextLevel}");

        if (currentXP >= xpToNextLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        level++;
        currentXP -= xpToNextLevel;
        xpToNextLevel = CalculateXPForNextLevel();

        // Increase squad limit based on level
        if (UnitManager.Instance != null)
        {
            UnitManager.Instance.squadLimit = GetSquadLimitForLevel();
        }

        Debug.Log($"Hero leveled up to level {level}!");

        // Show attribute selection UI
        if (UIManager.Instance != null)
        {
            // UIManager.Instance.ShowAttributeSelection();
        }
    }

    private int CalculateXPForNextLevel()
    {
        return 100 + (level * 50);
    }

    private int GetSquadLimitForLevel()
    {
        switch (level)
        {
            case 0: return 2;
            case 1: return 3;
            case 2: return 4;
            case 3: return 5;
            case 4: return 6;
            case 5: return 7;
            default: return 7;
        }
    }

    public void ChooseAttribute(HeroAttribute attribute)
    {
        chosenAttributes.Add(attribute);
        ApplyAttributeEffects(attribute);
        Debug.Log($"Chose attribute: {attribute.attributeName}");
    }

    private void ApplyAttributeEffects(HeroAttribute attribute)
    {
        // Apply the attribute effects to the hero
        switch (attribute.type)
        {
            case AttributeType.Strength:
                strength++;
                maxHealth += 20;
                currentHealth = maxHealth;
                break;
            case AttributeType.Dexterity:
                dexterity++;
                break;
            case AttributeType.Intelligence:
                intelligence++;
                break;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("Hero died!");
        }
    }
}