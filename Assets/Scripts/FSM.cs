using UnityEngine;

public class FSM : MonoBehaviour
{
    private GameObject _characterPrefab;
    private GameObject _enemyPrefab;
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        FindNearEnemy();
    }

    private void FindNearEnemy()
    {
        float distance = Vector3.Distance(_characterPrefab.transform.position, _enemyPrefab.transform.position);

        if(distance < 3)
        {
            
        }
    }
}
