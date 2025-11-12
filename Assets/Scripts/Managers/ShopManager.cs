using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    [Header("Shop Settings")]
    public int unitPrice = 3;
    public int rerollCost = 1;

    [Header("Current Offerings")]
    public List<UnitData> availableUnits = new List<UnitData>();
    public List<Item> availableItems = new List<Item>();

    private void Awake()
    {
        Instance = this;
    }

    public void InitializeShop()
    {
        Debug.Log("Initializing shop");
        RefreshShopOfferings();

        // Add some starting souls for testing
        GameManager.Instance.AddSouls(10);
    }

    public void RefreshShopOfferings()
    {
        availableUnits.Clear();

        // Get available units from UnitManager
        if (UnitManager.Instance != null && UnitManager.Instance.allUnits.Count > 0)
        {
            // Generate 5 random units for the shop
            for (int i = 0; i < 5; i++)
            {
                UnitData randomUnit = GetRandomUnitForShop();
                if (randomUnit != null)
                {
                    availableUnits.Add(randomUnit);
                }
            }
        }
        else
        {
            Debug.LogWarning("No units available in UnitManager, creating test units");
            // Create test units if none available
            for (int i = 0; i < 5; i++)
            {
                UnitData testUnit = CreateTestUnit(i);
                availableUnits.Add(testUnit);
            }
        }

        Debug.Log($"Shop refreshed with {availableUnits.Count} units");

        // Update shop UI
        if (UIManager.Instance != null)
        {
            // UIManager.Instance.UpdateShopDisplay(availableUnits);
        }
    }

    private UnitData GetRandomUnitForShop()
    {
        if (UnitManager.Instance.allUnits.Count == 0)
            return CreateTestUnit(0);

        int randomIndex = Random.Range(0, UnitManager.Instance.allUnits.Count);
        return UnitManager.Instance.allUnits[randomIndex];
    }

    private UnitData CreateTestUnit(int index)
    {
        UnitData testUnit = ScriptableObject.CreateInstance<UnitData>();
        testUnit.unitName = $"Test Unit {index + 1}";
        testUnit.unitClass = (UnitClass)Random.Range(0, 3);
        testUnit.element = (Element)Random.Range(0, 5);
        testUnit.baseHealth = Random.Range(80, 120);
        testUnit.baseDamage = Random.Range(8, 15);
        testUnit.attackSpeed = Random.Range(0.7f, 1.2f);
        testUnit.range = Random.Range(1.5f, 3f);
        testUnit.moveSpeed = Random.Range(2f, 4f);
        testUnit.shopCost = unitPrice;
        return testUnit;
    }

    public void PurchaseUnit(UnitData unit)
    {
        if (GameManager.Instance.SpendSouls(unitPrice) &&
            UnitManager.Instance.CanAddToInventory())
        {
            UnitManager.Instance.AddUnitToInventory(unit);
            availableUnits.Remove(unit);
            Debug.Log($"Purchased {unit.unitName} for {unitPrice} souls");

            // Update shop UI
            if (UIManager.Instance != null)
            {
                // UIManager.Instance.UpdateShopDisplay(availableUnits);
                UIManager.Instance.UpdateSoulsDisplay();
            }
        }
        else
        {
            Debug.LogWarning("Cannot purchase unit: not enough souls or inventory full");
        }
    }

    public void RerollShop()
    {
        if (GameManager.Instance.SpendSouls(rerollCost))
        {
            RefreshShopOfferings();
            Debug.Log("Shop rerolled");
        }
        else
        {
            Debug.LogWarning("Not enough souls to reroll shop");
        }
    }

    public bool CanAffordUnit(UnitData unit)
    {
        return GameManager.Instance.souls >= unit.shopCost;
    }

    public bool CanAffordReroll()
    {
        return GameManager.Instance.souls >= rerollCost;
    }
}