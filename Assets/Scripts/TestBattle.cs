using UnityEngine;

public class TestBattle : MonoBehaviour
{
    void Update()
    {
        // Press B to start battle for testing
        if (Input.GetKeyDown(KeyCode.B) && GameManager.Instance.currentGameState == GameState.Preparation)
        {
            GameManager.Instance.ChangeState(GameState.Battle);
        }

        // Press N to go to shop for testing
        if (Input.GetKeyDown(KeyCode.N) && GameManager.Instance.currentGameState == GameState.Battle)
        {
            // Simulate winning battle
            BattleManager.Instance.EndBattle(true);
        }
    }
}