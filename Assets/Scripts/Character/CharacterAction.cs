using Unity.VisualScripting;
using UnityEngine;

public class CharacterAction : MonoBehaviour
{
    private Transform _target;
    [SerializeField] private float _moveSpeed = 1.0f;

    private void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Enemy").GetComponentInChildren<Transform>();

        if(_target == null) Debug.Log("error") ;

        GameStartManager.CharacterMoveEvnet += Move;
    }
    private void Update()
    {
        MoveToTarget();
    }

    public void Move()
    {
        print("Move");
    }

    public void MoveToTarget()
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
