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
    private GameObject _character;
    private Animator _animator;
    private Transform _target;

    [SerializeField]
    private int _moveSpeed = 3;

    private int _enemyLayerMask;

    private State _curState = State.Idle;

    private void Awake()
    {
        DataManager.Instance.LoadData();

        //_characterData = DataManager.Instance.GetCharacterData(8);
        int totalCharacters = DataManager.Instance.GetTotalCharacterCount();
        
        for (int i = 1; i < totalCharacters; i++)
        {
            _characterData = DataManager.Instance.GetCharacterData(i);
        }

        //_character = gameObject;
        _animator = GetComponentInChildren<Animator>();

        _enemyLayerMask = LayerMask.GetMask("Enemy");

    }

    private void Start()
    {
        
    }

    private void Update()
    {

        if (GameStartManager.Instance != null && GameStartManager.Instance.IsButtonClicked && _curState == State.Idle)
            StartBattle();

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
                break;
            case State.Dead:
                break;
        }
    }

    public void SetIdle()
    {
        _curState = State.Idle;
        Debug.Log(_curState);
    }

    public void StartBattle()
    {
        if (_curState != State.Idle) return;

        _curState = State.Move;
        Debug.Log(_curState);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _characterData.Range);
    }

    private void SetTarget()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 50, _enemyLayerMask);

        if (colliders.Length == 0) return;

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
    }

    private void MoveToTarget()
    {
        //if (!_target)
        //{
        //    Debug.Log("targeterror");
        //    return;
        //}
        //
        //Vector3 direction = _target.position - transform.position;
        //float distance = direction.magnitude;
        //
        //transform.Translate(direction.normalized * _moveSpeed * Time.deltaTime);

        SetTarget();

        bool hasLogged = false;

        if (_target == null && !hasLogged)
        {
            Debug.Log("null");
            hasLogged = true;
            return;
        }

        Vector3 direction = _target.position - transform.position;
        float distance = direction.magnitude;

        if(distance < _characterData.Range)
        {
            _animator.SetFloat("Speed", 0);
            _curState = State.Attack;
            return;
        }

        transform.Translate(direction.normalized * _moveSpeed * Time.deltaTime);

        Debug.Log(_target);
    }
}
