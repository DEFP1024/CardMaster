using UnityEngine;

[CreateAssetMenu(fileName = "EnemyList", menuName = "Scriptable Objects/EnemyList")]
public class EnemyList : ScriptableObject
{
    [SerializeField] EnemyUnit[] enemyUnits;

    public EnemyUnit[] EnemyUnits => enemyUnits;
}
