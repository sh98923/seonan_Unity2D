using Unity.VisualScripting;
using UnityEngine;

public class CharacterAction : MonoBehaviour
{
    enum State
    {
        Idle, Move, Attack, Dead
    }

    private Transform _target;
    [SerializeField] private float _moveSpeed = 1.0f;

    private State _state = State.Move;

    private void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Enemy").GetComponentInChildren<Transform>();

        if(_target == null) Debug.Log("error") ;

        GameStartManager.CharacterMoveEvnet += Move;
    }
    private void Update()
    {
        switch (_state)
        {
            case State.Idle:
                break;
            case State.Move:
                MoveToTarget();
                break;
            case State.Attack:
                break;
        }
    }

    public void Move()
    {
        print("Move");
    }

    private void FindEnemy()
    {
        
    }

    private void MoveToTarget()
    {
        if (!_target)
        {
            Debug.Log("targeterror");
            return;
        }
        

        Vector3 direction = _target.position - transform.position;
        float distance = direction.magnitude;

        transform.Translate(direction.normalized * _moveSpeed * Time.deltaTime);
    }
}
