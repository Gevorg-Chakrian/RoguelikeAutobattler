using UnityEngine;

public class TestStart : MonoBehaviour
{
    void Start()
    {
        // Wait a frame to ensure UIManager.Start() runs first
        Invoke("StartGameDelayed", 0.1f);
    }

    void StartGameDelayed()
    {
        // Auto-start the game for testing
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartNewRun();

            // Add test unit
            UnitData stoneGuardian = Resources.Load<UnitData>("Units/SO_StoneGuardian");
            if (stoneGuardian != null)
            {
                UnitInstance testUnit = new UnitInstance(stoneGuardian);
                UnitManager.Instance.AddUnitToSquad(testUnit);
                Debug.Log("Added test unit to squad");
            }
            else
            {
                Debug.LogError("Could not load Stone Guardian unit!");
            }
        }
        else
        {
            Debug.LogError("GameManager instance is null!");
        }
    }
}