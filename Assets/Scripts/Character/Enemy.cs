using UnityEngine;
using System;

public class Enemy : Character
{
    private EnemyData _enemyData;

    protected override int _targetLayerMask => LayerMask.GetMask("Player");
    protected override float _characterRange => _enemyData.Range;

    public event Action<Enemy> OnEnemyDefeated;

    protected override void Awake()
    {
        int totalEnemies = DataManager.Instance.GetTotalEnemyCount();

        for (int i = 100; i < totalEnemies; i++)
        {
            _enemyData = DataManager.Instance.GetEnemyData(i);
        }

        base.Awake();
    }

    public void Initialize(EnemyData data)
    {
        _enemyData = data;
        _currentHp = _enemyData.Hp;
    }

    protected override void Die()
    {
        base.Die();
        OnEnemyDefeated?.Invoke(this);
        Destroy(gameObject, 1f);
    }
}
