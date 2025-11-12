using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
	public static BattleManager Instance;

	[Header("Battle Settings")]
	public float battleTimeLimit = 120f;
	public float currentBattleTime;
	public bool isBattleActive = false;

	[Header("Battle Field")]
	public Transform playerSpawnArea;
	public Transform enemySpawnArea;

	[Header("Units in Battle")]
	public List<UnitController> playerUnits = new List<UnitController>();
	public List<UnitController> enemyUnits = new List<UnitController>();

	private Coroutine battleCoroutine;

	private void Awake()
	{
		Instance = this;
	}

	public void StartBattle()
	{
		if (isBattleActive) return;

		Debug.Log("Starting battle!");
		isBattleActive = true;
		currentBattleTime = battleTimeLimit;

		// Spawn player units
		SpawnPlayerUnits();

		// Spawn enemy units
		SpawnTestEnemies();

		// Start battle coroutine
		if (battleCoroutine != null)
			StopCoroutine(battleCoroutine);
		battleCoroutine = StartCoroutine(BattleLoop());
	}

	private void SpawnPlayerUnits()
	{
		Debug.Log($"Attempting to spawn {UnitManager.Instance.currentSquad.Count} player units");

		foreach (UnitInstance unitInstance in UnitManager.Instance.currentSquad)
		{
			if (unitInstance.unitData.unitPrefab != null)
			{
				Vector3 spawnPos = GetRandomSpawnPosition(playerSpawnArea);
				GameObject unitObj = Instantiate(unitInstance.unitData.unitPrefab, spawnPos, Quaternion.identity);
				UnitController unitController = unitObj.GetComponent<UnitController>();
				if (unitController != null)
				{
					unitController.Initialize(unitInstance, true);
					playerUnits.Add(unitController);
					Debug.Log($"Spawned player unit: {unitInstance.unitData.unitName} at {spawnPos}");
				}
				else
				{
					Debug.LogError("UnitController component missing from unit prefab!");
				}
			}
			else
			{
				Debug.LogError($"Unit prefab is null for {unitInstance.unitData.unitName}");
			}
		}

		Debug.Log($"Spawned {playerUnits.Count} player units");
	}

	private void SpawnTestEnemies()
	{
		// For now, spawn 3 basic enemies
		for (int i = 0; i < 3; i++)
		{
			GameObject testEnemy = GameObject.CreatePrimitive(PrimitiveType.Cube);
			Vector3 spawnPos = GetRandomSpawnPosition(enemySpawnArea);
			testEnemy.transform.position = spawnPos;
			testEnemy.name = $"Test Enemy {i + 1}";

			// Make enemy red and smaller
			testEnemy.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
			testEnemy.GetComponent<Renderer>().material.color = Color.red;

			// Add a simple enemy controller
			UnitController enemyController = testEnemy.AddComponent<UnitController>();

			// Create basic unit data for enemy
			UnitData enemyData = ScriptableObject.CreateInstance<UnitData>();
			enemyData.unitName = "Test Enemy";
			enemyData.baseHealth = 50;
			enemyData.baseDamage = 5;
			enemyData.attackSpeed = 1f;
			enemyData.range = 2f;
			enemyData.moveSpeed = 2f;

			UnitInstance enemyInstance = new UnitInstance(enemyData);
			enemyController.Initialize(enemyInstance, false);
			enemyUnits.Add(enemyController);

			Debug.Log($"Spawned enemy at {spawnPos}");
		}

		Debug.Log($"Spawned {enemyUnits.Count} test enemies");
	}

	private Vector3 GetRandomSpawnPosition(Transform spawnArea)
	{
		if (spawnArea != null)
		{
			Vector3 center = spawnArea.position;
			Vector3 size = spawnArea.localScale;
			Vector3 randomPos = center + new Vector3(
				Random.Range(-size.x / 2, size.x / 2),
				0,
				Random.Range(-size.z / 2, size.z / 2)
			);
			return randomPos;
		}
		return Vector3.zero;
	}

	private IEnumerator BattleLoop()
	{
		Debug.Log("Battle loop started");

		while (isBattleActive && currentBattleTime > 0)
		{
			currentBattleTime -= Time.deltaTime;

			// Debug info every 5 seconds
			if (Mathf.FloorToInt(currentBattleTime) % 5 == 0)
			{
				Debug.Log($"Battle: {playerUnits.Count} players vs {enemyUnits.Count} enemies, Time: {currentBattleTime:F1}");
			}

			// Check win/lose conditions
			if (playerUnits.Count == 0)
			{
				Debug.Log("Player lost - no units remaining");
				EndBattle(false);
				yield break;
			}

			if (enemyUnits.Count == 0)
			{
				Debug.Log("Player won - no enemies remaining");
				EndBattle(true);
				yield break;
			}

			yield return null;
		}

		// Time's up
		if (isBattleActive)
		{
			Debug.Log("Battle ended - time's up");
			EndBattle(false);
		}
	}

	public void EndBattle(bool playerWon)
	{
		if (!isBattleActive) return;

		isBattleActive = false;

		if (battleCoroutine != null)
		{
			StopCoroutine(battleCoroutine);
			battleCoroutine = null;
		}

		Debug.Log($"Battle ended. Player won: {playerWon}");

		// Clean up units
		foreach (UnitController unit in playerUnits)
		{
			if (unit != null)
				Destroy(unit.gameObject);
		}
		foreach (UnitController unit in enemyUnits)
		{
			if (unit != null)
				Destroy(unit.gameObject);
		}

		playerUnits.Clear();
		enemyUnits.Clear();

		// Give rewards if won
		if (playerWon)
		{
			int soulsEarned = CalculateSoulReward();
			GameManager.Instance.AddSouls(soulsEarned);
			GameManager.Instance.currentRunLevel++;
			GameManager.Instance.ChangeState(GameState.Shop);
		}
		else
		{
			GameManager.Instance.ChangeState(GameState.GameOver);
		}
	}

	private int CalculateSoulReward()
	{
		// Base reward + bonus for surviving units
		int baseReward = 5;
		int survivalBonus = UnitManager.Instance.currentSquad.Count * 2;
		return baseReward + survivalBonus;
	}

	public void RemoveUnit(UnitController unit, bool isPlayerUnit)
	{
		if (isPlayerUnit)
			playerUnits.Remove(unit);
		else
			enemyUnits.Remove(unit);
	}
}