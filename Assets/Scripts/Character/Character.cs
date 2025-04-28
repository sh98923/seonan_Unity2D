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
    private Collider _collider;

    [SerializeField]
    private int _moveSpeed = 3;

    private int _enemyLayerMask;

    private State _curState;

    private void Awake()
    {
        DataManager.Instance.LoadData();

        _character = GetComponent<GameObject>();
        _animator = GetComponentInChildren<Animator>();
        _collider = GetComponent<Collider>();

        _enemyLayerMask = LayerMask.GetMask("Enemy");

    }

    private void Update()
    {
        switch(_curState)
        {
            case State.Idle:
                break;
            case State.Move:
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
    private void SetTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _characterData.Range, _enemyLayerMask);

        float minDistance = float.MaxValue;

        _target = null;

        foreach(Collider collider in colliders)
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

        if (_target == null) return;

        Vector3 direction = _target.position - transform.position;
        float distance = direction.magnitude;

        transform.Translate(direction.normalized * _moveSpeed * Time.deltaTime);
    }
}
