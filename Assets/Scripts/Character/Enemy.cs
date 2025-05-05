using UnityEngine;

public class Enemy : Character
{
    private EnemyData _enemyData;

    protected override int _targetLayerMask => LayerMask.GetMask("Player");
    protected override float _characterRange => _enemyData.Range;

    protected override void Awake()
    {
        int totalEnemies = DataManager.Instance.GetTotalEnemyCount();

        for (int i = 100; i < totalEnemies; i++)
        {
            _enemyData = DataManager.Instance.GetEnemyData(i);
        }

        _currentHp = _enemyData.Hp;
        base.Awake();
    }

    public void Initialize(EnemyData data)
    {
        _enemyData = data;
        _enemyData.Hp = _currentHp;
    }
}
