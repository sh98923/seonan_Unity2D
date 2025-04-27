using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static UnityEngine.GraphicsBuffer;

public class Character : MonoBehaviour
{
    private enum State
    {
        Idle, Move, Attack, Dead
    }

    private GameObject _character;
    private Animator _animator;
    [SerializeField] private float _moveSpeed = 1.0f;

    private State _curState;

    private void Awake()
    {
        _character = GetComponent<GameObject>();
        _animator = GetComponentInChildren<Animator>();

    }

    private void Update()
    {
        switch(_curState)
        {
            case State.Idle:
                break;
            case State.Move:
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
        _animator.Play("IDLE");
    }

    public void SetMove()
    {
        _curState = State.Move;
        Debug.Log(_curState);
        _animator.Play("MOVE");
    }

    //private void MoveToTarget()
    //{
    //    if (!_target)
    //    {
    //        Debug.Log("targeterror");
    //        return;
    //    }
    //
    //    Vector3 direction = _target.position - transform.position;
    //    float distance = direction.magnitude;
    //
    //    transform.Translate(direction.normalized * _moveSpeed * Time.deltaTime);
    //}
}
