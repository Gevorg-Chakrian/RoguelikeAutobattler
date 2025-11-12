using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ShopUI : MonoBehaviour
{
    [Header("Shop UI References")]
    public TextMeshProUGUI shopTitleText;
    public Button rerollButton;
    public TextMeshProUGUI rerollCostText;
    public Transform shopUnitsContainer;
    public GameObject unitButtonPrefab;
    public Button continueButton;

    private List<GameObject> unitButtons = new List<GameObject>();
    private ShopManager shopManager;

    void Start()
    {
        // Find ShopManager safely
        shopManager = FindObjectOfType<ShopManager>();
        if (shopManager == null)
        {
            Debug.LogError("ShopManager not found in scene!");
            return;
        }

        // Set up button listeners
        if (rerollButton != null)
        {
            rerollButton.onClick.AddListener(OnRerollClicked);
        }

        if (continueButton != null)
        {
            continueButton.onClick.AddListener(OnContinueClicked);
        }

        UpdateShopUI();
    }

    public void UpdateShopUI()
    {
        if (shopManager == null) return;

        UpdateShopTitle();
        UpdateRerollButton();
        UpdateUnitButtons();
    }

    private void UpdateShopTitle()
    {
        if (shopTitleText != null && GameManager.Instance != null)
        {
            // Simple text without color coding
            shopTitleText.text = $"SHOP - {GameManager.Instance.souls} Souls";
        }
    }

    private void UpdateRerollButton()
    {
        if (rerollButton != null && shopManager != null)
        {
            bool canReroll = shopManager.CanAffordReroll();
            rerollButton.interactable = canReroll;

            if (rerollCostText != null)
            {
                // Simple text without color coding
                rerollCostText.text = $"Reroll ({shopManager.rerollCost})";
            }
        }
    }

    private void UpdateUnitButtons()
    {
        // Clear existing unit buttons
        foreach (GameObject button in unitButtons)
        {
            if (button != null) Destroy(button);
        }
        unitButtons.Clear();

        // Create new unit buttons only if we have a container
        if (shopManager != null && shopUnitsContainer != null)
        {
            foreach (UnitData unit in shopManager.availableUnits)
            {
                GameObject unitButton = CreateUnitButton(unit);
                if (unitButton != null) unitButtons.Add(unitButton);
            }
        }
    }

    private GameObject CreateUnitButton(UnitData unit)
    {
        if (unitButtonPrefab != null)
        {
            GameObject unitButton = Instantiate(unitButtonPrefab, shopUnitsContainer);
            SetupUnitButton(unitButton, unit);
            return unitButton;
        }
        else
        {
            Debug.LogWarning("UnitButtonPrefab is not assigned!");
            return null;
        }
    }

    private void SetupUnitButton(GameObject buttonObj, UnitData unit)
    {
        if (buttonObj == null || unit == null) return;

        bool canAfford = shopManager.CanAffordUnit(unit);

        // Find all TextMeshPro components
        TextMeshProUGUI[] texts = buttonObj.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (TextMeshProUGUI text in texts)
        {
            if (text.name == "UnitNameText" || text.gameObject.name.Contains("Name"))
            {
                text.text = unit.unitName;
            }
            else if (text.name == "UnitCostText" || text.gameObject.name.Contains("Cost"))
            {
                // Simple text without color coding
                text.text = $"{unit.shopCost} souls";
            }
            else if (text.name == "UnitStatsText" || text.gameObject.name.Contains("Stats"))
            {
                text.text = $"{GetClassAbbreviation(unit.unitClass)} • {GetElementAbbreviation(unit.element)}";
            }
        }

        // Set up button
        Button button = buttonObj.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => OnUnitClicked(unit));
            button.interactable = canAfford;
        }

        // Remove any background color - use default button appearance
        Image buttonImage = buttonObj.GetComponent<Image>();
        if (buttonImage != null)
        {
            // Use default white or transparent background
            buttonImage.color = Color.red;
        }
    }

    private string GetClassAbbreviation(UnitClass unitClass)
    {
        switch (unitClass)
        {
            case UnitClass.Warrior: return "WAR";
            case UnitClass.Mage: return "MAG";
            case UnitClass.Archer: return "ARC";
            default: return "???";
        }
    }

    private string GetElementAbbreviation(Element element)
    {
        switch (element)
        {
            case Element.Earth: return "ERTH";
            case Element.Fire: return "FIRE";
            case Element.Water: return "WATR";
            case Element.Lightning: return "LGTN";
            case Element.Wind: return "WIND";
            default: return "????";
        }
    }

    private void OnUnitClicked(UnitData unit)
    {
        if (shopManager != null && shopManager.CanAffordUnit(unit))
        {
            shopManager.PurchaseUnit(unit);
            UpdateShopUI();
        }
    }

    private void OnRerollClicked()
    {
        if (shopManager != null && shopManager.CanAffordReroll())
        {
            shopManager.RerollShop();
            UpdateShopUI();
        }
    }

    private void OnContinueClicked()
    {
        // Transition to next battle phase
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ChangeState(GameState.Preparation);
        }
    }
}