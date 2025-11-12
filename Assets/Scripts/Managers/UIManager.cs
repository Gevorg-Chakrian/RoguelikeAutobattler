using UnityEngine;
using UnityEngine.UI;
using TMPro; // Add this for TextMeshPro

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject preparationPanel;
    public GameObject battlePanel;
    public GameObject shopPanel;
    public GameObject gameOverPanel;

    [Header("Battle UI - TextMeshPro")]
    public TextMeshProUGUI timerText; // Changed from Text to TextMeshProUGUI
    public TextMeshProUGUI soulsText; // Changed from Text to TextMeshProUGUI

    [Header("Shop UI")]
    public Transform shopUnitsContainer;
    public GameObject unitShopPrefab;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        HideAllPanels();
        mainMenuPanel.SetActive(true);
    }

    public void ShowPreparationUI()
    {
        HideAllPanels();
        preparationPanel.SetActive(true);
        UpdateSoulsDisplay();
    }

    public void ShowBattleUI()
    {
        HideAllPanels();
        battlePanel.SetActive(true);
        UpdateBattleUI();
    }

    public void ShowShopUI()
    {
        HideAllPanels();
        shopPanel.SetActive(true);
        UpdateShopUI();
        UpdateSoulsDisplay();
    }

    public void ShowGameOver()
    {
        HideAllPanels();
        gameOverPanel.SetActive(true);
    }

    private void HideAllPanels()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (preparationPanel != null) preparationPanel.SetActive(false);
        if (battlePanel != null) battlePanel.SetActive(false);
        if (shopPanel != null) shopPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    public void UpdateBattleUI()
    {
        if (battlePanel != null && battlePanel.activeInHierarchy && BattleManager.Instance != null)
        {
            if (timerText != null)
                timerText.text = $"Time: {Mathf.CeilToInt(BattleManager.Instance.currentBattleTime)}";
        }
    }

    public void UpdateSoulsDisplay()
    {
        if (soulsText != null && GameManager.Instance != null)
        {
            soulsText.text = $"Souls: {GameManager.Instance.souls}";
        }
    }

    public void UpdateShopUI()
    {
        // This would populate the shop with available units
        // For now, just a placeholder
    }

    // Button handlers
    public void OnStartGameClicked()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.StartNewRun();
    }

    public void OnShopRerollClicked()
    {
        if (ShopManager.Instance != null)
            ShopManager.Instance.RerollShop();
    }

    private void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.currentGameState == GameState.Battle)
        {
            UpdateBattleUI();
        }
    }
}