using UnityEngine;

public class FSM : MonoBehaviour
{
    private GameObject _characterPrefab;
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    
}
