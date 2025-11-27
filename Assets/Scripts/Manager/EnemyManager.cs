using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    [SerializeField] private EnemyList enemyUnits;

    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform activeParent;
    [SerializeField] private Transform poolParent;

    private List<EnemyUnit> activeEnemys = new List<EnemyUnit>();

    public List<EnemyUnit> ActiveEnemys => activeEnemys;

    private readonly Dictionary<EnemyUnit, List<EnemyUnit>> pools = new();

    private new void Awake()
    {
    }

    public EnemyUnit RandomEnemy(MonsterType type)
    {
        if (enemyUnits == null || enemyUnits.EnemyUnits.Length == 0)
            return null;

        List<EnemyUnit> randomList = new List<EnemyUnit>();

        foreach (var enemy in enemyUnits.EnemyUnits)
        {
            if (enemy.Data._MonsterType == type)
                randomList.Add(enemy);
        }

        int index = Random.Range(0, randomList.Count);
        return randomList[index];
    }

    public EnemyUnit SpawnEnemy(MonsterType type)
    {
        EnemyUnit enemy = RandomEnemy(type);
        if (enemy == null)
            return null;

        return FromPool(enemy);
    }

    public EnemyUnit FromPool(EnemyUnit enemy)
    {
        if (enemy == null)
            return null;

        if (pools.TryGetValue(enemy, out var pool) == false)
        {
            pool = new List<EnemyUnit>();
            pools[enemy] = pool;
        }

        EnemyUnit instance = null;

        foreach (var enemys in pool)
        {
            if (enemys.gameObject.activeSelf == false)
            {
                instance = enemys;
                break;
            }
        }

        if (instance == null)
        {
            Vector3 pos = spawnPoint.position;
            instance = Instantiate(enemy, pos, Quaternion.identity, activeParent);
            pool.Add(instance);
        }
        else
        {
            Vector3 pos = spawnPoint.position;
            instance.transform.position = pos;
            instance.transform.SetParent(activeParent);
        }

        instance.ResetStatus();
        instance.gameObject.SetActive(true);

        OnActiveEnemy(instance);

        return instance;
    }

    private void OnActiveEnemy(EnemyUnit enemy)
    {
        if (activeEnemys.Contains(enemy) == false)
            activeEnemys.Add(enemy);
    }

    private void OnUnActiveEnemy(EnemyUnit enemy)
    {
        if (activeEnemys.Contains(enemy))
            activeEnemys.Remove(enemy);
    }

    public void OnEnemyDead(EnemyUnit enemy)
    {
        OnUnActiveEnemy(enemy);

        if (enemy.Data._MonsterType == MonsterType.Boss)
        {
            GameManager.Instance?.HandleBossClear();
            ReturnPool(enemy);
            return;
        }


        ReturnPool(enemy);

        if (activeEnemys.Count == 0)
            GameManager.Instance?.EndBattle();

    }

    private void ReturnPool(EnemyUnit enemy)
    {
        enemy.gameObject.SetActive(false);

        if (poolParent != null)
            enemy.transform.SetParent(poolParent);
        else
            enemy.transform.SetParent(transform);
    }

    public bool HasEnemy()
    {
        return activeEnemys.Count > 0;
    }

    public void ResetAll()
    {
        foreach (var enemy in activeEnemys)
        {
            if (enemy != null)
            {
                enemy.gameObject.SetActive(false);
                enemy.transform.SetParent(poolParent);
            }
        }

        activeEnemys.Clear();
        pools.Clear();
    }
}
