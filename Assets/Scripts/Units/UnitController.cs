using UnityEngine;

public class UnitController : MonoBehaviour
{
    [Header("Unit References")]
    public UnitInstance unitInstance;
    public bool isPlayerUnit = true;

    [Header("Combat")]
    public UnitController currentTarget;
    public float lastAttackTime = 0f;

    private void Start()
    {
        if (unitInstance != null)
        {
            InitializeVisuals();
        }
    }

    public void Initialize(UnitInstance instance, bool isPlayer)
    {
        unitInstance = instance;
        isPlayerUnit = isPlayer;
        InitializeVisuals();
    }

    private void InitializeVisuals()
    {
        // Set color based on element and player/enemy status
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            if (isPlayerUnit)
            {
                renderer.material.color = GetElementColor(unitInstance.unitData.element);
            }
            else
            {
                renderer.material.color = Color.red; // Enemies are red
            }
        }

        gameObject.name = unitInstance.unitData.unitName + (isPlayerUnit ? " (Player)" : " (Enemy)");
    }

    private Color GetElementColor(Element element)
    {
        switch (element)
        {
            case Element.Earth: return new Color(0.55f, 0.27f, 0.07f); // Brown
            case Element.Fire: return new Color(1f, 0.4f, 0f); // Orange
            case Element.Water: return Color.blue;
            case Element.Lightning: return Color.yellow;
            case Element.Wind: return Color.cyan;
            default: return Color.white;
        }
    }

    private void Update()
    {
        if (unitInstance == null || !unitInstance.isAlive) return;

        FindTarget();
        MoveTowardsTarget();
        TryAttack();
    }

    private void FindTarget()
    {
        // Simple target finding - nearest enemy
        UnitController[] allUnits = FindObjectsByType<UnitController>(FindObjectsSortMode.None);
        UnitController nearestEnemy = null;
        float nearestDistance = float.MaxValue;

        foreach (UnitController unit in allUnits)
        {
            if (unit.isPlayerUnit != isPlayerUnit && unit.unitInstance.isAlive)
            {
                float distance = Vector3.Distance(transform.position, unit.transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestEnemy = unit;
                }
            }
        }

        currentTarget = nearestEnemy;
    }

    private void MoveTowardsTarget()
    {
        if (currentTarget == null || !currentTarget.unitInstance.isAlive) return;

        float distanceToTarget = Vector3.Distance(transform.position, currentTarget.transform.position);

        // Move if not in attack range
        if (distanceToTarget > unitInstance.unitData.range)
        {
            Vector3 direction = (currentTarget.transform.position - transform.position).normalized;
            transform.position += direction * unitInstance.unitData.moveSpeed * Time.deltaTime;
        }
    }

    private void TryAttack()
    {
        if (currentTarget == null || !currentTarget.unitInstance.isAlive) return;

        float distanceToTarget = Vector3.Distance(transform.position, currentTarget.transform.position);

        if (distanceToTarget <= unitInstance.unitData.range)
        {
            if (Time.time - lastAttackTime >= 1f / unitInstance.unitData.attackSpeed)
            {
                Attack();
                lastAttackTime = Time.time;
            }
        }
    }

    private void Attack()
    {
        if (currentTarget != null && currentTarget.unitInstance.isAlive)
        {
            int damage = unitInstance.GetDamage();
            currentTarget.TakeDamage(damage);
            Debug.Log($"{unitInstance.unitData.unitName} attacks {currentTarget.unitInstance.unitData.unitName} for {damage} damage");
        }
    }

    public void TakeDamage(int damage)
    {
        if (unitInstance.isAlive)
        {
            unitInstance.TakeDamage(damage);
            Debug.Log($"{unitInstance.unitData.unitName} takes {damage} damage. HP: {unitInstance.currentHealth}/{unitInstance.GetMaxHealth()}");

            if (!unitInstance.isAlive)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        Debug.Log($"{unitInstance.unitData.unitName} died");
        if (BattleManager.Instance != null)
        {
            BattleManager.Instance.RemoveUnit(this, isPlayerUnit);
        }
        Destroy(gameObject);
    }
}