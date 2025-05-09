using UnityEngine;
using System;

public class Enemy : Character
{
    private EnemyData _enemyData;

    protected override int _targetLayerMask => LayerMask.GetMask("Player");
    protected override int _characterAtk => _enemyData.Atk;
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

    protected override void MoveToTarget()
    {
        SetTarget();

        if (_target == null)
            return;

        Vector3 direction = _target.position - transform.position;
        float distance = direction.magnitude;

        if (distance < _characterRange)
        {
            _animator.SetFloat("Speed", 0);
            _curState = State.Attack;
            StartAttack();
            return;
        }

        transform.Translate(direction.normalized * _moveSpeed * Time.deltaTime);
    }

    protected override void Die()
    {
        base.Die();
        OnEnemyDefeated?.Invoke(this);
        Destroy(gameObject, 1f);
    }
}
