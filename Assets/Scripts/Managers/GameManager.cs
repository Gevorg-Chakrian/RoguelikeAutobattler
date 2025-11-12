using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game State")]
    public GameState currentGameState = GameState.MainMenu;
    public int currentRunLevel = 1;
    public int souls = 0;
    public int gold = 0;

    [Header("References")]
    public HeroController currentHero;
    public UnitManager unitManager;
    public BattleManager battleManager;
    public ShopManager shopManager;
    public UIManager uiManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeManagers();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeManagers()
    {
        // These will be set in the Inspector
        if (unitManager == null)
            unitManager = FindObjectOfType<UnitManager>();
        if (battleManager == null)
            battleManager = FindObjectOfType<BattleManager>();
        if (shopManager == null)
            shopManager = FindObjectOfType<ShopManager>();
        if (uiManager == null)
            uiManager = FindObjectOfType<UIManager>();
    }

    private void Start()
    {
        ChangeState(GameState.MainMenu);
    }

    public void ChangeState(GameState newState)
    {
        currentGameState = newState;
        OnStateChanged();
    }

    private void OnStateChanged()
    {
        Debug.Log($"GameState changed to: {currentGameState}");

        switch (currentGameState)
        {
            case GameState.MainMenu:
                uiManager.ShowMainMenu();
                break;
            case GameState.Preparation:
                unitManager.SetupPreparationPhase();
                uiManager.ShowPreparationUI();
                break;
            case GameState.Battle:
                battleManager.StartBattle();
                uiManager.ShowBattleUI();
                break;
            case GameState.Shop:
                shopManager.InitializeShop();
                uiManager.ShowShopUI();
                break;
            case GameState.GameOver:
                uiManager.ShowGameOver();
                break;
        }
    }

    public void ContinueToNextBattle()
    {
        ChangeState(GameState.Preparation);
    }

    public void StartNewRun()
    {
        currentRunLevel = 1;
        souls = 0;
        unitManager.ResetRunData();
        ChangeState(GameState.Preparation);
    }

    public void AddSouls(int amount)
    {
        souls += amount;
        uiManager.UpdateSoulsDisplay();
    }

    public bool SpendSouls(int amount)
    {
        if (souls >= amount)
        {
            souls -= amount;
            uiManager.UpdateSoulsDisplay();
            return true;
        }
        return false;
    }
}