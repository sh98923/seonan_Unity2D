using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static UnityEngine.GraphicsBuffer;

public abstract class Character : MonoBehaviour
{
    //public static Character Instance { get; private set; }

    protected enum State
    {
        Idle, Move, Attack, Dead
    }

    protected Animator _animator;
    protected Transform _target;

    protected abstract int _targetLayerMask { get; }
    protected abstract int _characterAtk { get; }
    protected abstract float _characterRange {  get; }

    [SerializeField] protected string _baseAttackEffectKey;
    [SerializeField] protected Transform _baseAttackSpawnPos;

    [SerializeField]
    protected int _moveSpeed = 2;
    protected int _currentHp;

    protected bool _isDead = false;
    protected bool _isAttacking = false;

    protected State _curState = State.Idle;

    protected virtual void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    protected virtual void OnEnable()
    {
        if(GameStartManager.Instance != null)
        {
            GameStartManager.Instance.BattleStartEvent += StartBattle;
        }
    }

    protected virtual void OnDisable()
    {
        if(GameStartManager.Instance != null)
        {
            GameStartManager.Instance.BattleStartEvent -= StartBattle;
        }
    }

    protected virtual void Update()
    {
        if (_curState == State.Dead)
            return;

        if (GameStartManager.Instance != null && GameStartManager.Instance.IsButtonClicked)
        {
            StartBattle();
        }

        if (Input.GetKeyDown(KeyCode.J) && !_isDead)
        {
            Die();
        }

        switch (_curState)
        {
            case State.Idle:
                _animator.SetFloat("Speed", 0);
                break;
            case State.Move:
                _animator.SetFloat("Speed", 1.0f);
                MoveToTarget();
                break;
            case State.Attack:
                ManageAttackState();
                break;
            case State.Dead:
                _animator.SetBool("isDead", true);
                break;
        }
    }

    protected virtual void SetIdle()
    {
        _curState = State.Idle;
    }

    protected virtual void StartBattle()
    {
        if (_curState != State.Idle) return;

        _curState = State.Move;
        //Debug.Log(_curState);
    }

    protected virtual bool SetTarget()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 50, _targetLayerMask);

        if (colliders.Length == 0)
        {
            _target = null;
            return false;
        }

        float minDistance = float.MaxValue;

        _target = null;

        foreach(Collider2D collider in colliders)
        {
            Character targetCharacter = collider.GetComponentInChildren<Character>();

            //죽은 상태 예외 처리
            if(targetCharacter != null && !targetCharacter._isDead)
            {
                float distance = Vector2.Distance(transform.position, collider.transform.position);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    _target = collider.transform;
                }
            }
        }

        return _target != null;
    }

    protected virtual void MoveToTarget()
    {
        SetTarget();

        if (_target == null)
            return;

        Vector3 direction = _target.position - transform.position;
        float distance = direction.magnitude;

        if(distance < _characterRange)
        {
            _animator.SetFloat("Speed", 0);
            _curState = State.Attack;
            StartAttack();
            return;
        }

        transform.Translate(-direction.normalized * _moveSpeed * Time.deltaTime);
    }

    protected virtual void StartAttack()
    {
        if (_curState != State.Attack || _isAttacking) return;

        _isAttacking = true;
        _animator.SetTrigger("Attack");
    }

    public virtual void SpawnEffect()
    {
        GameObject effect = PoolingManager.Instance.Pop(_baseAttackEffectKey);

        if(effect != null)
        {
            effect.transform.position = _baseAttackSpawnPos.position;
            effect.transform.rotation = Quaternion.identity;

            BaseAttack baseAttack = effect.GetComponent<BaseAttack>();

            if(baseAttack != null)
            {
                baseAttack.Initialize(_target, _characterAtk, 5f);
            }
        }
    }

    protected virtual void ManageAttackState()
    {
        if (_target == null)
        {
            Debug.Log("new target trace");

            if (SetTarget())
            {
                _curState = State.Move;
            }   
            else
            {
                SetIdle();
                Debug.Log(_curState);
                return;
            }
        }
        else
        {
            StartAttack();
        }
    }

    public virtual void TakeDamage(int damage)
    {
        if (_curState == State.Dead)
            return;

        _currentHp -= damage;
        Debug.Log("Hp : " + _currentHp);

        if ( _currentHp <= 0 )
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        if(_isDead) return;

        _isDead = true;
        _curState = State.Dead;
        _animator.SetTrigger("Death");

        OnDeath();
    }

    protected virtual void OnDeath()
    {
        PoolingManager.Instance.Release(gameObject.name, gameObject);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _characterRange);
    }
}
