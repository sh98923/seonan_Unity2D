using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static UnityEngine.GraphicsBuffer;

public class Character : MonoBehaviour
{
    private enum State
    {
        Idle, Move, Attack, Dead
    }

    private CharacterData _characterData;
    private Animator _animator;
    private Transform _target;

    [SerializeField]
    private int _moveSpeed = 3;

    private int _enemyLayerMask;

    private State _curState = State.Idle;

    private void Awake()
    {
        DataManager.Instance.LoadData();

        int totalCharacters = DataManager.Instance.GetTotalCharacterCount();
        
        for (int i = 1; i < totalCharacters; i++)
        {
            _characterData = DataManager.Instance.GetCharacterData(i);
        }

        _animator = GetComponentInChildren<Animator>();

        _enemyLayerMask = LayerMask.GetMask("Enemy");

    }
    private void OnEnable()
    {
        if(GameStartManager.Instance != null)
        {
            GameStartManager.Instance.BattleStartEvent += StartBattle;
        }
    }
    private void OnDisable()
    {
        if(GameStartManager.Instance != null)
        {
            GameStartManager.Instance.BattleStartEvent -= StartBattle;
        }
    }

    private void Update()
    {
        if (GameStartManager.Instance != null && GameStartManager.Instance.IsButtonClicked)
        {
            StartBattle();
            GameStartManager.Instance.ResetButtonClicked();
        }

        switch(_curState)
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
                break;
        }
    }
    private void SetIdle()
    {
        _curState = State.Idle;
    }

    public void StartBattle()
    {
        if (_curState != State.Idle) return;

        _curState = State.Move;
        Debug.Log(_curState);
    }

    private bool SetTarget()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 50, _enemyLayerMask);

        if (colliders.Length == 0)
        {
            _target = null;
            return false;
        }

        float minDistance = float.MaxValue;

        _target = null;

        foreach(Collider2D collider in colliders)
        {
            float distance = Vector2.Distance(transform.position, collider.transform.position);

            if(distance < minDistance)
            {
                minDistance = distance;
                _target = collider.transform;
            }
        }

        return _target != null;
    }

    private void MoveToTarget()
    {
        SetTarget();

        bool hasLogged = false;

        if (_target == null && !hasLogged)
        {
            //Debug.Log("null");
            hasLogged = true;
            return;
        }

        Vector3 direction = _target.position - transform.position;
        float distance = direction.magnitude;

        if(distance < _characterData.Range)
        {
            _animator.SetFloat("Speed", 0);
            _curState = State.Attack;
            StartAttack();
            return;
        }

        transform.Translate(direction.normalized * _moveSpeed * Time.deltaTime);
    }

    private void StartAttack()
    {
        if (_curState != State.Attack) return;

        _animator.SetTrigger("Attack");

        //Debug.Log("Attack");
    }

    private void ManageAttackState()
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
    public void Initialize(CharacterData data)
    {
        _characterData = data;

        // 초기화 작업 (체력, 데미지 등)
        Debug.Log($"캐릭터 생성: ID={data.Name}, 체력={data.Hp}, 공격력={data.Atk}");
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _characterData.Range);
    }
}
