using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance;

    [Header("Unit Lists")]
    public List<UnitData> allUnits = new List<UnitData>();
    public List<UnitInstance> currentSquad = new List<UnitInstance>();
    public List<UnitData> inventoryUnits = new List<UnitData>();

    [Header("Limits")]
    public int squadLimit = 2;
    public int inventoryLimit = 5;

    private void Awake()
    {
        Instance = this;
    }

    public void Initialize()
    {
        LoadAllUnitData();
    }

    private void LoadAllUnitData()
    {
        // Load all UnitData ScriptableObjects from Resources
        UnitData[] units = Resources.LoadAll<UnitData>("Units");
        allUnits.AddRange(units);
        Debug.Log($"Loaded {allUnits.Count} units");
    }

    public void ResetRunData()
    {
        currentSquad.Clear();
        inventoryUnits.Clear();
        squadLimit = 2; // Reset to base level
    }

    public void SetupPreparationPhase()
    {
        Debug.Log("Setting up preparation phase");
        // This will be called when entering preparation phase
    }

    public bool CanAddToSquad()
    {
        return currentSquad.Count < squadLimit;
    }

    public bool CanAddToInventory()
    {
        return inventoryUnits.Count < inventoryLimit;
    }

    public void AddUnitToSquad(UnitInstance unit)
    {
        if (CanAddToSquad())
        {
            currentSquad.Add(unit);
            Debug.Log($"Added {unit.unitData.unitName} to squad. Squad size: {currentSquad.Count}/{squadLimit}");
        }
        else
        {
            Debug.LogWarning("Cannot add unit to squad: squad limit reached");
        }
    }

    public void AddUnitToInventory(UnitData unitData)
    {
        if (CanAddToInventory())
        {
            inventoryUnits.Add(unitData);
            Debug.Log($"Added {unitData.unitName} to inventory. Inventory size: {inventoryUnits.Count}/{inventoryLimit}");
        }
        else
        {
            Debug.LogWarning("Cannot add unit to inventory: inventory limit reached");
        }
    }

    public void RemoveUnitFromSquad(UnitInstance unit)
    {
        currentSquad.Remove(unit);
    }

    public void RemoveUnitFromInventory(UnitData unitData)
    {
        inventoryUnits.Remove(unitData);
    }

    public void EvolveUnit(UnitData unitData)
    {
        // Find 3 identical units in inventory
        List<UnitData> identicalUnits = inventoryUnits.FindAll(u => u.unitName == unitData.unitName);

        if (identicalUnits.Count >= 3 && unitData.evolvedForm != null)
        {
            // Remove 3 units
            for (int i = 0; i < 3; i++)
            {
                inventoryUnits.Remove(identicalUnits[i]);
            }

            // Add evolved unit
            inventoryUnits.Add(unitData.evolvedForm);
            Debug.Log($"Evolved {unitData.unitName} into {unitData.evolvedForm.unitName}");
        }
        else
        {
            Debug.LogWarning($"Cannot evolve {unitData.unitName}: need 3 copies and evolved form defined");
        }
    }

    public List<UnitData> GetAvailableUnitsByRarity(Rarity rarity)
    {
        return allUnits.FindAll(unit => unit.rarity == rarity);
    }
}